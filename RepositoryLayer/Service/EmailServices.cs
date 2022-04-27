using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RepositoryLayer.Service
{
    public class EmailServices
    {
        public static void SendMail(string email, string token)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;

                client.Credentials = new NetworkCredential("panulambat@gmail.com", "123@Pranali");
                MailMessage msgObj = new MailMessage();
                msgObj.To.Add(email);
                msgObj.From = new MailAddress("panulambat@gmail.com");
                msgObj.Subject = "Password Reset Link";
                msgObj.Body = $"Hi User,Find the below token \n \n www.FundooNotes.com/reset-password/{token}";
                client.Send(msgObj);
            }
        }
    }
}