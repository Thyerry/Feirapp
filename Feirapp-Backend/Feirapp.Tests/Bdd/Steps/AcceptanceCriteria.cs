using Feirapp.DocumentModels.Documents;
using FluentAssertions;
using System.Collections.Generic;
using System.Net.Http;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Feirapp.Tests.Bdd.Steps;

[Binding]
public class AcceptanceCriteria
{
    private readonly List<GroceryItem> _groceryItems = new();
    private readonly HttpClient _httpClient;

    [When(@"the client provides the right Grocery Item data")]
    public void WhenTheClientProvidesTheRightGroceryItemData(Table table)
    {
        _groceryItems.AddRange(table.CreateSet<GroceryItem>());
    }

    [Then(@"return the Grocery Item")]
    public void ThenReturnTheGroceryItem()
    {
        _groceryItems.Should().NotBeEmpty();
    }
}