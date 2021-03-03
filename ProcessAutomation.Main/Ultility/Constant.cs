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
        public const string REG_EXTRACT_MONEY_TEMPLATE3 = @"(\+)+(.*?VND)";
        public const string REG_EXTRACT_ACCOUNT1 = @"(hl|bk|dn|az|tl|3s| h l| b k| d n| a z| t l| 3 s|hl |dn |az |tl |3s | h l | d n | a z | t l | 3 s )+(\d\d\d\d)";
        public const string REG_EXTRACT_ACCOUNT2 = @"(hl|bk|dn|az|tl|3s)+(\d \d\d\d)";                                                                    
        public const string REG_EXTRACT_ACCOUNT3 = @"(hl|bk|dn|az|tl|3s| h l| b k| d n| a z| t l| 3 s|hl |dn |az |tl |3s | h l | d n | a z | t l | 3 s )+(\d\d \d\d)";
        public const string REG_EXTRACT_ACCOUNT4 = @"(hl|bk|dn|az|tl|3s)+( \d\d\d\d)";
        public const string REG_EXTRACT_SO_DU = @"^(.*?)VNĐ";
       
        public static List<string> WEBS_NAME = new List<string> { "hl", "bk", "dn", "az", "tl", "3s" };
        #endregion

        #region Limitation
        public const string MINIMUM_MONEY_NAME = "MinimumMoney";
        public const string MINIMUM_PAY_MONEY_NAME = "MinimumPayMoney";
        public const string BONUS = "Bonus";
        #endregion

        #region WebName
        public const string BANHKEO = "bk";
        public const string CAYBANG = "cb";
        public const string HANHLANG = "hl";
        public const string MH = "mh";
        public const string ALL = "Tất Cả";
        public const string LANQUEPHUONG = "lq";
        public const string DIENNUOC = "dn";
        public const string NAPAZ = "az";
        public const string TRUMLANG = "tl";
        public const string NAP3S = "3s";
        #endregion

        #region RegisterAccount
        public const string GET_NEW_ACCOUNT_URL = "https://checkcode.sinsudaidu.com/php/adddatactv.php";
        public const string UPDATE_ACCOUNT_URL = "https://checkcode.sinsudaidu.com/php/updatedatactv.php";
        public const string INTEREST = "1";
        #endregion

        #region CheckOTP
        public const string OTP = "otp";
        public const string GET_OTP_URL = "https://checkcode.sinsudaidu.com/php/getotp.php";
        #endregion

        #region Telegram
        public const string TELEGRAM_TOKEN = "1493452165:AAHf2jkrJWT5cE2ly4WJYIGK9DhLJSAc87k";
        public const string CHAT_ID_GROUP = "-1001414620121";
        public const string CHAT_ID_CHAU = "755964496";
        #endregion

    }
}
