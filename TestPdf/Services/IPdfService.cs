using PuppeteerSharp;
using RazorLight;
using TestPdf.Enums;
using TestPdf.Extensions;

namespace TestPdf.Services;

public interface IPdfService
{
    Task<byte[]> GeneratePdfAsync<T>(string viewName, T model, string pageSize = "A4", PaperOrientation orientation = PaperOrientation.Portrait);
}
public class PdfService : IPdfService
{
    private readonly RazorLightEngine _engine;

    public PdfService()
    {
        var templatesPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(templatesPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<byte[]> GeneratePdfAsync<T>(string viewName, T model, string pageSize = "A4", PaperOrientation orientation = PaperOrientation.Portrait)
    {
        if (!viewName.EndsWith(".cshtml"))
            viewName += ".cshtml";
        string html = await _engine.CompileRenderAsync(viewName, model);
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        PdfOptions options = pageSize.ToPdfPaperFormat(orientation);
        return await page.PdfDataAsync(options);
    }
}
