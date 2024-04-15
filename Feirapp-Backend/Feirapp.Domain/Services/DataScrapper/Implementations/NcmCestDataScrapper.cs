using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Entities.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Feirapp.Domain.Services.DataScrapper.Implementations;

public class NcmCestDataScrapper : INcmCestDataScrapper
{
    private const string BaseUrl = "https://codigocest.com.br/consulta-codigo-cest-pelo-ncm";
    private readonly IBaseRepository<Ncm> _ncmRepository;
    private readonly IBaseRepository<Cest> _cestRepository;

    public NcmCestDataScrapper(IBaseRepository<Ncm> ncmRepository, IBaseRepository<Cest> cestRepository)
    {
        _ncmRepository = ncmRepository;
        _cestRepository = cestRepository;
    }

    public async Task UpdateNcmAndCestsDetails(CancellationToken ct)
    {
        var ncms = _ncmRepository.GetByQuery(c => string.IsNullOrEmpty(c.Description), ct);
        var driver = new ChromeDriver();
        driver.Navigate().GoToUrl(BaseUrl);
        foreach (var ncm in ncms) await GetNcmData(ncm.Code, driver, ct);
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
            
            await _ncmRepository.UpdateAsync(ncmUpdate, ct);
            foreach (var cest in cests)
                await _cestRepository.AddIfNotExistsAsync(cest, c => c.Code == cest.Code, ct);
        }
        catch (Exception)
        {
            return;
        }
    }
}