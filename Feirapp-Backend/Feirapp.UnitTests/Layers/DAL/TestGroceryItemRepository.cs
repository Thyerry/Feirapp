using System.Collections.Generic;
using System.Threading.Tasks;
using Feirapp.DAL.Repositories;
using Feirapp.Domain.Models;
using FluentAssertions;
using Xunit;

namespace Feirapp.UnitTests.Layers.DAL;

public class TestGroceryItemRepository
{
    [Fact]
    public async Task GetAllGroceryItems_ReturnListOfGroceryItems()
    {
        // Arrange
        var sut = new GroceryItemRepository();
        
        // Act
        var result = await sut.GetAllGroceryItems();
        
        // Assert
        result.Should().BeOfType<List<GroceryItem>>();
    }

    [Fact]
    public async Task GetAllGroceryItems_AccessDataBase()
    {
        // Arrange
        var sut = new GroceryItemRepository();
        
        // Act
        var result = await sut.GetAllGroceryItems();
        
        // Assert
        result.Should().BeOfType<List<GroceryItem>>();
    }
}