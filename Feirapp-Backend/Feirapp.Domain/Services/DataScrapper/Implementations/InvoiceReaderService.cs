using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace Feirapp.Domain.Services.DataScrapper.Implementations;

public class InvoiceReaderService : IInvoiceReaderService
{
    private readonly SefazPE _sefazPe;

    public InvoiceReaderService(IOptions<SefazPE> options)
    {
        _sefazPe = options.Value;
    }

    public async Task<List<InvoiceGroceryItem>> InvoiceDataScrapperAsync(string invoiceCode, CancellationToken ct)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(invoiceCode, ct);

        var groceryItemXmlList = doc.DocumentNode.SelectNodes("//prod");
        var storeNameXml = doc.DocumentNode.SelectSingleNode("//emit");
        var purchaseDateXml = doc.DocumentNode.SelectSingleNode("//ide//dhemi");
        var groceryItemList = new List<InvoiceGroceryItem>();
        var store = new InvoiceStore
        (
            Name: storeNameXml.SelectSingleNode("//xnome").InnerText,
            Cnpj: storeNameXml.SelectSingleNode("//cnpj").InnerText,
            Cep: storeNameXml.SelectSingleNode("//enderEmit//CEP").InnerText,
            Street: storeNameXml.SelectSingleNode("//enderEmit//xLgr").InnerText,
            StreetNumber: storeNameXml.SelectSingleNode("//enderEmit//nro").InnerText,
            Neighborhood: storeNameXml.SelectSingleNode("//enderEmit//xBairro").InnerText,
            CityName: storeNameXml.SelectSingleNode("//enderEmit//xMun").InnerText,
            State: storeNameXml.SelectSingleNode("//enderEmit//UF").InnerText,
            Country: storeNameXml.SelectSingleNode("//enderEmit//xPais").InnerText
        );

        foreach (var groceryItemXml in groceryItemXmlList)
        {
            var xpath = groceryItemXml.XPath;

            var cest = groceryItemXml.SelectSingleNode($"{xpath}/cest")?.InnerText;
            var ncm = groceryItemXml.SelectSingleNode($"{xpath}/ncm").InnerText;
            var groceryItem = new InvoiceGroceryItem
            (
                Name: groceryItemXml.SelectSingleNode($"{xpath}/xprod").InnerText,
                Price: ToDecimal(groceryItemXml.SelectSingleNode($"{xpath}/vuncom").InnerText),
                MeasureUnit: groceryItemXml.SelectSingleNode($"{xpath}/ucom").InnerText,
                Barcode: groceryItemXml.SelectSingleNode($"{xpath}/cean").InnerText,
                PurchaseDate: DateTime.Parse(purchaseDateXml.InnerText),
                Ncm: (!string.IsNullOrWhiteSpace(ncm) ? ToNcmFormat(ncm) : null) ?? string.Empty,
                Cest: (!string.IsNullOrWhiteSpace(cest) ? ToCestFormat(cest) : null) ?? string.Empty,
                Store: store
            )
            {
                Quantity = ToDecimal(groceryItemXml.SelectSingleNode($"{xpath}/qcom").InnerText)
            };

            var objectExists = groceryItemList.FirstOrDefault(g => g.Barcode == groceryItem.Barcode);
            if (objectExists is null)
                groceryItemList.Add(groceryItem);
            else
                objectExists.Quantity += groceryItem.Quantity;
        }
        return groceryItemList;
    }

    private static decimal ToDecimal(string text) => Convert.ToDecimal(string.Join(",", text.Split(".")));

    private static string? ToCestFormat(string text) => Convert.ToInt64(text).ToString(@"00\.000\.00");

    private static string? ToNcmFormat(string text) =>
        text.Length switch
        {
            4 => Convert.ToInt64(text).ToString(@"0000"),
            5 => Convert.ToInt64(text).ToString(@"0000\.0"),
            6 => Convert.ToInt64(text).ToString(@"0000\.00"),
            7 => Convert.ToInt64(text).ToString(@"0000\.00\.0"),
            8 => Convert.ToInt64(text).ToString(@"0000\.00\.00"),
            _ => null
        };
}