using Invoices;
using Newtonsoft.Json;
using SummaProject1Vue.Controllers;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

public class InvoiceFetching
{
    /// <summary>
    /// Asynchronously sends PDF data to a specified email address.
    /// </summary>
    /// <param name="inputPdfPath">The file path of the input PDF.</param>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <returns>A JSON string representation of the invoice data if successful; otherwise, null.</returns>

    public static async Task<string> SendDataToMailAsync(string inputPdfPath,string toEmail)
    {
        try
        {
            string json = ConvertPdfToJson(inputPdfPath);
            string htmlContent = GenerateHtmlContent(json);
            await SendEmail.Execute(htmlContent, inputPdfPath, toEmail);
            return json;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Converts the contents of a PDF file to a JSON string.
    /// </summary>
    /// <param name="inputPdfPath">The file path of the input PDF.</param>
    /// <returns>A JSON string representation of the invoice data.</returns>
    private static string ConvertPdfToJson(string inputPdfPath)
    {
        try
        {
            using (PdfDocument pdfDocument = PdfDocument.Open(inputPdfPath))
            {
                StringBuilder textBuilder = new StringBuilder();

                foreach (Page page in pdfDocument.GetPages())
                {
                    string pageText = ConvertToRightToLeft(page.Text);
                    textBuilder.AppendLine(pageText);
                }

                string[] lines = Regex.Split(textBuilder.ToString(), @"[:,\t\r\n]| {3,}")
                                      .Where(line => !string.IsNullOrWhiteSpace(line))
                                      .Select(line => line.Replace("מ\"בע", "בע\"מ"))
                                      .ToArray();

                Invoice invoice = ExtractInvoiceData(lines);
                string json = JsonConvert.SerializeObject(invoice, Formatting.Indented);

                return json;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Generates HTML content from a JSON string.
    /// </summary>
    /// <param name="json">The JSON string representing the invoice data.</param>
    /// <returns>An HTML string representation of the JSON data.</returns>
    private static string GenerateHtmlContent(string json)
    {
        return $"<html><body><pre>{json}</pre></body></html>";
    }

    /// <summary>
    /// Converts the text to right-to-left format.
    /// </summary>
    /// <param name="text">The text to convert.</param>
    /// <returns>The right-to-left formatted text.</returns>
    private static string ConvertToRightToLeft(string text)
    {
        var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = ReverseHebrewOnly(lines[i]);
        }
        return string.Join(Environment.NewLine, lines);
    }

    /// <summary>
    /// Reverses only the Hebrew parts of the input string.
    /// </summary>
    /// <param name="s">The input string containing Hebrew text.</param>
    /// <returns>The string with Hebrew parts reversed.</returns>
    private static string ReverseHebrewOnly(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;

        StringBuilder result = new StringBuilder();
        foreach (Match match in Regex.Matches(s, @"([א-ת\""\',]+|[^א-ת]+)"))
        {
            string part = match.Value;
            if (Regex.IsMatch(part, @"[א-ת]"))
            {
                result.Append(ReverseHebrewPart(part));
            }
            else
            {
                result.Append(part);
            }
        }

        return result.ToString();
    }

    private static string ReverseHebrewPart(string part)
    {
        StringBuilder reversed = new StringBuilder();
        StringBuilder temp = new StringBuilder();
        for (int i = 0; i < part.Length; i++)
        {
            if (char.IsLetter(part[i]) && part[i] >= 'א' && part[i] <= 'ת')
            {
                temp.Append(part[i]);
            }
            else
            {
                if (temp.Length > 0)
                {
                    reversed.Append(ReverseString(temp.ToString()));
                    temp.Clear();
                }
                reversed.Append(part[i]);
            }
        }
        if (temp.Length > 0)
        {
            reversed.Append(ReverseString(temp.ToString()));
        }

        return reversed.ToString();
    }

    /// <summary>
    /// Reverses a string.
    /// </summary>
    /// <param name="s">The string to reverse.</param>
    /// <returns>The reversed string.</returns>
    private static string ReverseString(string s)
    {
        char[] arr = s.ToCharArray();
        Array.Reverse(arr);
        return new string(arr);
    }

    /// <summary>
    /// Extracts invoice data from an array of lines.
    /// </summary>
    /// <param name="lines">The array of lines extracted from the PDF.</param>
    /// <returns>An Invoice object containing the extracted data.</returns>
    private static Invoice ExtractInvoiceData(string[] lines)
    {
        Invoice invoice = new Invoice();

        string text = string.Join(Environment.NewLine, lines);

        invoice.SupplierName = ExtractSupplierName(text);
        invoice.CustomerName = ExtractCustomerName(lines);
        invoice.SupplierId = ExtractSupplierId(text);
        invoice.CustomerId = ExtractCustomerId(text);

        ExtractProductsAndCosts(text, invoice);

        invoice.TotalBeforeVAT = ExtractTotalBeforeVAT(text);
        invoice.TotalWithVAT = ExtractTotalWithVAT(text);
        invoice.Dates.Add(ExtractDate(text));

        return invoice;
    }

    /// <summary>
    /// Extracts the supplier name from the text.
    /// </summary>
    /// <param name="text">The text containing the supplier name.</param>
    /// <returns>The extracted supplier name.</returns>
    private static string ExtractSupplierName(string text)
    {
        var firstLine = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        return firstLine != null ? firstLine.Trim() : string.Empty;
    }

    /// <summary>
    /// Extracts the customer name from the lines.
    /// </summary>
    /// <param name="lines">The array of lines containing the customer name.</param>
    /// <returns>The extracted customer name.</returns>
    private static string ExtractCustomerName(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("לכבוד"))
            {
                if (i + 1 < lines.Length)
                {
                    return lines[i + 1].Trim();
                }
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Extracts the supplier ID from the text.
    /// </summary>
    /// <param name="text">The text containing the supplier ID.</param>
    /// <returns>The extracted supplier ID.</returns>
    private static string ExtractSupplierId(string text)
    {
        var match = Regex.Match(text, @"(?<=ח "" פ\s)[\d]{9}");
        return match.Success ? match.Value.Trim() : string.Empty;
    }

    /// <summary>
    /// Extracts the customer ID from the text.
    /// </summary>
    /// <param name="text">The text containing the customer ID.</param>
    /// <returns>The extracted customer ID.</returns>
    private static string ExtractCustomerId(string text)
    {
        var match = Regex.Matches(text, @"(?<=ח "" פ\s)[\d]{9}");
        return match.Count > 1 ? match[1].Value.Trim() : string.Empty;
    }

    /// <summary>
    /// Extracts products and their costs from the text and adds them to the invoice.
    /// </summary>
    /// <param name="text">The text containing product and cost information.</param>
    /// <param name="invoice">The Invoice object to populate with product data.</param>
    private static void ExtractProductsAndCosts(string text, Invoice invoice)
    {
        var productSection = Regex.Match(text, @"(?<=הכל סך)[\s\S]*(?=ביניים סכום)").Value;
        var productLines = productSection.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        List<Product> products = new List<Product>();

        foreach (var line in productLines)
        {
            var matches = Regex.Matches(line, @"(\d+)\s+([^0-9]+?)\s+(\d+)\s+(\d+)");
            foreach (Match match in matches)
            {
                if (match.Success && match.Groups.Count == 5)
                {
                    products.Add(new Product
                    {
                        Amount = match.Groups[1].Value,
                        Name = match.Groups[2].Value.Trim(),
                        Price = match.Groups[3].Value,
                        Total = match.Groups[4].Value
                    });
                }
            }
        }

        invoice.Products = products;
    }

    /// <summary>
    /// Extracts the total amount before VAT from the text.
    /// </summary>
    /// <param name="text">The text containing the total amount before VAT.</param>
    /// <returns>The extracted total amount before VAT.</returns>
    private static decimal ExtractTotalBeforeVAT(string text)
    {
        var match = Regex.Match(text, @"(?<=ביניים סכום\s)\d+(\.\d{1,2})?");
        if (match.Success && decimal.TryParse(match.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
        {
            return result;
        }
        return 0;
    }

    /// <summary>
    /// Extracts the total amount with VAT from the text.
    /// </summary>
    /// <param name="text">The text containing the total amount with VAT.</param>
    /// <returns>The extracted total amount with VAT.</returns>
    private static decimal ExtractTotalWithVAT(string text)
    {
        var match = Regex.Match(text, @"(?<=לתשלום כולל סכום\s)(\d+\s*\.\s*\d+)");
        if (match.Success)
        {
            // Remove any spaces within the number
            string cleanedValue = match.Value.Replace(" ", string.Empty);
            if (decimal.TryParse(cleanedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
        }
        return 0;
    }


    /// <summary>
    /// Extracts the date from the text.
    /// </summary>
    /// <param name="text">The text containing the date.</param>
    /// <returns>The extracted date.</returns>
    private static DateTime ExtractDate(string text)
    {
        var match = Regex.Match(text, @"\d{2}/\d{2}/\d{4}");
        if (match.Success && DateTime.TryParseExact(match.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            return date;
        }
        return DateTime.MinValue;
    }

}