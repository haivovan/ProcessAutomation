﻿using ProcessAutomation.Main.Ultility;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MongoDB.Driver;
using ProcessAutomation.DAL;
using System.Media;
using ProcessAutomation.Main.PayIn;

namespace ProcessAutomation.Main.Services
{
    public class MessageService
    {
        MongoDatabase<Message> database = new MongoDatabase<Message>(typeof(Message).Name);
        MongoDatabase<MessageFromDevice> messFromDevice = new MongoDatabase<MessageFromDevice>(typeof(MessageFromDevice).Name);
        CSV csvHelper = new CSV();

        public void ReadMessageFromDevice(SerialPort serialPort)
        {
            serialPort.Write("AT+CMGF=1" + Environment.NewLine);
            serialPort.Write("AT+CMGL=\"ALL\"" + Environment.NewLine);
            System.Threading.Thread.Sleep(50);
            var response = serialPort.ReadExisting();
            if (!string.IsNullOrEmpty(response) &&
                response.Contains("REC UNREAD"))
            {
                SaveMessageFromDevice(response);
                System.Threading.Thread.Sleep(50);

                //Delele message after read from sim
                serialPort.Write("AT+CMGD=,4" + Environment.NewLine);
                System.Threading.Thread.Sleep(50);
                serialPort.ReadExisting();
            }
        }
        private void SaveMessageFromDevice(string fullMessage)
        {
            try
            {
                messFromDevice.InsertOne(new MessageFromDevice { Message = fullMessage, IsProcessed = false }); ;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool StartReadMessage()
        {
            var messageComming = false;
            var messages = messFromDevice.Query.Where(x => x.IsProcessed == false).ToList();
            foreach (var mess in messages)
            {
                var rule = new Regex(Constant.REG_EXTRACT_MESSAGE);
                var matches = rule.Matches(mess.Message);
                SaveMessage(matches);

                var updateOption = Builders<MessageFromDevice>.Update
                .Set(p => p.IsProcessed, true);
                messFromDevice.UpdateOne(x => x.Id == mess.Id, updateOption);
                messageComming = true;
            }
            return messageComming;
        }

        public Dictionary<string, List<Message>> ReadMessage(MessageContition messageCondition)
        {
            return database.Query
               .Where(x => x.IsProcessed == false && x.IsSatisfied == true)
               .Where(x => messageCondition.WebSRun.Contains(x.Web))
               .ToList()
               .GroupBy(x => x.Web)
               .ToDictionary(x => x.Key, x => x.ToList());
        }

        private void SaveMessage(MatchCollection matches)
        {
            try
            {
                var messageComing = 0;
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        var dataToWrite = new StringBuilder();
                        var mess = AnalyzeMessage(match.Groups[6].ToString());
                        mess.RecievedDate = string.Join(
                            string.Empty, match.Groups[4].Value.Trim()
                            .Replace("+00", "")
                            .Replace("\"", "")
                            .Skip(1));

                        if (database.Query.Any(x =>
                            x.RecievedDate.Equals(mess.RecievedDate) &&
                            x.Account.Equals(mess.Account) &&
                            x.Web.Equals(mess.Web) &&
                            x.Money.Equals(mess.Money))) {
                            continue;
                        }

                        mess.MessageContent = match.Groups[6].Value.Trim();
                        dataToWrite.AppendFormat(
                            "{0},{1},{2},{3},{4},{5},{6},{7}",
                            mess.Account,
                            mess.Money,
                            mess.Web,
                            mess.RecievedDate,
                            mess.IsSatisfied,
                            mess.IsProcessed,
                            mess.Error,
                            mess.MessageContent + Environment.NewLine
                        );
                        //messages.Add(mess);
                        database.InsertOne(mess);
                        csvHelper.WriteToFile(dataToWrite, $"{DateTime.Now.ToString("dd-MM-yyyy")}.csv");
                        messageComing++;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private Message AnalyzeMessage(string mess)
        {
            var result = new Message();
            try
            {
                // Check account
                var matches = new Regex(Constant.REG_EXTRACT_ACCOUNT, RegexOptions.IgnoreCase).Match(mess).Groups;
                if (matches.Count > 0 && matches[1].Value != "")
                {
                    result.Web = Regex.Replace(matches[1].ToString(), @"\s+", "").ToLower();
                    result.Account = Regex.Replace(matches[2].ToString().Trim(), @"[^0-9]+", "");
                    result.IsSatisfied = Constant.WEBS_NAME
                    .Any(x => result.Web.Contains(x));
                }

                // Check money
                if (result.IsSatisfied)
                {
                    var match = new Regex(Constant.REG_EXTRACT_MONEY_TEMPLATE1).Match(mess).Groups[2] ?? null;
                    if (match.Length == 0)
                    {
                        match = new Regex(Constant.REG_EXTRACT_MONEY_TEMPLATE2).Match(mess).Groups[2] ?? null;
                    }

                    if (string.IsNullOrEmpty(match.Value))
                        return result;

                    var money = match.Value?.Replace("VND", "");
                    decimal outMoney = 0;

                    result.IsSatisfied = false;
                    if (decimal.TryParse(money, out outMoney))
                    {   
                        result.Money = outMoney.ToString();
                        result.IsSatisfied = outMoney >= Constant.SATISFIED_PAYIN;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}
