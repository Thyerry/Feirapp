using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Entities.Enums;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace Feirapp.Domain.Services.DataScrapper.Implementations;

public class InvoiceReaderService : IInvoiceReaderService
{
    private readonly SefazPE _sefazPe;
    private readonly IGroceryItemService _groceryItemService;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceReaderService"/> class.
    /// </summary>
    /// <param name="options">The options for the SefazPE service.</param>
    /// <param name="groceryItemService"></param>
    public InvoiceReaderService(IOptions<SefazPE> options, IGroceryItemService groceryItemService)
    {
        _groceryItemService = groceryItemService;
        _sefazPe = options.Value;
    }

    /// <summary>
    /// Scrapes invoice data asynchronously.
    /// </summary>
    /// <param name="invoiceCode">The invoice code.</param>
    /// <param name="isInsert"></param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of invoice grocery items.</returns>
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

        return new InvoiceImportResponse(store, groceryItems);                                              
    }

    private static List<InvoiceScanGroceryItem> GetGroceryItemList(HtmlNodeCollection groceryItemXmlList, HtmlNode purchaseDateXml)
    {
        var result = new List<InvoiceScanGroceryItem>();
        foreach (var groceryItemXml in groceryItemXmlList)
        {
            var xpath = groceryItemXml.XPath;

            var cest = groceryItemXml.SelectSingleNode($"{xpath}/cest")?.InnerText;
            var ncm = groceryItemXml.SelectSingleNode($"{xpath}/ncm").InnerText;
            var groceryItem = new InvoiceScanGroceryItem
            (
                Name: groceryItemXml.SelectSingleNode($"{xpath}/xprod").InnerText,
                Price: ToDecimal(groceryItemXml.SelectSingleNode($"{xpath}/vuncom").InnerText),
                MeasureUnit: groceryItemXml.SelectSingleNode($"{xpath}/ucom").InnerText.NormalizeMeasureUnit(),
                Barcode: groceryItemXml.SelectSingleNode($"{xpath}/cean").InnerText,
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

    /// <summary>
    /// Converts a string to a decimal.
    /// </summary>
    /// <param name="text">The string to be converted.</param>
    /// <returns>
    /// A decimal number that is equivalent to the number in the string, or 0 (zero) if the conversion fails.
    /// </returns>
    private static decimal ToDecimal(string text) => Convert.ToDecimal(string.Join(",", text.Split(".")));
}