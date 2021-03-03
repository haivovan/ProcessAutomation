﻿using MongoDB.Driver;
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
            if (webToRun.IndexOf(Constant.BANHKEO) != -1) cbBanhKeo.Checked = true;
            if (webToRun.IndexOf(Constant.HANHLANG) != -1) cbHanhLang.Checked = true;
            if (webToRun.IndexOf(Constant.DIENNUOC) != -1) cbDienNuoc.Checked = true;
            if (webToRun.IndexOf(Constant.NAPAZ) != -1) cbNapaz.Checked = true;
            if (webToRun.IndexOf(Constant.TRUMLANG) != -1) cbTrumLang.Checked = true;
            if (webToRun.IndexOf(Constant.NAP3S) != -1) cbNap3S.Checked = true;

            GetSettingMinimumMoney();
            GetSettingBonus();
            GetSettingMinimumPayMoney();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            webToRun = new List<string>();
            //if (cbCayBang.Checked) webToRun.Add(Constant.CAYBANG);
            if (cbBanhKeo.Checked) webToRun.Add(Constant.BANHKEO);
            if (cbHanhLang.Checked) webToRun.Add(Constant.HANHLANG);
            if (cbDienNuoc.Checked) webToRun.Add(Constant.DIENNUOC);
            if (cbNapaz.Checked) webToRun.Add(Constant.NAPAZ);
            if (cbTrumLang.Checked) webToRun.Add(Constant.TRUMLANG);
            if (cbNap3S.Checked) webToRun.Add(Constant.NAP3S);

            MongoDatabase<AdminSetting> database = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);
            var updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoney_BK.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                && x.Key == Constant.BANHKEO, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoney_HL.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                && x.Key == Constant.HANHLANG, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoney_DN.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                && x.Key == Constant.DIENNUOC, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoney_AZ.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                && x.Key == Constant.NAPAZ, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoney_TL.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                && x.Key == Constant.TRUMLANG, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoney_3S.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_MONEY_NAME
                                && x.Key == Constant.NAP3S, updateOption);

            UpdateBonus(updateOption, database);
            UpdateMinPayMoney(updateOption, database);
        }
        
        private void UpdateBonus(UpdateDefinition<AdminSetting> updateOption, MongoDatabase<AdminSetting> database)
        {
            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtBonus_BK.Text);
            database.UpdateOne(x => x.Name == Constant.BONUS
                                && x.Key == Constant.BANHKEO, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtBonus_HL.Text);
            database.UpdateOne(x => x.Name == Constant.BONUS
                                && x.Key == Constant.HANHLANG, updateOption);


            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtBonus_DN.Text);
            database.UpdateOne(x => x.Name == Constant.BONUS
                                && x.Key == Constant.DIENNUOC, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtBonus_AZ.Text);
            database.UpdateOne(x => x.Name == Constant.BONUS
                                && x.Key == Constant.NAPAZ, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtBonus_TL.Text);
            database.UpdateOne(x => x.Name == Constant.BONUS
                                && x.Key == Constant.TRUMLANG, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtBonus_3S.Text);
            database.UpdateOne(x => x.Name == Constant.BONUS
                                && x.Key == Constant.NAP3S, updateOption);
        }

        private void UpdateMinPayMoney(UpdateDefinition<AdminSetting> updateOption, MongoDatabase<AdminSetting> database)
        {
            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoneyPay_BK.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_PAY_MONEY_NAME
                                && x.Key == Constant.BANHKEO, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoneyPay_HL.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_PAY_MONEY_NAME
                                && x.Key == Constant.HANHLANG, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoneyPay_DN.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_PAY_MONEY_NAME
                                && x.Key == Constant.DIENNUOC, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoneyPay_AZ.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_PAY_MONEY_NAME
                                && x.Key == Constant.NAPAZ, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoneyPay_TL.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_PAY_MONEY_NAME
                                && x.Key == Constant.TRUMLANG, updateOption);

            updateOption = Builders<AdminSetting>.Update
            .Set(p => p.Value, txtMoneyPay_3S.Text.Replace(",", ""));
            database.UpdateOne(x => x.Name == Constant.MINIMUM_PAY_MONEY_NAME
                                && x.Key == Constant.NAP3S, updateOption);
        }


        private void GetSettingMinimumMoney()
        {
            var setting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);
            var minimumMoney = setting.Query.Where(x => x.Name == Constant.MINIMUM_MONEY_NAME).ToList();
            if(minimumMoney.Count > 0)
            {
                txtMoney_BK.Text = minimumMoney.Where(x => x.Key == Constant.BANHKEO).FirstOrDefault().Value;
                txtMoney_HL.Text = minimumMoney.Where(x => x.Key == Constant.HANHLANG).FirstOrDefault().Value;
                txtMoney_DN.Text = minimumMoney.Where(x => x.Key == Constant.DIENNUOC).FirstOrDefault().Value;
                txtMoney_AZ.Text = minimumMoney.Where(x => x.Key == Constant.NAPAZ).FirstOrDefault().Value;
                txtMoney_TL.Text = minimumMoney.Where(x => x.Key == Constant.TRUMLANG).FirstOrDefault().Value;
                txtMoney_3S.Text = minimumMoney.Where(x => x.Key == Constant.NAP3S).FirstOrDefault().Value;

                var value = decimal.Parse(txtMoney_BK.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoney_BK.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoney_HL.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoney_HL.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoney_DN.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoney_DN.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoney_AZ.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoney_AZ.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoney_TL.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoney_TL.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoney_3S.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoney_3S.Text = String.Format(culture, "{0:N0}", value);
            }
        }

        private void GetSettingBonus()
        {
            var setting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);
            var bonus = setting.Query.Where(x => x.Name == Constant.BONUS).ToList();
            if (bonus.Count > 0)
            {
                txtBonus_BK.Text = bonus.Where(x => x.Key == Constant.BANHKEO).FirstOrDefault().Value;
                txtBonus_HL.Text = bonus.Where(x => x.Key == Constant.HANHLANG).FirstOrDefault().Value;
                txtBonus_DN.Text = bonus.Where(x => x.Key == Constant.DIENNUOC).FirstOrDefault().Value;
                txtBonus_AZ.Text = bonus.Where(x => x.Key == Constant.NAPAZ).FirstOrDefault().Value;
                txtBonus_TL.Text = bonus.Where(x => x.Key == Constant.TRUMLANG).FirstOrDefault().Value;
                txtBonus_3S.Text = bonus.Where(x => x.Key == Constant.NAP3S).FirstOrDefault().Value;

                txtBonus_BK.Text = decimal.Parse(txtBonus_BK.Text).ToString();
                txtBonus_HL.Text = decimal.Parse(txtBonus_HL.Text).ToString();
                txtBonus_DN.Text = decimal.Parse(txtBonus_DN.Text).ToString();
                txtBonus_AZ.Text = decimal.Parse(txtBonus_AZ.Text).ToString();
                txtBonus_TL.Text = decimal.Parse(txtBonus_TL.Text).ToString();
                txtBonus_3S.Text = decimal.Parse(txtBonus_3S.Text).ToString();
            }
        }

        private void GetSettingMinimumPayMoney()
        {
            var setting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);
            var minPayMoney = setting.Query.Where(x => x.Name == Constant.MINIMUM_PAY_MONEY_NAME).ToList();
            if (minPayMoney.Count > 0)
            {
                txtMoneyPay_BK.Text = minPayMoney.Where(x => x.Key == Constant.BANHKEO).FirstOrDefault().Value;
                txtMoneyPay_HL.Text = minPayMoney.Where(x => x.Key == Constant.HANHLANG).FirstOrDefault().Value;
                txtMoneyPay_DN.Text = minPayMoney.Where(x => x.Key == Constant.DIENNUOC).FirstOrDefault().Value;
                txtMoneyPay_AZ.Text = minPayMoney.Where(x => x.Key == Constant.NAPAZ).FirstOrDefault().Value;
                txtMoneyPay_TL.Text = minPayMoney.Where(x => x.Key == Constant.TRUMLANG).FirstOrDefault().Value;
                txtMoneyPay_3S.Text = minPayMoney.Where(x => x.Key == Constant.NAP3S).FirstOrDefault().Value;

                decimal value = decimal.Parse(txtMoneyPay_BK.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoneyPay_BK.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoneyPay_HL.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoneyPay_HL.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoneyPay_DN.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoneyPay_DN.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoneyPay_AZ.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoneyPay_AZ.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoneyPay_TL.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoneyPay_TL.Text = String.Format(culture, "{0:N0}", value);

                value = decimal.Parse(txtMoneyPay_3S.Text, System.Globalization.NumberStyles.AllowThousands);
                txtMoneyPay_3S.Text = String.Format(culture, "{0:N0}", value);
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
        private void txtMoney_DN_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal value = decimal.Parse(txtMoney_DN.Text,
                System.Globalization.NumberStyles.AllowThousands);
                txtMoney_DN.Text = String.Format(culture, "{0:N0}", value);
            }
            catch
            {
                txtMoney_DN.Text = String.Format(culture, "{0:N0}", 0);
            }
        }

        private void txtMoney_AZ_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal value = decimal.Parse(txtMoney_AZ.Text,
                System.Globalization.NumberStyles.AllowThousands);
                txtMoney_AZ.Text = String.Format(culture, "{0:N0}", value);
            }
            catch
            {
                txtMoney_AZ.Text = String.Format(culture, "{0:N0}", 0);
            }
        }

        private void txtMoney_TL_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal value = decimal.Parse(txtMoney_TL.Text,
                System.Globalization.NumberStyles.AllowThousands);
                txtMoney_TL.Text = String.Format(culture, "{0:N0}", value);
            }
            catch
            {
                txtMoney_TL.Text = String.Format(culture, "{0:N0}", 0);
            }
        }

        private void txtMoney_Nap3S_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal value = decimal.Parse(txtMoney_3S.Text,
                System.Globalization.NumberStyles.AllowThousands);
                txtMoney_3S.Text = String.Format(culture, "{0:N0}", value);
            }
            catch
            {
                txtMoney_3S.Text = String.Format(culture, "{0:N0}", 0);
            }
        }
    }
}
