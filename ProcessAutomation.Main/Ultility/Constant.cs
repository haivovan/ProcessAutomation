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
        public const string REG_EXTRACT_ACCOUNT1 = @"(hl|bk|dn|az|tl|3s|sn|mm| h l| b k| d n| a z| t l| 3 s| s n| m m|hl |dn |az |tl |3s |sn |mm | h l | d n | a z | t l | 3 s | s n | m m )+(\d\d\d\d)";
        public const string REG_EXTRACT_ACCOUNT2 = @"(hl|bk|dn|az|tl|3s|sn|mm)+(\d \d\d\d)";                                                                    
        public const string REG_EXTRACT_ACCOUNT3 = @"(hl|bk|dn|az|tl|3s|sn|mm| h l| b k| d n| a z| t l| 3 s| s n| m m|hl |dn |az |tl |3s |sn |mm | h l | d n | a z | t l | 3 s | s n | m m )+(\d\d \d\d)";
        public const string REG_EXTRACT_ACCOUNT4 = @"(hl|bk|dn|az|tl|3s|sn|mm)+( \d\d\d\d)";
        public const string REG_EXTRACT_SO_DU = @"^(.*?)Đ";
        public const string REG_EXTRACT_GHI_CHU = @"(.*?)(SD:|. So du)";

        public static List<string> WEBS_NAME = new List<string> { "hl", "bk", "dn", "az", "tl", "3s", "sn", "mm" };
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
        public const string SIEUNHANH= "sn";
        public const string MEOMUOP = "mm";
        public const string HOASUA = "hs";
        #endregion

        #region RegisterAccount
        public const string GET_NEW_ACCOUNT_URL = "https://api.hcm2099.xyz/toolnaptien/adddatactv.php";
        public const string UPDATE_ACCOUNT_URL = "https://api.hcm2099.xyz/toolnaptien/updatedatactv.php";
        public const string INTEREST = "1";
        #endregion

        #region CheckOTP
        public const string OTP = "otp";
        public const string GET_OTP_URL = "https://api.hcm2099.xyz/toolnaptien/getotp.php";
        #endregion

        #region Telegram
        public const string TELEGRAM_TOKEN = "2116574718:AAHrN6eebagXtUnJ1M2w_2yUqFaFN3Ie8IA";
        public const string CHAT_ID_GROUP = "-1001715788221";//-655898220
        public const string CHAT_ID_CHAU = "-655898220";
        #endregion

    }
}
