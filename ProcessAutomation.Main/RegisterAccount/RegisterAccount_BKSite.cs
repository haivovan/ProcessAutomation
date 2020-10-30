using MongoDB.Driver;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.Services;
using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProcessAutomation.Main.PayIn
{
    public class RegisterAccount_BKSite : IRegisterAccount
    {
        RegisterAccount registerAccountform;
        MailService mailService = new MailService();
        Helper helper = new Helper();
        private WebBrowser webLayout;
        private RegisterAccountModel data = new RegisterAccountModel();
        private const string web_name = "banhkeo";
        private const string url = "https://banhkeo.club/";
        private const string index_URL = url + "Login";
        private const string user_URL = url + "Users";
        private const string agencies_URL = url + "Users/Agencies";
        private const string addMoney_URL = url + "Users/AddMoneyToUser";
        private bool isFinishProcess = true;
        Message currentMessage;
        //Void v;
        //TaskCompletionSource<Void> tcs = null;
        WebBrowserDocumentCompletedEventHandler documentComplete = null;
        MongoDatabase<AdminSetting> adminSetting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);

        public RegisterAccount_BKSite(RegisterAccountModel data)
        {
            this.data = data;
        }

        public bool checkProcessDone()
        {
            return isFinishProcess;
        }

        public void startRegister(WebBrowser webLayout, RegisterAccount form)
        {
            this.webLayout = webLayout;
            this.registerAccountform = form;
        }
    }
}
