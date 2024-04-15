﻿using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Dtos.Commands;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
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
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of invoice grocery items.</returns>
    public async Task<InvoiceImportResponse> InvoiceDataScrapperAsync(string invoiceCode, CancellationToken ct)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(_sefazPe.SefazUrl.Replace("{INVOICE_CODE}", invoiceCode), ct);

        var groceryItemXmlList = doc.DocumentNode.SelectNodes("//prod");
        var storeNameXml = doc.DocumentNode.SelectSingleNode("//emit");
        var purchaseDateXml = doc.DocumentNode.SelectSingleNode("//ide//dhemi");
        
        var store = new InvoiceStore
        (
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
        var insertCommand = new InsertGroceryItemCommand(groceryItems, store);
        await _groceryItemService.InsertBatchAsync(insertCommand, ct);

        return new InvoiceImportResponse(store, groceryItems);
    }

    private static List<InvoiceGroceryItem> GetGroceryItemList(HtmlNodeCollection groceryItemXmlList,
        HtmlNode purchaseDateXml)
    {
        var result = new List<InvoiceGroceryItem>();
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
                NcmCode: ncm ?? string.Empty,
                CestCode: cest ?? string.Empty
            )
            {
                Quantity = ToDecimal(groceryItemXml.SelectSingleNode($"{xpath}/qcom").InnerText)
            };

            var objectExists = result.FirstOrDefault(g => g.Barcode == groceryItem.Barcode);
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