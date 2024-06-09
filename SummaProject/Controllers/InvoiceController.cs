using Microsoft.AspNetCore.Mvc;

namespace SummaProject1Vue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        [HttpGet("sendemail/{ToEmail}")]
        public async Task<IActionResult> ProcessInvoiceAsync(string ToEmail)
        {
            string pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "pdf", "invoice.pdf");

            if (System.IO.File.Exists(pdfPath))
            {
                string json = await InvoiceFetching.SendDataToMailAsync(pdfPath , ToEmail);
                return Ok(json);
            }
            else
            {
                return NotFound("File not found: " + pdfPath);
            }
        }
    }
}
