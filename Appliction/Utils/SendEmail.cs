using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MarcketAppliction.Utils
{
    public class SendEmail
    {
        public static void Send(string to,string subject,string body)
        {
            //
            MailMessage mail = new MailMessage();
            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            var SmtpServer = new SmtpClient("mail.homayonmusic.ir");

            mail.From = new MailAddress("info@homayonmusic.ir", "همایون موزیک");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.

            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);

            //SmtpServer.Port = 587;
            SmtpServer.Port = 465;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("mohammadmy619@gmail.com", "jzvpcfzwskqwmlqh");
            SmtpServer.Credentials = new System.Net.NetworkCredential("info@homayonmusic.ir", "@t3nSo56");
            SmtpServer.EnableSsl = true;
       
             SmtpServer.Send(mail);

            

        }
    }
}
////mohammadmy619@gmail.com

//netcoremy619@gmail.com
//netcoremy619@gmail.com
//09214399123Mm#$