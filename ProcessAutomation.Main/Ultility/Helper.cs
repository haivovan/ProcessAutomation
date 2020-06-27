using ProcessAutomation.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProcessAutomation.Main.Ultility
{
    public class Helper
    {
        public bool CheckInternetConnection()
        {
            try
            {
                using (var client = new WebClient() { Proxy = null })
                using (client.OpenRead("http://www.google.com"))
                {
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        public string DecodeFromHexToString(string hexString)
        {
            try
            {
                var bytes = new byte[hexString.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return hexString;
            }
        }

        public async void sendMessageZalo(string message)
        {
            var database = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);
            List<AdminSetting> listZaloIdReceive = database.Query.Where(x => x.Name == "zalo").ToList();
            try
            {
                foreach(var item in listZaloIdReceive)
                {
                    using (var client = new HttpClient())
                    {
                        HttpResponseMessage response = client.PostAsync("https://openapi.zalo.me/v2.0/oa/message?access_token=SRGfJyQJcKnwjoL8nv6PJ7cqBql4YvWS3lHwUhQCh1rsqajyr8or9mZdOL35euTD0zno3C26eqOy-KCvqA7mNWU40oh5m-GZ8E0a9_scs5CMm34jrPRXPtVs5XMogzWHPSCxTDcSrJupw1auq87mKHgW7LJXvDOhMS4QS9UFtsqWr0mf_h73Vp_XS1x4feP6FULMFuQaYdz-vsm5cxdG0qt-47Q3cjSqDByRGU_azZ0pcpj8ohtc8Ix5327fuSTWCg1wDUd4bcGAf6SxSJcF4OGmpOkPJG",
                         new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(
                           new
                           {
                               recipient = new
                               {
                                   user_id = item.Value
                               },
                               message = new
                               {
                                   text = message
                               }
                           }),
                           Encoding.UTF8, "application/json")
                       ).Result;
                        var customerJsonString = response.Content.ReadAsStringAsync();
                    } 
                }
                
            }
            catch (Exception ex)
            {

            }

        }
    }
}