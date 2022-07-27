using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.PayIn;
using ProcessAutomation.Main.Services;
using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using MongoDB.Bson.IO;
using System.Media;
using System.IO;
using MongoDB.Driver.Builders;
using System.Globalization;
using RestSharp;
using System.Net.Http;
using RestSharp.Serialization.Json;
using Gecko;
using System.Threading.Tasks;

namespace ProcessAutomation.Main
{
    public partial class Main : Form
    {
        DevicePortCOMService serialPortService = new DevicePortCOMService();
        SerialPort serialPort = new SerialPort();
        MessageService messageService = new MessageService();
        IAutomationPayIn iAutomationPayin;
        bool isCurrentPayInProcessDone = true;
        Dictionary<string, List<Message>> listMessage = new Dictionary<string, List<Message>>();
        System.Timers.Timer timerAnalyzeMessage;
        System.Timers.Timer timerReadMessageFromDevice;
        MessageContition messageContition = new MessageContition();
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
        SoundPlayer audio = new SoundPlayer(Properties.Resources.ring1);
        AccountService accountService = new AccountService();
        OTPService otpService = new OTPService();

        public Main()
        {
            Xpcom.Initialize("Firefox");
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //if (checkLicense())
            //{
            //isQualified = true;
            AddPortsToCombobox();
                InitAllTimer();
                InitControl();
            //}
            //else
            //{
            //    tabControl.Hide();
            //    // Creating and setting the label 
            //    Label illegaLabel = new Label(); 
            //    illegaLabel.Text = "Eyyyyy! Đừng Xài Lậu Chứ Fen :)";
            //    illegaLabel.Location = new Point(300, 300);
            //    illegaLabel.AutoSize = true;
            //    illegaLabel.Font = new Font("Calibri", 50);
            //    illegaLabel.ForeColor = Color.Red;

            //    // Adding this control to the form 
            //    this.Controls.Add(illegaLabel);

            //}
        }

        private bool checkLicense()
        {
            var macAddr =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();

            var database = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);
            string license = database.Query.Where(x => x.Name == "License").FirstOrDefault().Value;
            return license.ToLower() == GetStringSha256Hash(macAddr + DateTime.Now.Year.ToString());
        }

        private void btnStartReadMessage_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("Chưa kết nối thiết bị");
                return;
            }

            lblErrorReadMessage.Hide();
            lblReadMessageProgress.Show();
            btnStopReadMessage.Show();
            btnStartReadMessage.Hide();

            if (!timerAnalyzeMessage.Enabled)
                timerAnalyzeMessage.Start();
        }

        private void btnStopReadMessage_Click(object sender, EventArgs e)
        {
            lblReadMessageProgress.Hide();
            timerAnalyzeMessage.Stop();
            btnStopReadMessage.Hide();
            btnStartReadMessage.Show();
        }

        private void btnStartPayIn_Click(object sender, EventArgs e)
        {
            btnStopPayIn.Show();
            btnStartPayIn.Hide();
            lblPayInProgress.Show();
            if (!timerCheckPayInProcess.Enabled)
                timerCheckPayInProcess.Start();
        }

        private void btnStopPayIn_Click(object sender, EventArgs e)
        {
            lblPayInProgress.Hide();
            timerCheckPayInProcess.Stop();
            btnStopPayIn.Hide();
            btnStartPayIn.Show();
        }

        private void connectPortBtn_Click(object sender, EventArgs e)
        {
            var portName = SerialPortCombobox.Text;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("Hãy chọn cổng kết nối");
                return;
            }

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort = null;
            }

            serialPort = serialPortService.GetPortCOM(portName);
            if (serialPort == null)
            {
                MessageBox.Show("Lỗi thiết bị, hãy kiểm tra lại");
                return;
            }
            //MessageBox.Show("Kết nối thiết bị thành công");
            timerReadMessageFromDevice.Start();
        }

        private void StartReadMessageFromDevice(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerReadMessageFromDevice.Stop();
                messageService.ReadMessageFromDevice(serialPort);
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(() =>
                {
                    btnStopReadMessage.Hide();
                    btnStartReadMessage.Show();
                    timerReadMessageFromDevice.Stop();
                    lblErrorReadMessage.Text = "Có lỗi hệ thống khi đọc tin nhắn: " + ex.Message
                        + Environment.NewLine + "Hãy kiểm tra và bắt đầu lại";
                }));
            }
            finally
            {
                timerReadMessageFromDevice.Start();
            }
        }


        private void StartReadMessage(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerAnalyzeMessage.Stop();
                Thread.Sleep(2000);
                if (messageService.StartReadMessage())
                {
                    audio.Play();
                }
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(() =>
                {
                    btnStopReadMessage.Hide();
                    btnStartReadMessage.Show();
                    timerAnalyzeMessage.Stop();
                    lblErrorReadMessage.Text = "Có lỗi hệ thống khi phân tích tin nhắn: " + ex.Message
                        + Environment.NewLine + "Hãy kiểm tra và bắt đầu lại";
                }));
            }
            finally
            {
                //if (cbStopAutoLoadMess.Checked) showSearchMessage();
                timerAnalyzeMessage.Start();
            }
        }

        private void StartGettingAccountAndCreate(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<RegisterAccount>().Any())
            {
                return;
            }

            try
            {
                var registerModel = accountService.GetNewAccount();
                if (registerModel.Id == 0)
                {
                    return;
                }

                IRegisterAccount registerAccount = null;
                switch (registerModel.WebId.ToLower())
                {
                    case Constant.BANHKEO:
                        registerAccount = new RegisterAccount_BKSite(registerModel);
                        break;
                    //case Constant.HANHLANG:
                    //    registerAccount = new RegisterAccount_HLCSite(registerModel);
                    //    break;
                    //case Constant.LANQUEPHUONG:
                    //    registerAccount = new RegisterAccount_LQSite(registerModel);
                    //    break;
                    //case Constant.DIENNUOC:
                    //    registerAccount = new RegisterAccount_DNSite(registerModel);
                    //    break;
                    //case Constant.NAPAZ:
                    //    registerAccount = new RegisterAccount_AZSite(registerModel);
                    //    break;
                    //case Constant.TRUMLANG:
                    //    registerAccount = new RegisterAccount_TLSite(registerModel);
                    //    break;
                    //case Constant.NAP3S:
                    //    registerAccount = new RegisterAccount_NAP3SSite(registerModel);
                    //    break;
                    //case Constant.SIEUNHANH:
                    //    registerAccount = new RegisterAccount_SNSite(registerModel);
                    //    break;
                    //case Constant.MEOMUOP:
                    //    registerAccount = new RegisterAccount_MMSite(registerModel);
                    //    break;
                }

                RegisterAccount form = new RegisterAccount(registerAccount);
                form.Show(this);
                form.StartRegister(form);
            }
            catch (Exception)
            {
            }
        }

        private void StartCheckingNewOTP(object sender, EventArgs e)
        {
            try
            {
                otpService.CrawlingNewOTP();
            }
            catch (Exception)
            {
            }
        }
        
        private void StartPayIn(object sender, EventArgs e)
        {
            try
            {
                if (!isCurrentPayInProcessDone)
                    return;

                listMessage = GetMessageToRun();
                if (listMessage.Count == 0)
                    isCurrentPayInProcessDone = true;
                else
                {
                    isCurrentPayInProcessDone = false;
                    if (!timerCheckChildProcess.Enabled)
                    {
                        timerCheckChildProcess.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                btnStopPayIn.Hide();
                btnStartPayIn.Show();
                MessageBox.Show(ex.Message);
            }
        }

        private void Process(object sender, EventArgs e)
        {
            try
            {
                if (listMessage.Count == 0)
                {
                    isCurrentPayInProcessDone = true;
                    timerCheckChildProcess.Stop();
                    return;
                }
                if (listMessage.ContainsKey(Constant.BANHKEO) && listMessage[Constant.BANHKEO].Count > 0)
                {
                    if (iAutomationPayin == null || !(iAutomationPayin is BKSite))
                    {
                        iAutomationPayin = new BKSite(new List<Message>(listMessage[Constant.BANHKEO]), webLayout);
                        iAutomationPayin.startPayIN();
                    }

                    if (!iAutomationPayin.checkProcessDone())
                        return;

                    listMessage.Remove(Constant.BANHKEO);
                    iAutomationPayin = null;
                    //showSearchMessage();
                }
                //else if (listMessage.ContainsKey(Constant.HANHLANG) && listMessage[Constant.HANHLANG].Count > 0)
                //{
                //    if (iAutomationPayin == null || !(iAutomationPayin is HLCSite))
                //    {
                //        iAutomationPayin = new HLCSite(new List<Message>(listMessage[Constant.HANHLANG]), webLayout);
                //        iAutomationPayin.startPayIN();
                //    }

                //    if (!iAutomationPayin.checkProcessDone())
                //        return;

                //    listMessage.Remove(Constant.HANHLANG);
                //    iAutomationPayin = null;
                //    showSearchMessage();
                //}
                //else if (listMessage.ContainsKey(Constant.MH) && listMessage[Constant.MH].Count > 0)
                //{
                //    if (iAutomationPayin == null || !(iAutomationPayin is MHSite))
                //    {
                //        iAutomationPayin = new MHSite(new List<Message>(listMessage[Constant.MH]), webLayoutIE);
                //        iAutomationPayin.startPayIN();
                //    }

                //    if (!iAutomationPayin.checkProcessDone())
                //        return;

                //    listMessage.Remove(Constant.MH);
                //    iAutomationPayin = null;
                //    showSearchMessage();
                //}
                //else if (listMessage.ContainsKey(Constant.LANQUEPHUONG) && listMessage[Constant.LANQUEPHUONG].Count > 0)
                //{
                //    if (iAutomationPayin == null || !(iAutomationPayin is LQSite))
                //    {
                //        iAutomationPayin = new LQSite(new List<Message>(listMessage[Constant.LANQUEPHUONG]), webLayoutIE);
                //        iAutomationPayin.startPayIN();
                //    }

                //    if (!iAutomationPayin.checkProcessDone())
                //        return;

                //    listMessage.Remove(Constant.LANQUEPHUONG);
                //    iAutomationPayin = null;
                //    showSearchMessage();
                //}
                //else if (listMessage.ContainsKey(Constant.DIENNUOC) && listMessage[Constant.DIENNUOC].Count > 0)
                //{
                //    if (iAutomationPayin == null || !(iAutomationPayin is DNSite))
                //    {
                //        iAutomationPayin = new DNSite(new List<Message>(listMessage[Constant.DIENNUOC]), webLayout);
                //        iAutomationPayin.startPayIN();
                //    }

                //    if (!iAutomationPayin.checkProcessDone())
                //        return;

                //    listMessage.Remove(Constant.DIENNUOC);
                //    iAutomationPayin = null;
                //    showSearchMessage();
                //}
                //else if (listMessage.ContainsKey(Constant.NAPAZ) && listMessage[Constant.NAPAZ].Count > 0)
                //{
                //    if (iAutomationPayin == null || !(iAutomationPayin is AZSite))
                //    {
                //        iAutomationPayin = new AZSite(new List<Message>(listMessage[Constant.NAPAZ]), webLayout);
                //        iAutomationPayin.startPayIN();
                //    }

                //    if (!iAutomationPayin.checkProcessDone())
                //        return;

                //    listMessage.Remove(Constant.NAPAZ);
                //    iAutomationPayin = null;
                //    showSearchMessage();
                //}
                //else if (listMessage.ContainsKey(Constant.TRUMLANG) && listMessage[Constant.TRUMLANG].Count > 0)
                //{
                //    if (iAutomationPayin == null || !(iAutomationPayin is TLSite))
                //    {
                //        iAutomationPayin = new TLSite(new List<Message>(listMessage[Constant.TRUMLANG]), webLayout);
                //        iAutomationPayin.startPayIN();
                //    }

                //    if (!iAutomationPayin.checkProcessDone())
                //        return;

                //    listMessage.Remove(Constant.TRUMLANG);
                //    iAutomationPayin = null;
                //    showSearchMessage();
                //}
                //else if (listMessage.ContainsKey(Constant.NAP3S) && listMessage[Constant.NAP3S].Count > 0)
                //{
                //    if (iAutomationPayin == null || !(iAutomationPayin is NAP3SSite))
                //    {
                //        iAutomationPayin = new NAP3SSite(new List<Message>(listMessage[Constant.NAP3S]), webLayout);
                //        iAutomationPayin.startPayIN();
                //    }

                //    if (!iAutomationPayin.checkProcessDone())
                //        return;

                //    listMessage.Remove(Constant.NAP3S);
                //    iAutomationPayin = null;
                //    showSearchMessage();
                //}
                //else if (listMessage.ContainsKey(Constant.SIEUNHANH) && listMessage[Constant.SIEUNHANH].Count > 0)
                //{
                //    if (iAutomationPayin == null || !(iAutomationPayin is SNSite))
                //    {
                //        iAutomationPayin = new SNSite(new List<Message>(listMessage[Constant.SIEUNHANH]), webLayout);
                //        iAutomationPayin.startPayIN();
                //    }

                //    if (!iAutomationPayin.checkProcessDone())
                //        return;

                //    listMessage.Remove(Constant.SIEUNHANH);
                //    iAutomationPayin = null;
                //    showSearchMessage();
                //}
                //else if (listMessage.ContainsKey(Constant.MEOMUOP) && listMessage[Constant.MEOMUOP].Count > 0)
                //{
                //    if (iAutomationPayin == null || !(iAutomationPayin is MMSite))
                //    {
                //        iAutomationPayin = new MMSite(new List<Message>(listMessage[Constant.MEOMUOP]), webLayout);
                //        iAutomationPayin.startPayIN();
                //    }

                //    if (!iAutomationPayin.checkProcessDone())
                //        return;

                //    listMessage.Remove(Constant.MEOMUOP);
                //    iAutomationPayin = null;
                //    showSearchMessage();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddPortsToCombobox()
        {
            var portNames = SerialPort.GetPortNames();
            if (portNames != null && portNames.Length > 0)
            {
                SerialPortCombobox.Items.AddRange(portNames);
                SerialPortCombobox.SelectedIndex = portNames.Length - 1;
            }
            else
            {
                MessageBox.Show("Hãy kết nối thiết bị");
            }
        }
        async Task PutTaskDelay()
        {
            await Task.Delay(5000);
        }
        private async void btnShowHistory_Click(object sender, EventArgs e)
        {
            webLayout.Navigate("https://khoaisan.net/");
            await PutTaskDelay();
            var htmlLogin = webLayout.Document;
            var inputUserName = htmlLogin.GetElementById("Username");
            var inputPassword = htmlLogin.GetElementById("Password");
            var inputOTP = htmlLogin.GetElementById("OTP");
            //var otpSetting = adminSetting.Query.Where(x => x.Name == "OTP" && x.Key.ToLower() == Constant.BANHKEO).FirstOrDefault();
            //var otpValue = otpSetting?.Value ?? string.Empty;
            GeckoHtmlElement btnLogin = (GeckoHtmlElement)htmlLogin.GetElementsByName("login")[0];

            if (inputUserName != null && inputPassword != null)
            {
                inputUserName.SetAttribute("value", "123");
                inputPassword.SetAttribute("value", "123");
                inputOTP.SetAttribute("value", "123");
                await PutTaskDelay();
                btnLogin.Click();
                await PutTaskDelay();
            }
            //showSearchMessage();
        }

        private void showSearchMessage()
        {
            this.Invoke(new Action(() =>
            {
                var account = txtAccount_filter.Text.Trim();
                List<string> selectedList = new List<string>();
                foreach (var item in web_listBox_filter.SelectedItems)
                {
                    selectedList.Add(item.ToString());
                }
                var database = new MongoDatabase<Message>(typeof(Message).Name);
                List<Message> listMessge = database.Query
                    .Where(x => (cbStopAutoLoadMess.Checked) ||
                        (x.DateExcute >= dateExecuteFrom.Value.Date && x.DateExcute <= dateExecuteTo.Value.Date))
                    .Where(x =>
                    (web_listBox_filter.SelectedItems.Count == 1 && selectedList[0].Equals("Tất Cả"))
                        || (web_listBox_filter.SelectedItems.Count == 9) || selectedList.Contains(x.Web))
                    .Where(x => string.IsNullOrEmpty(account) || x.Account == account)
                    .Where(x => (isSatisfied_filter.SelectedItem.ToString().Equals("Tất Cả"))
                        || (isSatisfied_filter.SelectedItem.ToString().Equals("Hợp Lệ") && x.IsSatisfied)
                        || (isSatisfied_filter.SelectedItem.ToString().Equals("Không") && !x.IsSatisfied))
                    .Where(x => (isProcessed_filter.SelectedItem.ToString().Equals("Tất Cả"))
                        || (isProcessed_filter.SelectedItem.ToString().Equals("Rồi") && x.IsProcessed)
                        || (isProcessed_filter.SelectedItem.ToString().Equals("Chưa") && !x.IsProcessed))
                    .Where(x => (isError_filter.SelectedItem.ToString().Equals("Tất Cả"))
                        || (isSatisfied_filter.SelectedItem.ToString().Equals("Có") && !string.IsNullOrEmpty(x.Error))
                        || (isSatisfied_filter.SelectedItem.ToString().Equals("Không") && string.IsNullOrEmpty(x.Error)))
                    .ToList();

                dataGridView1.Columns[7].DefaultCellStyle.Format = "dd/MM/yyyy";
                dataGridView1.Columns[4].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns[4].Frozen = false;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12);
                dataGridView1.ScrollBars = ScrollBars.Both;

                if (cbStopAutoLoadMess.Checked)
                {
                    listMessge = listMessge.OrderByDescending(x => x.Id).Take(50).ToList();
                    txtTotal.Text = "... VND";
                }
                else
                {
                    listMessge = listMessge.OrderByDescending(x => x.Id).ToList();
                    var total = listMessge.Sum(x =>
                    decimal.TryParse(x.Money, out decimal val)
                    ? val : 0);
                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                    txtTotal.Text = total.ToString("#,###", cul.NumberFormat) + " VND";
                }
                dataGridView1.DataSource = listMessge;
            }));
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                if (e.Value != null && e.Value is BsonDateTime)
                {
                    e.Value = DateTime.Parse(e.Value.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                }
            }

            foreach (DataGridViewRow Myrow in dataGridView1.Rows)
            {
                if ((Myrow.Cells[8].Value != null &&
                    !string.IsNullOrEmpty(Myrow.Cells[8].Value.ToString())) ||
                    (Myrow.Cells[5].Value != null &&
                     Myrow.Cells[5].Value is bool && !((bool)Myrow.Cells[5].Value)))
                {
                    Myrow.DefaultCellStyle.BackColor = Color.Bisque;
                }
                else
                {
                    Myrow.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private Dictionary<string, List<Message>> GetMessageToRun()
        {
            return messageService.ReadMessage(messageContition);
        }

        private void InitAllTimer()
        {
            timerReadMessageFromDevice = new System.Timers.Timer(500);
            timerReadMessageFromDevice.AutoReset = true;
            timerReadMessageFromDevice.Elapsed += new ElapsedEventHandler(this.StartReadMessageFromDevice);

            timerAnalyzeMessage = new System.Timers.Timer(10000);
            timerAnalyzeMessage.AutoReset = false;
            timerAnalyzeMessage.Elapsed += new ElapsedEventHandler(this.StartReadMessage);

            timerCheckPayInProcess = new System.Windows.Forms.Timer();
            timerCheckPayInProcess.Interval = (10000);
            timerCheckPayInProcess.Tick += new EventHandler(StartPayIn);

            timerCheckChildProcess = new System.Windows.Forms.Timer();
            timerCheckChildProcess.Interval = (5000);
            timerCheckChildProcess.Tick += new EventHandler(Process);

            timerCheckNewAccount = new System.Windows.Forms.Timer();
            timerCheckNewAccount.Interval = (5000);
            timerCheckNewAccount.Tick += new EventHandler(StartGettingAccountAndCreate);

            timerCheckNewOTP = new System.Windows.Forms.Timer();
            timerCheckNewOTP.Interval = (30000);
            timerCheckNewOTP.Tick += new EventHandler(StartCheckingNewOTP);
            timerCheckNewOTP.Start();
        }

        private void InitControl()
        {
            lblErrorReadMessage.Hide();
            btnStopReadMessage.Hide();
            btnStopPayIn.Hide();
            lblReadMessageProgress.Hide();
            lblPayInProgress.Hide();
            btnStopCreateAccount.Hide();

            isProcessed_filter.Items.Add(new ComboboxItem() { Text = "Tất Cả", Value = "" });
            isProcessed_filter.Items.Add(new ComboboxItem() { Text = "Rồi", Value = true });
            isProcessed_filter.Items.Add(new ComboboxItem() { Text = "Chưa", Value = false });
            isProcessed_filter.SelectedIndex = 0;

            isSatisfied_filter.Items.Add(new ComboboxItem() { Text = "Tất Cả", Value = "" });
            isSatisfied_filter.Items.Add(new ComboboxItem() { Text = "Hợp Lệ", Value = true });
            isSatisfied_filter.Items.Add(new ComboboxItem() { Text = "Không", Value = false });
            isSatisfied_filter.SelectedIndex = 0;

            isError_filter.Items.Add(new ComboboxItem() { Text = "Tất Cả", Value = "" });
            isError_filter.Items.Add(new ComboboxItem() { Text = "Có", Value = true });
            isError_filter.Items.Add(new ComboboxItem() { Text = "Không", Value = false });
            isError_filter.SelectedIndex = 0;

            web_listBox_filter.Items.Add(Constant.ALL);
            web_listBox_filter.Items.Add(Constant.BANHKEO);
            web_listBox_filter.Items.Add(Constant.HANHLANG);
            web_listBox_filter.Items.Add(Constant.DIENNUOC);
            web_listBox_filter.Items.Add(Constant.NAPAZ);
            web_listBox_filter.Items.Add(Constant.TRUMLANG);
            web_listBox_filter.Items.Add(Constant.NAP3S);
            web_listBox_filter.Items.Add(Constant.SIEUNHANH);
            web_listBox_filter.Items.Add(Constant.MEOMUOP);

            web_listBox_filter.SetSelected(0, true);
            web_listBox_filter.SetSelected(1, true);
            web_listBox_filter.SetSelected(2, true);
            web_listBox_filter.SetSelected(3, true);
            web_listBox_filter.SetSelected(4, true);
            web_listBox_filter.SetSelected(5, true);
            web_listBox_filter.SetSelected(6, true);
            web_listBox_filter.SetSelected(7, true);
            web_listBox_filter.SetSelected(8, true);

            messageContition.WebSRun.Add(Constant.BANHKEO);
            messageContition.WebSRun.Add(Constant.HANHLANG);
            messageContition.WebSRun.Add(Constant.DIENNUOC);
            messageContition.WebSRun.Add(Constant.NAPAZ);
            messageContition.WebSRun.Add(Constant.TRUMLANG);
            messageContition.WebSRun.Add(Constant.NAP3S);
            messageContition.WebSRun.Add(Constant.SIEUNHANH);
            messageContition.WebSRun.Add(Constant.MEOMUOP);
            cbStopAutoLoadMess.Checked = true;
            //showSearchMessage();
        }

        private void SettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Setting formSetting = new Setting())
            {
                formSetting.webToRun = messageContition.WebSRun;
                if (formSetting.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    messageContition.WebSRun = formSetting.webToRun;
                    formSetting.Close();
                }
            }
        }

        private void cbStopAutoLoadMess_CheckedChanged(object sender, EventArgs e)
        {
            if (cbStopAutoLoadMess.Checked)
            {
                btnShowHistory.Enabled = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                dataGridView1.EnableHeadersVisualStyles = false;
            }
            else
            {
                btnShowHistory.Enabled = true;
                dataGridView1.ReadOnly = false;
                dataGridView1.Columns[7].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView1.EnableHeadersVisualStyles = false;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridView1.Rows[e.RowIndex];
            if (row != null)
            {
                var id = (ObjectId)row.Cells["id"].Value;
                var web = row.Cells[0].Value == null ? "" : row.Cells[0].Value.ToString().Trim();
                var account = row.Cells[1].Value == null ? "" : row.Cells[1].Value.ToString().Trim();
                decimal money = 0;
                if (decimal.TryParse(row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString(), out money))
                {
                }
                var IsSatisfied = (bool)row.Cells[5].Value;
                var IsProcessed = (bool)row.Cells[6].Value;
                var Error = row.Cells[8].Value == null ? "" : row.Cells[8].Value.ToString().Trim();

                MongoDatabase<Message> database = new MongoDatabase<Message>(typeof(Message).Name);
                var updateOption = Builders<Message>.Update
                .Set(p => p.Web, web)
                .Set(p => p.Account, account)
                .Set(p => p.Money, money.ToString())
                .Set(p => p.IsProcessed, IsProcessed)
                .Set(p => p.IsSatisfied, IsSatisfied)
                .Set(p => p.Error, Error);

                database.UpdateOne(x => x.Id == id, updateOption);
            }
        }
        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty).ToLower();
            }
        }

        private void btnStartCreateAccount_Click(object sender, EventArgs e)
        {
            timerCheckNewAccount.Start();
            btnStartCreateAccount.Hide();
            btnStopCreateAccount.Show();
        }

        private void btnStopCreateAccount_Click(object sender, EventArgs e)
        {
            timerCheckNewAccount.Stop();
            btnStopCreateAccount.Hide();
            btnStartCreateAccount.Show();
        }
    }
}