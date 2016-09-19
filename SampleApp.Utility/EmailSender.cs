using Newtonsoft.Json;
using SampleApp.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace SampleApp.Utility
{
    public static class EmailLogic
    {
        public static bool SendEmail(string to, string body, string subject)
        {
            bool result = false;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("ctsazaad@gmail.com", "teamcarina2014")
            };
            using (var message = new MailMessage("ctsazaad@gmail.com", to)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);



                result = true;
            }
            return result;
        }
    }
}
