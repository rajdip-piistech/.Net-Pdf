using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestPdf.Enums;
using TestPdf.Model;
using TestPdf.Services;

namespace TestPdf.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PdfController : ControllerBase
{
    private readonly IPdfService _pdfService;

    public PdfController(IPdfService pdfService)
    {
        _pdfService = pdfService;
    }

    [HttpGet("/invoice-pdf")]
    public async Task<IActionResult> Invoice()
    {
        var model = new InvoiceModel
        {
            InvoiceNumber = "INV-002",
            Date = DateTime.Today,
            Customer = "Parves Kawser",
            Items = new List<InvoiceItem>
            {
                new() { Name = "Laptop", Quantity = 1, Price = 85000 },
                new() { Name = "Mouse", Quantity = 2, Price = 800 }
            }
        };
        byte[] pdf = await _pdfService.GeneratePdfAsync("InvoiceTemplate", model, "A5", PaperOrientation.Landscape);
        return File(pdf, "application/pdf", "invoice.pdf");
    }
}
