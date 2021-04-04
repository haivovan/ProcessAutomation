using Gecko;
using Gecko.DOM;
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
    public class RegisterAccount_DNSite : IRegisterAccount
    {
        private RegisterAccount registerAccountForm;
        MailService mailService = new MailService();
        AccountService accountService = new AccountService();
        Helper helper = new Helper();
        private GeckoWebBrowser webLayout;
        private RegisterAccountModel data = new RegisterAccountModel();
        private const string web_name = "diennuoc";
        private const string url = "https://73ss.club/";
        private const string index_URL = url + "Login";
        private const string user_URL = url + "Users";
        private const string agencies_URL = url + "Users/Agencies";
        private const string addMoney_URL = url + "Users/AddMoneyToUser";
        private const string addUser_URL = url + "Users/EditUser";
        private bool isFinishProcess = true;
        Message currentMessage;
        Void v;
        TaskCompletionSource<Void> tcs = null;
        EventHandler<Gecko.Events.GeckoDocumentCompletedEventArgs> documentComplete;
        MongoDatabase<AdminSetting> adminSetting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);

        public RegisterAccount_DNSite(RegisterAccountModel data)
        {
            this.data = data;
        }

        public bool checkProcessDone()
        {
            return isFinishProcess;
        }

        public void startRegister(GeckoWebBrowser webLayout, RegisterAccount form)
        {
            this.webLayout = webLayout;
            this.registerAccountForm = form;
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
                                if (!Globals.isSentNotification_DN)
                                {
                                    Globals.isSentNotification_DN = true;
                                    SendNotificationForError("Account Admin Đăng Nhập Lỗi",
                                        $"{web_name} : Account admin đăng nhập web bị lỗi");
                                }
                                process = "Finish";
                                break;
                            }
                            Globals.isSentNotification_DN = false;
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

                            process = "AccessAddUser";
                            break;
                        case "AccessAddUser":
                            CreateSyncTask();
                            AccessToAddUser();
                            await tcs.Task;
                            await Task.Delay(5000);

                            if (webLayout.Url.ToString() != addUser_URL)
                            {
                                SendNotificationForError(
                                    "Truy cập vào thêm user bị lỗi",
                                    $"{web_name} : Trang thêm user bị lỗi");
                                process = "Finish";
                                break;
                            }

                            process = "AddUser";
                            break;
                        case "AddUser":
                            FillInUserInfor();
                            await Task.Delay(5000);

                            AddUser();
                            await tcs.Task;
                            await Task.Delay(5000);
                            
                            var errorFromServerPhp = UpdateUserStatus();
                            if (!webLayout.Url.ToString().Contains(agencies_URL))
                            {
                                var errorFromCreation = GetErrorFromCreation();
                                var errorMessage = $"Tạo user { data.WebId + data.IdNumber } bị lỗi";
                                SendNotificationForError(
                                    "Tạo user không thành công",
                                    (!string.IsNullOrEmpty(errorFromServerPhp) ? $"Có lỗi update từ server php {errorFromServerPhp}" + Environment.NewLine : string.Empty) +
                                    $"{Constant.DIENNUOC.ToUpper()} : user { data.WebId + data.IdNumber } lỗi {errorFromCreation}");
                            }
                            else
                            {
                                SendNotificationForError(
                                    $"Tạo user thành công cho web {Constant.DIENNUOC.ToUpper()}",
                                    (!string.IsNullOrEmpty(errorFromServerPhp) ? $"Tạo thành công nhưng có lỗi update từ server php {errorFromServerPhp}" + Environment.NewLine : string.Empty) +
                                    $"Thông tin:" + Environment.NewLine +
                                    $"Họ tên: {data.Name}" + Environment.NewLine +
                                    $"Sdt: {data.Phone}" + Environment.NewLine +
                                    $"Tên web: {Constant.DIENNUOC.ToUpper()} ({data.WebId})" + Environment.NewLine +
                                    $"Link web: {url}" + Environment.NewLine +
                                    $"Tk({data.GetLevel()}): {data.WebId + data.IdNumber}" + Environment.NewLine +
                                    $"Mk: {data.Password}" + Environment.NewLine +
                                    $"Nội dung chuyển khoản nạp số dư: {data.WebId + data.IdNumber}" + Environment.NewLine +
                                    $"STK VPBank: 97229748 - PHAN MINH CHÂU" + Environment.NewLine +
                                    $"(ACE lưu ý chọn chuyển nhanh 24 / 7 & chuyển khoản đúng nội dung quy định sẽ được tự động cộng vào tài khoản trong 3p.Trường hợp chuyển khoản sai xử lý vào cuối ngày)" + Environment.NewLine +
                                    $"Cần liên hệ ACE liên hệ Zalo 0981 694 994");
                            }
                            process = "Finish";
                            break;
                        case "Finish":
                            isFinishProcess = true;
                            registerAccountForm.Dispose();
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
            var otpSetting = adminSetting.Query.Where(x => x.Name == "OTP" && x.Key.ToLower() == Constant.DIENNUOC).FirstOrDefault();
            var otpValue = otpSetting.Value ?? string.Empty;
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

        private void AccessToAddUser()
        {
            var htmlIndex = webLayout.Document;
            var aTag = htmlIndex.GetElementsByTagName("a");
            foreach (GeckoHtmlElement item in aTag)
            {
                var href = url + item.GetAttribute("href").TrimStart('/');
                if (href != null && href == addUser_URL)
                {
                    item.Click();
                    break;
                }
            }
        }

        private void FillInUserInfor()
        {
            var html = webLayout.Document;
            var txtUserName = html.GetElementById("Username");
            var txtPassword = html.GetElementById("Password");
            var txtPhone = html.GetElementById("Phone");
            var txtFullName = html.GetElementById("FullName");
            var txtRateVT136 = html.GetElementById("RateVT136");
            var txtRateVT199 = html.GetElementById("RateVT199");
            var txtRateVTOrder = html.GetElementById("RateVTOrder");
            var txtRateMobiOrder = html.GetElementById("RateMobiOrder");
            var txtRateVinaOrder = html.GetElementById("RateVinaOrder");
            var txtRateVNMOrder = html.GetElementById("RateVNMOrder");
            var txtRateVT = html.GetElementById("RateVT");
            var txtRateMobi = html.GetElementById("RateMobi");
            var txtRateVina = html.GetElementById("RateVina");
            var txtRateSMS = html.GetElementById("RateSMS");
            var txtRateKPlus = html.GetElementById("RateKPlus");
            var txtPhysCardRate = html.GetElementById("PhysCardRate");

            var bonus = data.Percent;
            txtUserName.SetAttribute("value", data.WebId + data.IdNumber);
            txtPassword.SetAttribute("value", data.Password);
            txtFullName.SetAttribute("value", data.Name);
            txtPhone.SetAttribute("value", data.Phone);
            txtRateVT136.SetAttribute("value", bonus);
            txtRateVT199.SetAttribute("value", bonus);
            txtRateVT199.SetAttribute("value", bonus);
            txtRateVTOrder.SetAttribute("value", bonus);
            txtRateMobiOrder.SetAttribute("value", bonus);
            txtRateVinaOrder.SetAttribute("value", bonus);
            txtRateVNMOrder.SetAttribute("value", bonus);
            txtRateVT.SetAttribute("value", bonus);
            txtRateMobi.SetAttribute("value", bonus);
            txtRateVina.SetAttribute("value", bonus);
            txtRateSMS.SetAttribute("value", bonus);
            txtRateKPlus.SetAttribute("value", bonus);
            txtPhysCardRate.SetAttribute("value", bonus);
        }

        private void AddUser()
        {
            var html = webLayout.Document;
            var inputTag = html.GetElementsByTagName("input");
            foreach (GeckoHtmlElement item in inputTag)
            {
                var name = item.GetAttribute("name");
                if (name != null && name == "login")
                {
                    item.Click();
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

        private string UpdateUserStatus()
        {
            try
            {
                return accountService.UpdateAccountStatus(data.Id);
            }
            catch (Exception ex)
            {
                isFinishProcess = true;
            }
            return string.Empty;
        }

        private string GetErrorFromCreation()
        {
            var html = webLayout.Document;
            var form = html.GetElementsByTagName("form");
            foreach (GeckoHtmlElement item in form)
            {
                var p = item.GetElementsByTagName("P");
                foreach (GeckoHtmlElement temp in p)
                {
                    if (temp.InnerHtml.Contains("field-validation-error"))
                    {
                        return temp.TextContent;
                    }
                }
            }
            return "Lỗi không xác định";
        }

        private void SendNotificationForError(string subject, string message)
        {
            try
            {
                helper.sendMessageTelegram(message, Constant.CHAT_ID_CHAU);
            }
            catch (Exception ex)
            {
                isFinishProcess = true;
            }
        }
    }
}
