using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessAutomation.Main.Ultility
{
    public class Constant
    {
        #region Email Setting
        public const string SMTP_ADDRESS = "smtp.gmail.com";
        public const int PORT_NUMBER = 587;
        public const bool ENABLE_SSL = true;
        public const string EMAIL_TO = "autobot2099@gmail.com"; //"Phanminhchau2906@gmail.com";
        #endregion

        #region Extract Message
        public const string REG_EXTRACT_MESSAGE = "(\\+CMGL: \\d+)+(,\".*?\",)+(\".*?\",)+(,\".*?\")+(\n|\r\n)+(.*)";
        public const string REG_EXTRACT_MONEY_TEMPLATE1 = @"(tang)+(.*?VND)";
        public const string REG_EXTRACT_MONEY_TEMPLATE2 = @"(\+ )+(.*? )";
        public const string REG_EXTRACT_ACCOUNT = @"(cb|hl|bk|gd|nt| c b| h l| g d| b k| n t|cb |hl |gd |nt | c b | h l | g d | n t )+(\d\d\d\d)";
        public static List<string> WEBS_NAME = new List<string> { "cb", "hl", "bk" };
        #endregion

        #region Limitation
        public const decimal SATISFIED_PAYIN = 20000;
        public const decimal AMOUNT_ACCOUNT_CB = 1000000; //10000000
        public const decimal AMOUNT_ACCOUNT_BK = 1000000; //10000000
        public const decimal AMOUNT_ACCOUNT_HL = 1000000; //5000000
        public const decimal AMOUNT_ACCOUNT_GD = 0; //10000000
        public const decimal AMOUNT_ACCOUNT_NT = 0; //10000000
        public const decimal TEST_MONEY = 20000;
        public const string MINIMUM_MONEY_NAME = "MinimumMoney";
        #endregion

        #region WebName
        public const string BANHKEO = "bk";
        public const string CAYBANG = "cb";
        public const string HANHLANG = "hl";
        //public const string GIADINHVN = "gd";
        //public const string NT30s = "nt";
        #endregion
    }
}
