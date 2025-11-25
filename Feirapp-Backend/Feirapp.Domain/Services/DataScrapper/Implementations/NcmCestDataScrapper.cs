using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Entities.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Feirapp.Domain.Services.DataScrapper.Implementations;

public class NcmCestDataScrapper(IUnitOfWork uow, ILogger<NcmCestDataScrapper> logger) : INcmCestDataScrapper
{
    private const string BaseUrl = "https://codigocest.com.br/consulta-codigo-cest-pelo-ncm";
    
    public async Task UpdateNcmAndCestsDetailsAsync(CancellationToken ct)
    {
        var ncms = await uow.NcmRepository.GetNcmsWithoutDescriptionAsync(ct);
        var driver = new ChromeDriver();
        await driver.Navigate().GoToUrlAsync(BaseUrl);

        foreach (var ncm in ncms)
        {
            await GetNcmData(ncm.Code, driver, ct);
        }
        
        driver.Close();

        await uow.SaveChangesAsync(ct);
    }

    private async Task GetNcmData(string ncmCode, IWebDriver driver, CancellationToken ct)
    {
        var form = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/form"));

        var ncmInput = form.FindElement(By.XPath("//*[@id=\"focusedInput\"]"));
        ncmInput.Click();
        ncmInput.Clear();
        ncmInput.SendKeys(ncmCode);
        
        try
        {
            var searchButton = form.FindElement(By.XPath("/html/body/div[1]/div[2]/form/div/div[2]/button"));
            searchButton.Click();
        }
        catch (Exception e)
        {
            var closeAdd = form.FindElement(By.XPath("//*[@id=\"dismiss-button\"]"));
            closeAdd.Click();
            var searchButton = form.FindElement(By.XPath("/html/body/div[1]/div[2]/form/div/div[2]/button"));
            searchButton.Click();
        }

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        var ncmDivs = driver.FindElement(By.XPath("/html/body/div[1]/div[3]/div/div[2]/div[1]"));

        var ncmUpdate = new Ncm
        {
            Code = ncmCode,
            Description = ncmDivs.Text,
            LastUpdate = DateTime.Now
        };

        // Update NCM details even if CEST scraping fails later.
        await uow.NcmRepository.UpdateAsync(ncmUpdate, ct);

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

            foreach (var cest in cests)
            {
                await uow.CestRepository.AddIfNotExistsAsync(c => c.Code == cest.Code, cest, ct);
            }
        }
        catch (NoSuchElementException ex)
        {
            var screenshotPath = await CaptureDiagnosticsAsync(driver, ncmCode, "NoSuchElement", ct);
            logger.LogWarning(ex, "CEST elements not found for NCM {NcmCode}. Skipping CESTs. Screenshot: {ScreenshotPath};", ncmCode, screenshotPath);
        }
        catch (WebDriverException ex)
        {
            var screenshotPath = await CaptureDiagnosticsAsync(driver, ncmCode, "WebDriverException", ct);
            logger.LogError(ex, "WebDriver error while scraping NCM {NcmCode}. Screenshot: {ScreenshotPath};", ncmCode, screenshotPath);
        }
        catch (Exception ex)
        {
            var screenshotPath = await CaptureDiagnosticsAsync(driver, ncmCode, "Unexpected", ct);
            logger.LogError(ex, "Unexpected error while scraping NCM {NcmCode}. Screenshot: {ScreenshotPath};", ncmCode, screenshotPath);
        }
    }

    private async Task<string?> CaptureDiagnosticsAsync(IWebDriver driver, string ncmCode, string reason, CancellationToken ct)
    {
        try
        {
            var root = Path.Combine(AppContext.BaseDirectory, "diagnostics", "scraper");
            Directory.CreateDirectory(root);

            var safeNcm = SanitizeForFileName(ncmCode);
            var safeReason = SanitizeForFileName(reason);
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfff");
            var baseName = $"{safeNcm}_{safeReason}_{timestamp}";

            string? screenshotPath = null;

            // Screenshot capture (if supported by driver)
            try
            {
                if (driver is ITakesScreenshot shotTaker)
                {
                    var screenshot = shotTaker.GetScreenshot();
                    var path = Path.Combine(root, baseName + ".png");
                    screenshot.SaveAsFile(path);
                    screenshotPath = path;
                }
            }
            catch (Exception e)
            {
                logger.LogDebug(e, "Failed to capture screenshot for NCM {NcmCode}", ncmCode);
            }
            
            return screenshotPath;
        }
        catch (Exception e)
        {
            // Do not throw from diagnostics; just log and return nulls
            logger.LogDebug(e, "Failed to create diagnostics for NCM {NcmCode}", ncmCode);
            return null;
        }
    }

    private static string SanitizeForFileName(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "empty";
        var invalid = Path.GetInvalidFileNameChars();
        var sb = new StringBuilder(input.Length);
        foreach (var ch in input)
        {
            if (Array.IndexOf(invalid, ch) >= 0) continue;
            sb.Append(ch);
        }
        return sb.ToString();
    }
}