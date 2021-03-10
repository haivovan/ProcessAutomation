using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.Ultility;
using System.Net;
using System.Net.Mail;

namespace ProcessAutomation.Main.Services
{
    public class MailService
    {
        private AdminAccount account;
        public MailService()
        {
            var result = new MongoDatabase<AdminAccount>(typeof(AdminAccount).Name);
            account = result.Query.Where(x => x.Web == "email").FirstOrDefault();
        }

        //public void SendEmail(string subject, string body)
        //{
        //    using (MailMessage mail = new MailMessage())
        //    {
        //        mail.From = new MailAddress(account.AccountName);
        //        mail.To.Add(Constant.EMAIL_TO);
        //        mail.Subject = subject;
        //        mail.Body = body;

        //        using (SmtpClient smtp = new SmtpClient(Constant.SMTP_ADDRESS, Constant.PORT_NUMBER))
        //        {
        //            smtp.Credentials = new NetworkCredential(account.AccountName, account.Password);
        //            smtp.EnableSsl = Constant.ENABLE_SSL;
        //            smtp.Send(mail);
        //        }
        //    }
        //}
    }
}
