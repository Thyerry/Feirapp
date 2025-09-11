using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Misc;
using Feirapp.Entities.Enums;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace Feirapp.Domain.Services.DataScrapper.Implementations;

public class InvoiceReaderService(IOptions<SefazPe> options) : IInvoiceReaderService
{
    private readonly SefazPe _sefazPe = options.Value;

    public async Task<InvoiceImportResponse> InvoiceDataScrapperAsync(string invoiceCode, bool isInsert, CancellationToken ct)
    {
        var timeout = TimeSpan.FromSeconds(15);
        using var httpClient = new HttpClient();
        httpClient.Timeout = timeout;

        var web = new HtmlWeb
        {
            PreRequest = request =>
            {
                request.Timeout = (int)timeout.TotalMilliseconds;
                return true;
            },
        };
        var url = _sefazPe.SefazUrl.Replace("{INVOICE_CODE}", invoiceCode);
        var doc = await web.LoadFromWebAsync(url, ct);

        if (doc == null)
            throw new Exception("NFC-e Service is down.");

        var err = doc.DocumentNode.SelectSingleNode("//erro");
        
        if(err == null || !string.IsNullOrWhiteSpace(err.InnerText))
            throw new Exception("NFC-e not found.");
        
        var groceryItemXmlList = doc.DocumentNode.SelectNodes("//prod");
        var storeNameXml = doc.DocumentNode.SelectSingleNode("//emit");
        var purchaseDateXml = doc.DocumentNode.SelectSingleNode("//ide//dhemi");

        if(storeNameXml == null)
            throw new Exception("Store not found.");
        
        var store = new InvoiceScanStore(
            Name: storeNameXml.SelectSingleNode("//xnome")!.InnerText,
            Cnpj: storeNameXml.SelectSingleNode("//cnpj")!.InnerText,
            Cep: storeNameXml.SelectSingleNode("//enderemit//cep")!.InnerText,
            Street: storeNameXml.SelectSingleNode("//enderemit//xlgr")!.InnerText,
            StreetNumber: storeNameXml.SelectSingleNode("//enderemit//nro")!.InnerText,
            Neighborhood: storeNameXml.SelectSingleNode("//enderemit//xbairro")!.InnerText,
            CityName: storeNameXml.SelectSingleNode("//enderemit//xmun")!.InnerText,
            State: storeNameXml.SelectSingleNode("//enderemit//uf")!.InnerText
        );

        var groceryItems = GetGroceryItemList(groceryItemXmlList!, purchaseDateXml!);

        return new InvoiceImportResponse(invoiceCode, store, groceryItems);
    }

    private static List<InvoiceScanGroceryItem> GetGroceryItemList(HtmlNodeCollection groceryItemXmlList, HtmlNode purchaseDateXml)
    {
        var items = new List<InvoiceScanGroceryItem>();
        foreach (var groceryItemXml in groceryItemXmlList)
        {
            var xpath = groceryItemXml.XPath;

            var cest = groceryItemXml.SelectSingleNode($"{xpath}/cest")?.InnerText;
            var ncm = groceryItemXml.SelectSingleNode($"{xpath}/ncm")!.InnerText;
            var productCode = groceryItemXml.SelectSingleNode($"{xpath}/cprod")!.InnerText;
            var cean = groceryItemXml.SelectSingleNode($"{xpath}/cean")?.InnerText;

            var groceryItem = new InvoiceScanGroceryItem
            {
                Name = groceryItemXml.SelectSingleNode($"{xpath}/xprod")!.InnerText,
                Price = ToDecimal(groceryItemXml.SelectSingleNode($"{xpath}/vuncom")!.InnerText),
                MeasureUnit = groceryItemXml.SelectSingleNode($"{xpath}/ucom")!.InnerText.NormalizeMeasureUnit(),
                Barcode = cean ?? string.Empty,
                ProductCode = productCode,
                PurchaseDate = DateTime.Parse(purchaseDateXml.InnerText),
                NcmCode = ncm,
                CestCode = cest ?? string.Empty,
                Quantity = ToDecimal(groceryItemXml.SelectSingleNode($"{xpath}/qcom")!.InnerText)
            };

            items.Add(groceryItem);
        }

        var result = DataValidation(items);
        
        return result;
    }

    private static List<InvoiceScanGroceryItem> DataValidation(List<InvoiceScanGroceryItem> items)
    {
        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                item.ImportIssues.Add(ImportIssuesEnum.NameIsEmpty.StringValue());

            if (item.Price <= 0)
                item.ImportIssues.Add(ImportIssuesEnum.PriceIsZeroOrNegative.StringValue());

            if ((string.IsNullOrWhiteSpace(item.Barcode) || item.Barcode == "SEM GTIN") && string.IsNullOrWhiteSpace(item.ProductCode))
                item.ImportIssues.Add(ImportIssuesEnum.NoBarcodeAndProductCode.StringValue());

            if (item.Quantity <= 0)
                item.ImportIssues.Add(ImportIssuesEnum.QuantityIsZeroOrNegative.StringValue());
            
            if(string.IsNullOrWhiteSpace(item.MeasureUnit))
                item.ImportIssues.Add(ImportIssuesEnum.MeasureUnitIsEmpty.StringValue());
            
            if (items.Count(x => x.MeasureUnit == MeasureUnitEnum.KILO.StringValue() && x.ProductCode == item.ProductCode) > 1)
                item.ImportIssues.Add(ImportIssuesEnum.MultipleSameItemByKilo.StringValue());
        }
        
        var nonKiloItems = items.Where(x => x.MeasureUnit != MeasureUnitEnum.KILO.StringValue()).ToList();
        var kiloItems = items.Where(x => x.MeasureUnit == MeasureUnitEnum.KILO.StringValue()).ToList();

        var aggregatedItems = nonKiloItems
            .GroupBy(x => new { x.Barcode, x.ProductCode })
            .Select(g =>
            {
                var first = g.First();
                var totalQuantity = g.Sum(x => x.Quantity);
                var weightedPrice = g.Sum(x => x.Price * x.Quantity) / totalQuantity;

                return new InvoiceScanGroceryItem
                {
                    Name = first.Name,
                    Price = weightedPrice,
                    MeasureUnit = first.MeasureUnit,
                    Barcode = first.Barcode,
                    ProductCode = first.ProductCode,
                    PurchaseDate = first.PurchaseDate,
                    NcmCode = first.NcmCode,
                    CestCode = first.CestCode,
                    Quantity = totalQuantity,
                    ImportIssues = first.ImportIssues
                };
            })
            .ToList();

        return aggregatedItems.Concat(kiloItems).ToList();
    }

    private static decimal ToDecimal(string text) => Convert.ToDecimal(string.Join(",", text.Split(".")));
}