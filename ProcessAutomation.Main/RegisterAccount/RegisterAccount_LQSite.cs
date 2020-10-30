using MongoDB.Driver;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.Services;
using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessAutomation.Main.PayIn
{
    public class RegisterAccount_LQSite : IRegisterAccount
    {
        MailService mailService = new MailService();
        Helper helper = new Helper();
        private WebBrowser webLayout;
        private RegisterAccountModel data = new RegisterAccountModel();
        private RegisterAccount registerAccountForm;
        private const string web_name = "lanquephuong";
        private const string url = "https://lanquephuong.club/";
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

        public RegisterAccount_LQSite(RegisterAccountModel data)
        {
            this.data = data;
        }

        public void startRegister(WebBrowser web, RegisterAccount form)
        {
            this.webLayout = webLayout;
            this.registerAccountForm = form;
        }
    }
}
