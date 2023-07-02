using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;

namespace MarcketAppliction.Utils
{
    public class SendEmail
    {
        public static void Send(string to,string subject,string body)
        {
            
            MailMessage mail = new MailMessage();
          
            var SmtpServer = new SmtpClient("homayonmusic.ir");

            mail.From = new MailAddress("info@homayonmusic.ir", "همایون موزیک");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpServer.Port = 25;

            SmtpServer.Credentials = new System.Net.NetworkCredential("info@homayonmusic.ir", "WV%%5dayN6s2U2");
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            try
            {
                // ارسال ایمیل
                SmtpServer.Send(mail);
                Console.WriteLine("ایمیل با موفقیت ارسال شد.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("خطا در ارسال ایمیل: " + ex.Message);
            }
        


            
        

        }
    }
}