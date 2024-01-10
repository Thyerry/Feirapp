using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Feirapp.DocumentModels;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Feirapp.Tests.Bdd.Steps;

[Binding]
public class AcceptanceCriteria
{
    private readonly List<GroceryItem> _groceryItems = new ();
    private readonly HttpClient _httpClient;

    [When(@"the client provides the right Grocery Item data")]
    public void WhenTheClientProvidesTheRightGroceryItemData(Table table)
    {
        _groceryItems.AddRange(table.CreateSet<GroceryItem>());
    }
    
    [When(@"all of them having the following category")]
    public void WhenAllOfThemHavingTheFollowingCategory(Table table)
    {
        var groceryCategory = table.CreateInstance<GroceryCategory>();
        foreach (var groceryItem in _groceryItems)
        {
            groceryItem.Category = groceryCategory;
        }
    }

    [Then(@"return the Grocery Item")]
    public void ThenReturnTheGroceryItem()
    {
        _groceryItems.Should().NotBeEmpty();
    }
}