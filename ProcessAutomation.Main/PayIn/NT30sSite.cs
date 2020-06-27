/*using MongoDB.Bson;
using MongoDB.Driver;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.Services;
using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessAutomation.Main.PayIn
{
    public class NT30sSite : IAutomationPayIn
    {
        MailService mailService = new MailService();
        Helper helper = new Helper();
        private WebBrowser webLayout;
        private List<Message> data = new List<Message>();
        private const string web_name = "naptien30s";
        private const string url = "https://naptien30s.vn/";
        private const string index_URL = url + "HIMONEY/HiMM/";
        private const string user_URL = url + "HIMONEY/HiMM/helloVMV.php";
        private const string agencies_URL = url + "HIMONEY/HiMM/chuyenkhoan.php";
        private bool isFinishProcess = true;
        Message currentMessage;
        Void v;
        TaskCompletionSource<Void> tcs = null;
        WebBrowserDocumentCompletedEventHandler documentComplete = null;
        WebBrowserNavigatedEventHandler documentNavigatedComplete = null;

        public NT30sSite(List<Message> data, WebBrowser web)
        {
            this.data = data;
            this.webLayout = web;
        }

        public bool checkProcessDone()
        {
            return isFinishProcess;
        }

        public void startPayIN()
        {
            StartProcess();
        }

        struct Void { };

        async Task StartProcess()
        {
            try
            {
                documentComplete = new WebBrowserDocumentCompletedEventHandler((s, e) =>
                {
                    if (webLayout.DocumentText.Contains("res://ieframe.dll"))
                    {
                        tcs.SetException(new Exception("Lỗi không có kết nối internet"));
                        return;
                    }
                    if (!tcs.Task.IsCompleted)
                    {
                        HtmlDocument doc = webLayout.Document;
                        HtmlElement head = doc.GetElementsByTagName("head")[0];
                        HtmlElement script = doc.CreateElement("script");
                        script.SetAttribute("text",
                            "window.alert = function(e){" +
                            "if(e.indexOf('https://stackoverflow.com/') != -1)" +
                            "{ window.location.replace(" + url + ");}" +
                            "else {}}");
                        head.AppendChild(script);
                        webLayout.DocumentCompleted -= documentComplete;
                        tcs.SetResult(v);
                    }
                });

                documentNavigatedComplete = new WebBrowserNavigatedEventHandler((s, e) =>
                {
                    HtmlDocument doc = webLayout.Document;
                    HtmlElement head = doc.GetElementsByTagName("head")[0];
                    HtmlElement script = doc.CreateElement("script");
                    script.SetAttribute("text",
                        "window.alert = function(e){" +
                        "if(e.indexOf('Chuyển Khoản Thành Công') != -1) {}" +
                        "else {window.location.replace('https://www.google.com.vn/');}}");
                    head.AppendChild(script);
                    webLayout.Navigated -= documentNavigatedComplete;
                });

                isFinishProcess = false;
                AccountData userAccount = new AccountData();
                var adminAccount = new AdminAccount();

                var process = checkAccountAdmin(ref adminAccount);
                do
                {
                    switch (process)
                    {
                        case "OpenWeb":
                            CreateSyncTask();
                            webLayout.Navigate(url);
                            await tcs.Task;
                            await Task.Delay(5000);

                            process = "Login";
                            if (webLayout.Url.ToString() == user_URL)
                            {
                                process = "AccessToDaily";
                                break;
                            }

                            if (webLayout.Url.ToString() != index_URL)
                            {
                                SendNotificationForError(
                                       "Trang Web Không Truy Cập Được",
                                       $"{web_name} không thể truy cập");

                                process = "Finish";
                                break;
                            }
                            break;
                        case "Login":
                            CreateSyncTask();
                            Login(adminAccount);
                            await tcs.Task;
                            await Task.Delay(5000);

                            if (webLayout.Url.ToString() == index_URL)
                            {
                                if (!Globals.isSentNotification_NT)
                                {
                                    Globals.isSentNotification_NT = true;
                                    SendNotificationForError("Account Admin Đăng Nhập Lỗi",
                                        $"{web_name} : Account admin đăng nhập web bị lỗi");
                                }
                                process = "Finish";
                                break;
                            }
                            Globals.isSentNotification_NT = false;
                            process = "AccessToDaily";
                            break;
                        case "AccessToDaily":
                            CreateSyncTask();
                            AccessToDaily();
                            await tcs.Task;
                            await Task.Delay(5000);

                            if (webLayout.Url.ToString() != agencies_URL)
                            {
                                SendNotificationForError(
                                    "Truy cập vào đại lý bị lỗi",
                                    $"{web_name} : Trang đại lý bị lỗi");
                                process = "Finish";
                                break;
                            }

                            process = "SearchUser";
                            break;
                        case "SearchUser":
                            currentMessage = data.FirstOrDefault();
                            userAccount = SearchUser();
                            if (userAccount == null)
                            {
                                // save record
                                SaveRecord($"Không tìm thấy user {web_name} : user id {currentMessage.Account}");

                                data.Remove(currentMessage);
                                if (data.Count == 0)
                                {
                                    process = "Finish";
                                    break;
                                }
                                process = "OpenWeb";
                                break;
                            }
                            process = "CheckAmountAccount";
                            break;
                        case "CheckAmountAccount":
                            var isAmountEnough = CheckAmountAccount();
                            await Task.Delay(3000);

                            if (!isAmountEnough)
                            {
                                if (!Globals.isSentNotification_NT)
                                {
                                    Globals.isSentNotification_NT = true;
                                    SendNotificationForError("Account không đủ số tiền tối thiểu",
                                        $"{web_name} : Account admin không đủ số tiền tối thiểu");
                                }
                                process = "Finish";
                                break;
                            }
                            Globals.isSentNotification_NT = false;
                            process = "PayIn";
                            break;
                        case "PayIn":
                            PayIn(userAccount);
                            await Task.Delay(2000);

                            CreateSyncTask();
                            PayInSubmit();
                            webLayout.Navigated += documentNavigatedComplete;
                            await tcs.Task;
                            await Task.Delay(5000);

                            if (!webLayout.Url.ToString().Contains(agencies_URL))
                            {
                                var errorMessage = $"Cộng tiền account { currentMessage.Account } bị lỗi";
                                SendNotificationForError(
                                    "Cộng tiền không thành công",
                                    $"{web_name} : Cộng tiền account { currentMessage.Account } bị lỗi");

                                SaveRecord(errorMessage);
                            }
                            else
                            {
                                SendNotificationForError(
                                    "Cộng tiền thành công",
                                    $"{web_name} : Cộng tiền thành công account { currentMessage.Account }, " +
                                    $"số tiền { currentMessage.Money }");

                                SaveRecord();
                            }

                            data.Remove(currentMessage);
                            if (data.Count == 0)
                            {
                                process = "Finish";
                                break;
                            }
                            process = "OpenWeb";
                            break;
                        case "Finish":
                            isFinishProcess = true;
                            break;
                    }
                } while (!isFinishProcess);

            }
            catch (Exception ex)
            {
                isFinishProcess = true;
                if (ex.Message.Contains("Lỗi không có kết nối internet"))
                {
                    DialogResult dialog = MessageBox.Show("Hãy kiểm tra internet và thử lại."
                     , "Mất kết nối internet", MessageBoxButtons.OK);
                    if (dialog == DialogResult.OK)
                    {
                        Application.ExitThread();
                    }
                }

                if (webLayout.Url.ToString().Contains("stackoverflow"))
                {
                    SendNotificationForError(
                    "Lỗi Có Người Đăng Nhập",
                    $"{web_name} : Có người đăng nhập ở máy khác");
                }
                else
                {
                    SendNotificationForError(
                    "Lỗi không xác định",
                    $"{web_name} : {ex.Message}");
                }
            }

            return;
        }

        private void CreateSyncTask()
        {
            tcs = new TaskCompletionSource<Void>();
            webLayout.ScriptErrorsSuppressed = true;
            webLayout.DocumentCompleted += documentComplete;
        }

        private void Login(AdminAccount adminAccount)
        {
            var htmlLogin = webLayout.Document;
            var inputTag = htmlLogin.GetElementsByTagName("input");
            foreach (HtmlElement item in inputTag)
            {
                var name = item.GetAttribute("name");
                if (name != null && name == "txtemail")
                {
                    item.SetAttribute("value", adminAccount.AccountName);
                }
                else if (name != null && name == "ntxtupass")
                {
                    item.SetAttribute("value", adminAccount.Password);
                }
            }

            var btnLogin = htmlLogin.GetElementById("btn-login");
            btnLogin.InvokeMember("Click");
        }

        private void AccessToDaily()
        {
            var htmlIndex = webLayout.Document;
            var aTag = htmlIndex.GetElementsByTagName("a");
            foreach (HtmlElement item in aTag)
            {
                var href = item.GetAttribute("href");
                var innerHTML = item.InnerHtml;
                if (href != null && href == agencies_URL && innerHTML.Contains("Chuyển Khoản Cấp Dưới"))
                {
                    item.InvokeMember("Click");
                    break;
                }
            }
        }

        private AccountData SearchUser()
        {
            MongoDatabase<AccountData> accountData = new MongoDatabase<AccountData>(typeof(AccountData).Name);
            var userAccount = accountData.
                Query.Where(x => x.IDAccount == currentMessage.Account.Trim()).FirstOrDefault();

            if (userAccount == null || string.IsNullOrEmpty(userAccount.NT))
                return null;
            return userAccount;
        }

        private bool CheckAmountAccount()
        {
            try
            {
                var html = webLayout.Document;
                var totalMoney = html.GetElementById("totalMoney");
                if (totalMoney != null)
                {
                    var setting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);
                    var minimumMoney = setting.Query.Where(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                                            && x.Key == Constant.NT30s).FirstOrDefault();
                    decimal outMoney = 0;
                    return (decimal.TryParse(totalMoney.InnerHtml.Replace("VNĐ", "").Trim(), out outMoney)
                        && outMoney >= decimal.Parse(minimumMoney.Value));
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void PayIn(AccountData accountData)
        {
            var html = webLayout.Document;
            var amount = html.GetElementsByTagName("input");
            foreach (HtmlElement item in amount)
            {
                var value = item.GetAttribute("name");
                if (value == "txt_email")
                {
                    item.SetAttribute("value", accountData.NT);
                }
                else if (value == "txt_gia")
                {
                    item.SetAttribute("value", currentMessage.Money);
                }
            }

        }

        private void PayInSubmit()
        {
            var html = webLayout.Document;
            var button = html.GetElementsByTagName("button");
            foreach (HtmlElement item in button)
            {
                var btnSubmit = item.GetAttribute("name");
                //"kiemtraemailnguoinhan"
                //"chuyenkhoantronghethong"
                if (btnSubmit == "chuyenkhoantronghethong")
                {
                    item.InvokeMember("Click");
                    break;
                }
            }
        }

        private string checkAccountAdmin(ref AdminAccount account)
        {
            var dataAccount = new MongoDatabase<AdminAccount>(typeof(AdminAccount).Name);
            var accountToPay = dataAccount.Query.Where(x => x.Web == web_name).FirstOrDefault();
            if (accountToPay != null)
            {
                account = accountToPay;
                return "OpenWeb";
            }

            SendNotificationForError(
                "Lỗi Account Admin",
                $"Không lấy được hoặc không tồn tại account admin trang web {web_name}");

            return "Finish";
        }


        private void SendNotificationForError(string subject, string message)
        {
            mailService.SendEmail(subject, message);
            helper.sendMessageZalo(message);
        }

        private void SaveRecord(string error = "")
        {
            MongoDatabase<Message> database = new MongoDatabase<Message>(typeof(Message).Name);
            var updateOption = Builders<Message>.Update
            .Set(p => p.IsProcessed, true)
            .Set(p => p.Error, error)
            .Set(p => p.DateExcute, DateTime.Now);

            database.UpdateOne(x => x.Id == currentMessage.Id, updateOption);
        }
    }
}
*/