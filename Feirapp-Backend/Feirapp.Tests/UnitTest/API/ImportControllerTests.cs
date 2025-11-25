using System.Threading;
using System.Threading.Tasks;
using Feirapp.API.Controllers;
using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Feirapp.Tests.UnitTest.API;

public class ImportControllerTests
{
    #region UpdateNcmAndCestsDetails

    [Fact]
    public async Task UpdateNcmAndCestsDetails_OnSuccess_ReturnStatus200()
    {
        // Arrange
        var ncmCestDataScrapper = Substitute.For<INcmCestDataScrapper>();
        await ncmCestDataScrapper.UpdateNcmAndCestsDetailsAsync(Arg.Any<CancellationToken>());

        var sut = new ImportController(ncmCestDataScrapper);
        
        // Act
        var result = await sut.UpdateNcmAndCestsDetails(CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(200);
        objectResult?.Value.Should().BeOfType<ApiResponse<bool>>();
        await ncmCestDataScrapper.Received(1).UpdateNcmAndCestsDetailsAsync(Arg.Any<CancellationToken>());
    }
    
    #endregion
}