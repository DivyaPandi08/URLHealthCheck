using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;

class Program
{
    static void Main()
    {
        List<UrlStatus> urlList = new List<UrlStatus>
        {
            new UrlStatus { ApplicationName = "App 1", Url = "https://example.com", IsHealthy = false },
            new UrlStatus { ApplicationName = "App 2", Url = "https://google.com", IsHealthy = true },
            new UrlStatus { ApplicationName = "App 3", Url = "https://example.org", IsHealthy = true }
            // Add more URLs as needed
        };

        SendEmailWithUrlStatus(urlList);
    }

    static void SendEmailWithUrlStatus(List<UrlStatus> urlStatusList)
    {
        string smtpServer = "smtp.gmail.com";
        int smtpPort = 587; // Adjust the port accordingly
        string smtpUsername = "divinandy08@gmail.com";
        string smtpPassword = "amgwtmhpxahkmpyf";
        string fromEmail = "divinandy08@gmail.com";
        string toEmail = "divyapandi0828@gmail.com"; // Replace with the recipient email address
        string ccEmail = "divyapandi98@gmail.com"; // Replace with the CC recipient email address

        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("Health Check", fromEmail));
        message.To.Add(new MailboxAddress("Health Check", toEmail));
        message.Cc.Add(new MailboxAddress("Health Check", ccEmail));
        message.Subject = $"Application Health Check status on {DateTime.Now:yyyy-MM-dd}";

        // Create the HTML body with the URL status grid
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = GenerateHtmlBody(urlStatusList);

        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect(smtpServer, smtpPort, false);
            client.Authenticate(smtpUsername, smtpPassword);
            client.Send(message);
            client.Disconnect(true);
        }
    }

    static string GenerateHtmlBody(List<UrlStatus> urlStatusList)
    {
        string tableRows = "";
        foreach (var urlStatus in urlStatusList)
        {
            string statusColor = urlStatus.IsHealthy ? "green" : "red";
            tableRows += $"<tr><td>{urlStatus.ApplicationName}</td><td style='color:{statusColor}'>{(urlStatus.IsHealthy ? "Healthy" : "Unhealthy")}</td></tr>";
        }

        return $@"
            <html>
            <body>
                <h4>Hi all,</h4>
                <h4>Please find the Application Status report for the below applications</h4>
                <table border='1'>
                    <tr>
                        <th>Application Name</th>
                        <th>Application Health Check at 7:00 AM AEST</th>
                    </tr>
                    {tableRows}
                </table>
            </body>
            </html>";
    }
}
