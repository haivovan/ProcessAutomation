using Gecko;
using Gecko.DOM;
using MongoDB.Driver;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.Services;
using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProcessAutomation.Main.PayIn
{
    public class CBSite : IAutomationPayIn
    {
        MailService mailService = new MailService();
        Helper helper = new Helper();
        private GeckoWebBrowser webLayout;
        private List<Message> data = new List<Message>();
        private const string web_name = "caybang";
        private const string url = "https://caybang.club/";
        private const string index_URL = url + "Login";
        private const string user_URL = url + "Users";
        private const string agencies_URL = url + "Users/Agencies";
        private const string addMoney_URL = url + "Users/AddMoneyToUser";
        private bool isFinishProcess = true;
        Message currentMessage;
        Void v;
        TaskCompletionSource<Void> tcs = null;
        EventHandler<Gecko.Events.GeckoDocumentCompletedEventArgs> documentComplete;
        MongoDatabase<AdminSetting> adminSetting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);

        public CBSite(List<Message> data, GeckoWebBrowser web)
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
                documentComplete = new EventHandler<Gecko.Events.GeckoDocumentCompletedEventArgs>((s, e) =>
                {
                    //if (webLayout.DocumentText.Contains("res://ieframe.dll"))
                    //{
                    //    tcs.SetException(new Exception("Lỗi không có kết nối internet"));
                    //    return;
                    //}
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
                            if (webLayout.Url.ToString() == user_URL)
                            {
                                process = "CheckAmountAccount";
                                if (data.Count() == 0 && data.Exists(x => x.IsKeepSession))
                                {
                                    process = "Finish";
                                }
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
                                if (!Globals.isSentNotification_CB)
                                {
                                    Globals.isSentNotification_CB = true;
                                    SendNotificationForError("Account Admin Đăng Nhập Lỗi",
                                        $"{web_name} : Account admin đăng nhập web bị lỗi");
                                }
                                process = "Finish";
                                break;
                            }
                            Globals.isSentNotification_CB = false;
                            process = "CheckAmountAccount";
                            if (data.Exists(x => x.IsKeepSession))
                            {
                                process = "Finish";
                            }
                            break;
                        case "CheckAmountAccount":
                            var isAmountEnough = CheckAmountAccount();
                            await Task.Delay(5000);

                            if (!isAmountEnough)
                            {
                                if (!Globals.isSentNotification_CB)
                                {
                                    Globals.isSentNotification_CB = true;
                                    SendNotificationForError("Account không đủ số tiền tối thiểu",
                                        $"{web_name} : Account admin không đủ số tiền tối thiểu");
                                }
                            }
                            else
                            {
                                Globals.isSentNotification_CB = false;
                            }
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

                            CreateSyncTask();
                            SearchUserClick(userAccount);
                            await tcs.Task;
                            await Task.Delay(3000);

                            process = "AccessToPayIn";
                            break;
                        case "AccessToPayIn":
                            var userRow = FindAccountOnResult(userAccount);
                            if (userRow == null)
                            {
                                var errorMessage = $"" +
                                    $"Truy cập trang cộng tiền web {web_name} bị lỗi hoặc" +
                                    $" {web_name} : không tìm thấy account của User id {userAccount.IDAccount}";

                                SaveRecord(errorMessage);
                                SendNotificationForError(
                                    "Truy cập vào trang cộng tiền bị lỗi", errorMessage);

                                data.Remove(currentMessage);
                                if (data.Count == 0)
                                {
                                    process = "Finish";
                                    break;
                                }
                                process = "OpenWeb";
                                break;
                            }

                            CreateSyncTask();
                            AccessToPayIn(userRow);
                            await tcs.Task;
                            await Task.Delay(5000);

                            process = "PayIn";
                            break;
                        case "PayIn":
                            PayIn();
                            await Task.Delay(3000);

                            CreateSyncTask();
                            PayInSubmit();
                            await tcs.Task;
                            await Task.Delay(10000);

                            if (!webLayout.Url.ToString().Contains(agencies_URL))
                            {
                                var errorMessage = $"Cộng tiền account { currentMessage.Account } bị lỗi";
                                SaveRecord(errorMessage);

                                SendNotificationForError(
                                    "Cộng tiền không thành công",
                                    $"{Constant.CAYBANG.ToUpper()} : Lỗi + { currentMessage.Money } { currentMessage.Web }{ currentMessage.Account }");
                            }
                            else
                            {
                                SaveRecord();
                                SendNotificationForError(
                                     "Cộng tiền thành công",
                                     $"{Constant.CAYBANG.ToUpper()} : Đã + { currentMessage.Money } { currentMessage.Web }{ currentMessage.Account }");
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
            webLayout.DocumentCompleted += documentComplete;
        }

        private void Login(AdminAccount adminAccount)
        {
            var htmlLogin = webLayout.Document;
            var inputUserName = htmlLogin.GetElementById("Username");
            var inputPassword = htmlLogin.GetElementById("Password");
            var inputOTP = htmlLogin.GetElementById("OTP");
            //var btnLogin = htmlLogin.GetElementById("login");
            var otpSetting = adminSetting.Query.Where(x => x.Name == "OTP" && x.Key.ToLower() == Constant.CAYBANG).FirstOrDefault();
            var otpValue = otpSetting.Value ?? string.Empty;
            GeckoLinkElement btnLogin = (GeckoLinkElement)htmlLogin.GetElementsByName("login")[0];

            if (inputUserName != null && inputPassword != null)
            {
                inputUserName.SetAttribute("value", adminAccount.AccountName);
                inputPassword.SetAttribute("value", adminAccount.Password);
                inputOTP.SetAttribute("value", otpValue);
                btnLogin.Click();
            }
        }

        private void AccessToDaily()
        {
            var htmlIndex = webLayout.Document;
            var aTag = htmlIndex.GetElementsByTagName("a");
            foreach (GeckoHtmlElement item in aTag)
            {
                var href = item.GetAttribute("href");
                if (href != null && href == agencies_URL)
                {
                    item.Click();
                    break;
                }
            }
        }

        private AccountData SearchUser()
        {
            MongoDatabase<AccountData> accountData = new MongoDatabase<AccountData>(typeof(AccountData).Name);
            var userAccount = accountData.
                Query.Where(x => x.IDAccount == currentMessage.Account.Trim()).FirstOrDefault();

            if (userAccount == null || string.IsNullOrEmpty(userAccount.CB))
                return null;
            return userAccount;
        }

        private void SearchUserClick(AccountData userAccount)
        {
            var html = webLayout.Document;
            var userFilter = html.GetElementById("phone");
            userFilter.SetAttribute("value", userAccount.CB);
            var aTag = html.GetElementsByTagName("a");
            foreach (GeckoHtmlElement item in aTag)
            {
                var btnTimKiem = item.InnerHtml;
                if (btnTimKiem == "TÌM KIẾM")
                {
                    item.Click();
                    break;
                }
            }
        }

        private bool CheckAmountAccount()
        {
            try
            {
                GeckoElement tdResult = null;
                var html = webLayout.Document;
                var table = html.GetElementsByTagName("table")[0];
                var trs = table.GetElementsByTagName("tr");
                foreach (GeckoHtmlElement tr in trs)
                {
                    var tds = tr.GetElementsByTagName("td");
                    foreach (GeckoHtmlElement td in tds)
                    {
                        try
                        {
                            string value = td.InnerHtml.ToString();
                            if (value != null && value.Contains("SỐ DƯ TÀI KHOẢN"))
                            {
                                tdResult = tds[1]; //[1] is amount of money
                                break;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                if (tdResult != null)
                {
                    var minimumMoney = adminSetting.Query.Where(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                                            && x.Key == Constant.CAYBANG).FirstOrDefault();

                    decimal outMoney = 0;
                    return (decimal.TryParse(tdResult.TextContent.Replace("VNĐ", "").Trim(), out outMoney)
                        && outMoney >= decimal.Parse(minimumMoney.Value));
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private GeckoElement FindAccountOnResult(AccountData accountData)
        {
            GeckoElement trFound = null;
            var html = webLayout.Document;
            var table = html.GetElementsByTagName("table")[0];
            var trs = table.GetElementsByTagName("tr");
            foreach (GeckoHtmlElement tr in trs)
            {
                var tds = tr.GetElementsByTagName("td");
                foreach (GeckoHtmlElement td in tds)
                {
                    try
                    {
                        string value = td.TextContent.Trim();
                        if (value != null && value.Trim() == accountData.HLC.Trim())
                        {
                            trFound = tr;
                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return trFound;
        }

        private void AccessToPayIn(GeckoElement userRow)
        {
            if (userRow != null)
            {
                var aTag = userRow.GetElementsByTagName("a");
                foreach (GeckoElement item in aTag)
                {
                    var btnTimKiem = item.TextContent.Trim();
                    if (btnTimKiem == "CỘNG TIỀN")
                    {
                        GeckoLinkElement btnPay = (GeckoLinkElement)item;
                        btnPay.Click();
                        break;
                    }
                }
            }
        }

        private void PayIn()
        {
            var html = webLayout.Document;
            var amount = html.GetElementById("Amount");
            var bonus = adminSetting.Query
                .Where(x => x.Name == Constant.BONUS)
                .Where(x => x.Key == Constant.CAYBANG).FirstOrDefault().Value;
            var money = decimal.Parse(currentMessage.Money);
            var total = money + Math.Round(money * decimal.Parse(bonus) / 100);

            amount.SetAttribute("value", total.ToString());

            var message = html.GetElementById("Message");
            var messageContent = currentMessage.MessageContent;
            var matches = new Regex(Constant.REG_EXTRACT_GHI_CHU, RegexOptions.IgnoreCase).Match(messageContent).Groups;
            if (matches.Count >= 2)
            {
                messageContent = matches[1].ToString();
            }

            message.TextContent=messageContent;
        }

        private void PayInSubmit()
        {
            var html = webLayout.Document;
            GeckoLinkElement btnPay = (GeckoLinkElement)html.GetElementById("add_money_button");
            btnPay.Click();
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
                helper.SendMessageTelegram(message);
                isFinishProcess = true;
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
