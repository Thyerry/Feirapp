using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Entities.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Feirapp.Domain.Services.DataScrapper.Implementations;

public class NcmCestDataScrapper(IUnitOfWork uow) : INcmCestDataScrapper
{
    private const string BaseUrl = "https://codigocest.com.br/consulta-codigo-cest-pelo-ncm";
    
    public async Task UpdateNcmAndCestsDetails(CancellationToken ct)
    {
        var ncms = await uow.NcmRepository.GetByQuery(c => string.IsNullOrEmpty(c.Description), ct);
        var driver = new ChromeDriver();
        await driver.Navigate().GoToUrlAsync(BaseUrl);
        
        foreach (var ncm in ncms)
            await GetNcmData(ncm.Code, driver, ct);
        
        driver.Close();
    }

    private async Task GetNcmData(string ncmCode, IWebDriver driver, CancellationToken ct)
    {
        var form = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/form"));

        var ncmInput = form.FindElement(By.XPath("//*[@id=\"focusedInput\"]"));
        ncmInput.Click();
        ncmInput.Clear();
        ncmInput.SendKeys(ncmCode);

        var searchButton = form.FindElement(By.XPath("/html/body/div[1]/div[2]/form/div/div[2]/button"));
        searchButton.Click();

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        var ncmDivs = driver.FindElement(By.XPath("/html/body/div[1]/div[3]/div/div[2]/div[1]"));

        var ncmUpdate = new Ncm
        {
            Code = ncmCode,
            Description = ncmDivs.Text,
            LastUpdate = DateTime.Now
        };

        try
        {
            var cestTableRows = driver
                .FindElement(By.XPath("/html/body/div[1]/div[3]/div/div[2]/table[1]/tbody"))
                .FindElements(By.TagName("tr"));

            var cests = cestTableRows.Select(tr => new Cest
            {
                Code = tr.FindElements(By.TagName("td"))[1].Text.Replace(".", ""),
                Segment = tr.FindElements(By.TagName("td"))[2].Text,
                Description = tr.FindElements(By.TagName("td"))[3].Text,
                NcmCode = ncmCode
            });

            await uow.NcmRepository.UpdateAsync(ncmUpdate, ct);
            foreach (var cest in cests)
                await uow.CestRepository.AddIfNotExistsAsync(c => c.Code == cest.Code, cest, ct);
            
            await uow.SaveChangesAsync(ct);
        }
        catch (Exception)
        {
            return;
        }
    }
}