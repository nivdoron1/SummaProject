using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Please enter your email address: ");
        string email = Console.ReadLine();

        string pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "pdf", "invoice.pdf");

        string combinedArgs = string.Join(",", args.Prepend(email));

        string json = await InvoiceFetching.SendDataToMailAsync(pdfPath,combinedArgs);

        Console.WriteLine(json);
    }
}