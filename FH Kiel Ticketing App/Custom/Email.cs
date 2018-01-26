using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace FH_Kiel_Ticketing_App.Custom
{
    public class Email
    {
        public static void SendVerificationEmail(string email, string activationCode, string finalLink)
        {
            var fromEmail = new MailAddress("noreply.fhticketingapp@gmail.com", "The Ticketing Robot");
            var toEmail = new MailAddress(email);
            var fromPassword = "thisisalongpassword";
            string subject = "Your account is successfully created";

            string body = "<br/><br/>So you want to join the ticketing world? One more step and you're done." +
                " Please Click on the link below to verify your account." +
                " <br/><br/><a href='" + finalLink + "'>" + finalLink + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

                smtp.Send(message);
        }
    }
}