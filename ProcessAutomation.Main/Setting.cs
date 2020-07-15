using MongoDB.Driver;
using ProcessAutomation.DAL;
using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessAutomation.Main
{
    public partial class Setting : Form
    {
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
        public List<string> webToRun { get; set; }
        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            if (webToRun.IndexOf(Constant.CAYBANG) != -1) cbCayBang.Checked = true;
            if (webToRun.IndexOf(Constant.BANHKEO) != -1) cbBanhKeo.Checked = true;
            if (webToRun.IndexOf(Constant.HANHLANG) != -1) cbHanhLang.Checked = true;
            //if (webToRun.IndexOf(Constant.GIADINHVN) != -1) cbGiaDinh.Checked = true;
            //if (webToRun.IndexOf(Constant.NT30s) != -1) cb30s.Checked = true;

            GetSettingMinimumMoney();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            webToRun = new List<string>();
            if (cbCayBang.Checked) webToRun.Add(Constant.CAYBANG);
            if (cbBanhKeo.Checked) webToRun.Add(Constant.BANHKEO);
            if (cbHanhLang.Checked) webToRun.Add(Constant.HANHLANG);
            //if (cbGiaDinh.Checked) webToRun.Add(Constant.GIADINHVN);
            //if (cb30s.Checked) webToRun.Add(Constant.NT30s);

            var text = txtMoney_CB.Text;
            MongoDatabase<AdminSetting> database = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);
            var updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoney_CB.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME 
                                && x.Key == Constant.CAYBANG, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoney_BK.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                && x.Key == Constant.BANHKEO, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoney_HL.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                && x.Key == Constant.HANHLANG, updateOption);

            /* updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoney_GD.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                && x.Key == Constant.GIADINHVN, updateOption);

             updateOption = Builders<AdminSetting>.Update
             .Set(p => p.Value, txtMoney_30s.Text.Replace(",", ""));
             database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                && x.Key == Constant.NT30s, updateOption);
*/
        }

        private void GetSettingMinimumMoney()
        {
            var setting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);
            var minimumMoney = setting.Query.Where(x => x.Name == Constant.MINIMUM_MONEY_NAME).ToList();
            if(minimumMoney.Count > 0)
            {
                //txtMoney_30s.Text = minimumMoney.Where(x => x.Key == Constant.NT30s).FirstOrDefault().Value;
                txtMoney_CB.Text = minimumMoney.Where(x => x.Key == Constant.CAYBANG).FirstOrDefault().Value;
                txtMoney_BK.Text = minimumMoney.Where(x => x.Key == Constant.BANHKEO).FirstOrDefault().Value;
                //txtMoney_GD.Text = minimumMoney.Where(x => x.Key == Constant.GIADINHVN).FirstOrDefault().Value;
                txtMoney_HL.Text = minimumMoney.Where(x => x.Key == Constant.HANHLANG).FirstOrDefault().Value;

                //decimal value = decimal.Parse(txtMoney_30s.Text, System.Globalization.NumberStyles.AllowThousands);
                //txtMoney_30s.Text = String.Format(culture, "{0:N0}", value);

                decimal value = decimal.Parse(txtMoney_CB.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoney_CB.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoney_BK.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoney_BK.Text = String.Format(culture, "{0:N0}", value);

                //value = decimal.Parse(txtMoney_GD.Text, System.Globalization.NumberStyles.AllowThousands);
                //txtMoney_GD.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoney_HL.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoney_HL.Text = String.Format(culture, "{0:N0}", value);
            }
        }

        private void txtMoney_CB_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal value = decimal.Parse(txtMoney_CB.Text,
                System.Globalization.NumberStyles.AllowThousands);
                txtMoney_CB.Text = String.Format(culture, "{0:N0}", value);
            }
            catch
            {
                txtMoney_CB.Text = String.Format(culture, "{0:N0}", 0);
            }
        }

        private void txtMoney_BK_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal value = decimal.Parse(txtMoney_BK.Text,
                System.Globalization.NumberStyles.AllowThousands);
                txtMoney_BK.Text = String.Format(culture, "{0:N0}", value);
            }
            catch
            {
                txtMoney_BK.Text = String.Format(culture, "{0:N0}", 0);
            }
        }

        private void txtMoney_HL_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal value = decimal.Parse(txtMoney_HL.Text,
                System.Globalization.NumberStyles.AllowThousands);
                txtMoney_HL.Text = String.Format(culture, "{0:N0}", value);
            }
            catch
            {
                txtMoney_HL.Text = String.Format(culture, "{0:N0}", 0);
            }
        }

        private void txtMoney_GD_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal value = decimal.Parse(txtMoney_GD.Text,
                System.Globalization.NumberStyles.AllowThousands);
                txtMoney_GD.Text = String.Format(culture, "{0:N0}", value);
            }
            catch
            {
                txtMoney_GD.Text = String.Format(culture, "{0:N0}", 0);
            }
        }

        private void txtMoney_30s_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal value = decimal.Parse(txtMoney_30s.Text,
                System.Globalization.NumberStyles.AllowThousands);
                txtMoney_30s.Text = String.Format(culture, "{0:N0}", value);
            }
            catch
            {
                txtMoney_30s.Text = String.Format(culture, "{0:N0}", 0);
            }
        }
    }
}
