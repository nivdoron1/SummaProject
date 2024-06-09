using SendGrid.Helpers.Mail;
using SendGrid;
using System.IO;
using System.Threading.Tasks;
using System;

namespace SummaProject1Vue.Controllers
{
    public class SendEmail
    {
        /// <summary>
        /// Sends an email with HTML content and an optional attachment.
        /// </summary>
        /// <param name="htmlContent">The HTML content to include in the email body.</param>
        /// <param name="attachmentPath">The file path of the attachment to include in the email.</param>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task Execute(string htmlContent, string attachmentPath, string toEmail)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var fromEmail = Environment.GetEnvironmentVariable("SENDGRID_EMAIL");
            var username = Environment.GetEnvironmentVariable("SENDGRID_USERNAME");
            var from = new EmailAddress(fromEmail, username);
            var subject = "Send Email";
            var namePart = toEmail.Split('@')[0];
            var to = new EmailAddress(toEmail, namePart);
            var plainTextContent = "Json Data";

            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            if (File.Exists(attachmentPath))
            {
                byte[] fileBytes = File.ReadAllBytes(attachmentPath);
                string fileBase64 = Convert.ToBase64String(fileBytes);
                msg.AddAttachment(Path.GetFileName(attachmentPath), fileBase64);
            }

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.StatusCode);
        }
    }
}
