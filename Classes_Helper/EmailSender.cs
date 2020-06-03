using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace WhosIn
{
    public class EmailSender
    {
        private cMain m;
        public bool EmailSuccess { get; set; } = false;
        public string EmailErrorMessage { get; set; } = "";
        public string EmailHeaderInfo { get; set; } = "";
        public string EmailBodyInfo { get; set; } = "";

        public EmailSender(cMain mIn)
        {
            m = mIn;
        }

        public async Task<bool> SendEmailAsync(int EmpKey, string subject, string body)
        {
            string recipientEmail = "";

            recipientEmail = "aaaa@noplace.com";
            if (m.IsEmpty(recipientEmail))
            {
                return true;
            }
            return await SendEmailAsync(recipientEmail, string.Empty, subject, body);
        }

        public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
        {
            return await SendEmailAsync(recipientEmail, string.Empty, subject, body);
        }

        public async Task<bool> SendEmailAsync(string recipientEmail, string recipientName, string subject, string body)
        {
            SendGridClient client;
            Response response;
            HttpStatusCode statusCode;
            int statusCodeInt;

            if (m.x_DisableAllEmail)
            {
                return true;
            }

            if (!ValidEmailAddress(recipientEmail))
            {
                return false;
            }

            var message = new SendGridMessage();
            var from = new EmailAddress(m.x_FromEmailAddress, m.x_FromName);
            var to = new EmailAddress(recipientEmail, recipientName);

            message.SetFrom(from);
            message.AddTo(to);
            message.SetSubject(subject);
            message.AddContent(MimeType.Html, body);

            try
            {
                client = new SendGridClient(m.x_SendGridApiKey);
                response = await client.SendEmailAsync(message);
                statusCode = response.StatusCode;
                statusCodeInt = (int)response.StatusCode;
                EmailHeaderInfo = response.Headers.ToString();
                EmailBodyInfo = response.Body.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                EmailSuccess = false;
                EmailErrorMessage = e.Message;
                return EmailSuccess;
            }

            CheckForError(statusCodeInt);
            return EmailSuccess;
        }

        private void CheckForError(int statusCode)
        {
            string sMsg = "";

            switch (statusCode)
            {
                case 200:
                    sMsg = "Your message is valid, but it is not queued to be delivered. Sandbox mode"; 
                    break;

                case 400:
                    sMsg = "BAD REQUEST";
                    break;

                case 401:
                    sMsg = "You do not have authorization to make the request.";
                    break;

                case 403:
                    sMsg = "FORBIDDEN";
                    break;

                case 404:
                    sMsg = "The resource you tried to locate could not be found or does not exist.";
                    break;

                case 413:
                    sMsg = "The JSON payload you have included in your request is too large.";
                    break;

                case 429:
                    sMsg = "The number of requests you have made exceeds SendGrid’s rate limitations.";
                    break;

                case 500:
                    sMsg = "An error occurred on a SendGrid server.";
                    break;

                case 503:
                    sMsg = "The SendGrid v3 Web API is not available.";
                    break;

                case int n when (n >= 500 && n < 600):
                    sMsg = "An error occurred when SendGrid attempted to processes it.";
                    break;
            }

            if (m.IsEmpty(sMsg))
            {
                EmailErrorMessage = "";
                EmailSuccess = true;
            }
            else
            {
                EmailErrorMessage = sMsg;
                EmailSuccess = false;
            }

        }

        private bool ValidEmailAddress(string recipientEmail)
        {
            bool bOK;
            EmailValidator ev = new EmailValidator(m);

            bOK = ev.IsValidEmailAddress(recipientEmail);
            if (!bOK)
            {
                EmailErrorMessage = recipientEmail + " is not a valid email address.";
                EmailSuccess = false;
                return false;
            }

            return true;
        }
    }
}
