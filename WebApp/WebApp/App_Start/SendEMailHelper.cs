using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebApp.App_Start
{
    public class SendEMailHelper
    {
        public static void Send(string content,string to,string subject,string body)
        {
            MailMessage mailMessage = new MailMessage("admin@yahoo.com", to, subject, body);

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
        }

    }
}