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
    public class SNSite : IAutomationPayIn
    {
        MailService mailService = new MailService();
        Helper helper = new Helper();
        private GeckoWebBrowser webLayout;
        private List<Message> data = new List<Message>();
        private const string web_name = "sn";
        private const string url = "https://sieunhanh.vip/";
        private const string index_URL = url + "Login";
        private const string user_URL = url + "Users";
        private const string agencies_URL = url + "Users/Agencies";
        private bool isFinishProcess = true;
        Message currentMessage;
        Void v;
        TaskCompletionSource<Void> tcs = null;
        EventHandler<Gecko.Events.GeckoDocumentCompletedEventArgs> documentComplete;
        MongoDatabase<AdminSetting> adminSetting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);

        public SNSite(List<Message> data, GeckoWebBrowser web)
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
                decimal moneyLeft = 0;

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
                                if (!Globals.isSentNotification_TC)
                                {
                                    Globals.isSentNotification_TC = true;
                                    SendNotificationForError("Account Admin Đăng Nhập Lỗi",
                                        $"{web_name} : Account admin đăng nhập web bị lỗi");
                                }
                                process = "Finish";
                                break;
                            }
                            Globals.isSentNotification_TC = false;
                            process = "CheckAmountAccount";
                            if (data.Exists(x => x.IsKeepSession))
                            {
                                process = "Finish";
                            }
                            break;
                        case "CheckAmountAccount":
                            var currentMoney = GetAmountAccount();
                            var isAmountEnough = false;
                            if (currentMoney > 0)
                            {
                                isAmountEnough = CheckAmoutAccount(currentMoney);
                            }
                            await Task.Delay(5000);

                            if (!isAmountEnough)
                            {
                                if (!Globals.isSentNotification_TC)
                                {
                                    Globals.isSentNotification_TC = true;
                                    SendNotificationForError("Account không đủ số tiền tối thiểu",
                                        $"{web_name} : Account admin không đủ số tiền tối thiểu");
                                }
                            }
                            else
                            {
                                Globals.isSentNotification_TC = false;
                            }

                            moneyLeft = currentMoney;
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
                            await Task.Delay(2000);

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
                            await Task.Delay(5000);

                            var userName = GetUserName();
                            await Task.Delay(2000);

                            CreateSyncTask();
                            PayInSubmit();
                            await tcs.Task;
                            await Task.Delay(5000);

                            if (!webLayout.Url.ToString().Contains(agencies_URL))
                            {
                                var errorMessage = $"Cộng tiền account { currentMessage.Account } bị lỗi";
                                SaveRecord(errorMessage);

                                SendNotificationForError(
                                     "Cộng tiền không thành công",
                                     $"{Constant.SIEUNHANH.ToUpper()} : Lỗi + { helper.GetMoneyFormat(currentMessage.Money) } { currentMessage.Web }{ currentMessage.Account } ({ userName })");
                            }
                            else
                            {
                                var moneyAfterPay = moneyLeft - decimal.Parse(currentMessage.Money);
                                SaveRecord();
                                SendNotificationForError(
                                     "Cộng tiền thành công",
                                     $"{Constant.SIEUNHANH.ToUpper()} : Đã + { helper.GetMoneyFormat(currentMessage.Money) } { currentMessage.Web }{ currentMessage.Account } ({ userName }), SD: { helper.GetMoneyFormat(moneyAfterPay.ToString()) } ");
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
            var otpSetting = adminSetting.Query.Where(x => x.Name == "OTP" && x.Key.ToLower() == Constant.SIEUNHANH).FirstOrDefault();
            var otpValue = otpSetting?.Value ?? string.Empty;
            GeckoLinkElement btnLogin = new GeckoLinkElement(htmlLogin.GetElementsByName("login")[0].DomObject);

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
                var href = url + item.GetAttribute("href").TrimStart('/');
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

            if (userAccount == null || string.IsNullOrEmpty(userAccount.SN))
                return null;
            return userAccount;
        }

        private void SearchUserClick(AccountData userAccount)
        {
            var html = webLayout.Document;
            var userFilter = html.GetElementById("phone");
            userFilter.SetAttribute("value", userAccount.SN);
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

        private decimal GetAmountAccount()
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
                    var temp = tdResult.TextContent.Trim();
                    var matches = new Regex(Constant.REG_EXTRACT_SO_DU, RegexOptions.IgnoreCase).Match(temp).Groups;
                    if (matches.Count < 2)
                    {
                        return 0;
                    }

                    var money = matches[1].ToString();
                    decimal outMoney = 0;
                    decimal.TryParse(money.Replace("VNĐ", "").Trim(), out outMoney);
                    return outMoney;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private bool CheckAmoutAccount(decimal currentMoney)
        {
            var minimumMoney = adminSetting.Query.Where(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                                           && x.Key == Constant.SIEUNHANH).FirstOrDefault();
            return currentMoney >= decimal.Parse(minimumMoney.Value);
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
                        if (value != null && value.Trim() == accountData.SN.Trim())
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
                        GeckoLinkElement btnPay = new GeckoLinkElement(item.DomObject);
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
                    .Where(x => x.Key == Constant.SIEUNHANH).FirstOrDefault().Value;
            var money = decimal.Parse(currentMessage.Money);
            var total = money + Math.Round(money * decimal.Parse(bonus) / 100);

            amount.SetAttribute("value", total.ToString());
        }

        private string GetUserName()
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
                            if (value != null && value.Contains("TÀI KHOẢN"))
                            {
                                tdResult = tds[1];
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
                    return tdResult.TextContent.Trim();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        private void PayInSubmit()
        {
            var html = webLayout.Document;
            GeckoLinkElement btnPay = new GeckoLinkElement(html.GetElementById("add_money_button").DomObject);
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
                helper.sendMessageTelegram(message);
            }
            catch (Exception ex)
            {
                isFinishProcess = true;
            }
        }

        private void SaveRecord(string error = "")
        {
            MongoDatabase<Message> database = new MongoDatabase<Message>(typeof(Message).Name);
            var updateOption = Builders<Message>.Update;
            var updates = new List<UpdateDefinition<Message>>();

            updates.Add(updateOption.Set(p => p.Error, error));
            updates.Add(updateOption.Set(p => p.DateExcute, DateTime.Now.Date));

            if (string.IsNullOrEmpty(error))
            {
                updates.Add(updateOption.Set(p => p.IsProcessed, true));
            }
            else
            {
                if (currentMessage.TimeExecute.HasValue)
                {
                    var timeExcute = currentMessage.TimeExecute++;
                    if (timeExcute == 3)
                    {
                        updates.Add(updateOption.Set(p => p.IsProcessed, true));
                    }
                    updates.Add(updateOption.Set(p => p.TimeExecute, timeExcute));
                }
                else
                {
                    updates.Add(updateOption.Set(p => p.TimeExecute, 3));
                    updates.Add(updateOption.Set(p => p.IsProcessed, true));
                }
            }

            database.UpdateOne(x => x.Id == currentMessage.Id, updateOption.Combine(updates));
        }
    }
}
