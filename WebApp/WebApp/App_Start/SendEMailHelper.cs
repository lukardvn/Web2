using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebApp.App_Start
{
    public class SendEMailHelper
    {
        public static void Send(string to,string subject,string body)
        {
            MailMessage mailMessage = new MailMessage("webtest9696@gmail.com", to, subject, body);
            mailMessage.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Host ="smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("webtest9696@gmail.com", "web2web2");
            try
            {
                smtpClient.Send(mailMessage);
            }catch(Exception e)
            {
                string a = e.Message;
            }
        }

    }
}