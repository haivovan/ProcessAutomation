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
    public class MHSite : IAutomationPayIn
    {
        MailService mailService = new MailService();
        Helper helper = new Helper();
        private WebBrowser webLayout;
        private List<Message> data = new List<Message>();
        private const string web_name = "mh";
        private const string url = "http://12c.biz/";
        private const string index_URL = "http://12c.biz/ct/login.html";
        private const string user_URL = "http://12c.biz/ct/pages.html?r=";
        private const string agencies_URL = "http://12c.biz/ct/pages.html?r=0.8686662823899909#lich-su-nap/chuyen-tien?id=undefined";
        private bool isFinishProcess = true;
        Message currentMessage;
        Void v;
        TaskCompletionSource<Void> tcs = null;
        WebBrowserDocumentCompletedEventHandler documentComplete = null;
        MongoDatabase<AdminSetting> adminSetting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);

        public MHSite(List<Message> data, WebBrowser web)
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
                        webLayout.DocumentCompleted -= documentComplete;
                        tcs.SetResult(v);
                    }
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
                            if (webLayout.Url.ToString().Contains(user_URL))
                            {
                                process = "AccessToDaily";
                                if (data.Count() == 0 && data.Exists(x => x.IsKeepSession))
                                {
                                    process = "Finish";
                                }
                                break;
                            }

                            if (!webLayout.Url.ToString().Contains(index_URL))
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

                            if (webLayout.Url.ToString().Contains(index_URL))
                            {
                                if (!Globals.isSentNotification_MH)
                                {
                                    Globals.isSentNotification_MH = true;
                                    SendNotificationForError("Account Admin Đăng Nhập Lỗi",
                                        $"{web_name} : Account admin đăng nhập web bị lỗi");
                                }
                                process = "Finish";
                                break;
                            }
                            Globals.isSentNotification_MH = false;
                            process = "AccessToDaily";
                            if (data.Exists(x => x.IsKeepSession))
                            {
                                process = "Finish";
                            }
                            break;
                        case "AccessToDaily":
                            CreateSyncTask();
                            AccessToDaily();
                            await tcs.Task;
                            await Task.Delay(5000);

                            if (!webLayout.Url.ToString().Contains(agencies_URL))
                            {
                                SendNotificationForError(
                                    "Truy cập vào trang nạp tiền bị lỗi",
                                    $"{web_name} : Trang nạp tiền bị lỗi");
                                process = "Finish";
                                break;
                            }

                            process = "InputInformation";
                            break;
                        case "InputInformation":
                            currentMessage = data.FirstOrDefault();
                            InputInformation();
                            await Task.Delay(2000);

                            process = "PayIn";
                            break;
                        case "PayIn":
                            PayIn();
                            await Task.Delay(5000);

                            var payInSuccess = CheckSuccessPayIn();
                            await Task.Delay(5000);

                            if (!payInSuccess)
                            {
                                SaveRecord("Cộng tiền không thành công");
                                SendNotificationForError(
                                     "Cộng tiền không thành công",
                                     $"{Constant.MH.ToUpper()} : Lỗi + { currentMessage.Money } { currentMessage.Web }{ currentMessage.Account }");
                            }
                            else
                            {
                                SaveRecord();
                                SendNotificationForError(
                                     "Cộng tiền thành công",
                                     $"{Constant.MH.ToUpper()} : Đã + { currentMessage.Money } { currentMessage.Web }{ currentMessage.Account }");
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

                SendNotificationForError(
                    "Lỗi không xác định",
                    $"{web_name} : {ex.Message}");
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
            var inputUserName = htmlLogin.GetElementById("txtID");
            var inputPassword = htmlLogin.GetElementById("txtPW");

            if (inputUserName != null && inputPassword != null)
            {
                inputUserName.SetAttribute("value", adminAccount.AccountName);
                inputPassword.SetAttribute("value", adminAccount.Password);
            }

            var btnTag = htmlLogin.GetElementsByTagName("button");
            foreach (HtmlElement item in btnTag)
            {
                var attr = item.GetAttribute("onclick");
                if (attr != null)
                {
                    item.InvokeMember("onclick");
                    break;
                }
            }
        }

        private void AccessToDaily()
        {
            webLayout.Navigate("http://12c.biz/ct/pages.html?r=0.8686662823899909#lich-su-nap/chuyen-tien?id=undefined");
        }

        private void InputInformation()
        {
            var htmlLogin = webLayout.Document;
            var inputNguoiNhan = htmlLogin.GetElementById("txtIdNguoiNhan");
            inputNguoiNhan.SetAttribute("value", web_name + currentMessage.Account);

            var inputSoTien = htmlLogin.GetElementsByTagName("input");
            foreach (HtmlElement item in inputSoTien)
            {
                var attr = item.GetAttribute("fname");
                if (attr != null && attr == "sotien")
                {
                    var bonus = adminSetting.Query
                    .Where(x => x.Key == Constant.BONUS)
                    .Where(x => x.Name == Constant.MH).FirstOrDefault().Value;
                    var money = decimal.Parse(currentMessage.Money);
                    var total = money + Math.Round(money * decimal.Parse(bonus) / 100);
                    item.SetAttribute("value", total.ToString());
                    break;
                }
            }
            var buttonChuyenTien = htmlLogin.GetElementById("chuyentien_btnOk");
            buttonChuyenTien.InvokeMember("CLick");
        }

        private void PayIn()
        {
            var htmlLogin = webLayout.Document;
            var btns = htmlLogin.GetElementsByTagName("button");
            foreach (HtmlElement item in btns)
            {
                var btnConfirm = item.InnerHtml;
                if (!string.IsNullOrEmpty(btnConfirm) && btnConfirm == "OK")
                {
                    item.InvokeMember("Click");
                    break;
                }
            }
        }

        private bool CheckSuccessPayIn()
        {
            var isSuccess = false;
            var htmlLogin = webLayout.Document;
            var divs = htmlLogin.GetElementsByTagName("div");
            foreach (HtmlElement item in divs)
            {
                var message = item.InnerHtml;
                if (!string.IsNullOrEmpty(message) && message == "Chuyển tiền thành công")
                {
                    var btns = htmlLogin.GetElementsByTagName("button");
                    foreach (HtmlElement item1 in btns)
                    {
                        var btnConfirm = item1.InnerHtml;
                        if (!string.IsNullOrEmpty(btnConfirm) && btnConfirm == "OK")
                        {
                            item1.InvokeMember("Click");
                            isSuccess = true;
                            break;
                        }
                    }
                    break;
                }
            }
            return isSuccess;
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
            try
            {
                mailService.SendEmail(subject, message);
                helper.sendMessageZalo(message);
            }
            catch (Exception ex)
            {
                isFinishProcess = true;
            }
        }

        private void SaveRecord(string error = "")
        {
            MongoDatabase<Message> database = new MongoDatabase<Message>(typeof(Message).Name);
            var updateOption = Builders<Message>.Update
            .Set(p => p.IsProcessed, true)
            .Set(p => p.Error, error)
            .Set(p => p.DateExcute, DateTime.Now.Date);

            database.UpdateOne(x => x.Id == currentMessage.Id, updateOption);
        }
    }
}
