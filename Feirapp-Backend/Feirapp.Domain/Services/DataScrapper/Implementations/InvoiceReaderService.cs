using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Entities.Enums;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace Feirapp.Domain.Services.DataScrapper.Implementations;

public class InvoiceReaderService(IOptions<SefazPE> options) : IInvoiceReaderService
{
    private readonly SefazPE _sefazPe = options.Value;

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

        var store = new InvoiceScanStore(
            Name: storeNameXml.SelectSingleNode("//xnome").InnerText,
            Cnpj: storeNameXml.SelectSingleNode("//cnpj").InnerText,
            Cep: storeNameXml.SelectSingleNode("//enderemit//cep").InnerText,
            Street: storeNameXml.SelectSingleNode("//enderemit//xlgr").InnerText,
            StreetNumber: storeNameXml.SelectSingleNode("//enderemit//nro").InnerText,
            Neighborhood: storeNameXml.SelectSingleNode("//enderemit//xbairro").InnerText,
            CityName: storeNameXml.SelectSingleNode("//enderemit//xmun").InnerText,
            State: storeNameXml.SelectSingleNode("//enderemit//uf").InnerText
        );

        var groceryItems = GetGroceryItemList(groceryItemXmlList, purchaseDateXml);

        return new InvoiceImportResponse(invoiceCode, store, groceryItems);                                              
    }

    private static List<InvoiceScanGroceryItem> GetGroceryItemList(HtmlNodeCollection groceryItemXmlList, HtmlNode purchaseDateXml)
    {
        var result = new List<InvoiceScanGroceryItem>();
        foreach (var groceryItemXml in groceryItemXmlList)
        {
            var xpath = groceryItemXml.XPath;

            var cest = groceryItemXml.SelectSingleNode($"{xpath}/cest")?.InnerText;
            var ncm = groceryItemXml.SelectSingleNode($"{xpath}/ncm").InnerText;
            var productCode = groceryItemXml.SelectSingleNode($"{xpath}/cprod").InnerText;
            var cean = groceryItemXml.SelectSingleNode($"{xpath}/cean")?.InnerText;

            var groceryItem = new InvoiceScanGroceryItem
            (
                Name: groceryItemXml.SelectSingleNode($"{xpath}/xprod").InnerText,
                Price: ToDecimal(groceryItemXml.SelectSingleNode($"{xpath}/vuncom").InnerText),
                MeasureUnit: groceryItemXml.SelectSingleNode($"{xpath}/ucom").InnerText.NormalizeMeasureUnit(),
                Barcode: cean ?? string.Empty,
                ProductCode: productCode,
                PurchaseDate: DateTime.Parse(purchaseDateXml.InnerText),
                NcmCode: ncm ?? string.Empty,
                CestCode: cest ?? string.Empty
            )
            {
                Quantity = ToDecimal(groceryItemXml.SelectSingleNode($"{xpath}/qcom").InnerText)
            };

            var objectExists = result.FirstOrDefault(g =>
                groceryItem.Name == g.Name
                && groceryItem.Price == g.Price
                && groceryItem.MeasureUnit == g.MeasureUnit
                && groceryItem.MeasureUnit != MeasureUnitEnum.KILO.StringValue()
                && groceryItem.Barcode == g.Barcode
                && groceryItem.NcmCode == g.NcmCode
                && groceryItem.CestCode == g.CestCode);
            
            if (objectExists is null)
                result.Add(groceryItem);
            else
                objectExists.Quantity += groceryItem.Quantity;
        }

        return result;
    }

    private static decimal ToDecimal(string text) => Convert.ToDecimal(string.Join(",", text.Split(".")));
}