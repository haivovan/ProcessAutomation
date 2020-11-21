using ProcessAutomation.Main.Ultility;
using System;
using RestSharp;
using System.Collections.Generic;
using ProcessAutomation.DAL;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using System.Linq;

namespace ProcessAutomation.Main.Services
{
    public class OTPService
    {
        
        public void CrawlingNewOTP()
        {
            var otps = new List<OTPModel>();
            Uri baseUrl = new Uri(Constant.GET_OTP_URL);
            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest("get", Method.GET);
            IRestResponse response = client.Execute(request);

            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = response.Content;
                    if (!string.IsNullOrEmpty(content))
                    {
                        otps = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OTPModel>>(content);
                        if (otps != null || otps.Count > 0)
                        {
                            MongoDatabase<AdminSetting> adminSetting = new MongoDatabase<AdminSetting>(typeof(AdminSetting).Name);
                            var listOTPs = adminSetting.Query.Where(x => x.Name.ToLower() == Constant.OTP).ToList();

                            foreach (var newOTP in otps)
                            {
                                var otp = listOTPs.Where(x => x.Key.ToLower() == newOTP.WebId.ToLower()).FirstOrDefault();
                                if (otp != null && otp.Value != newOTP.OTP)
                                {
                                    var updateOption = Builders<AdminSetting>.Update
                                    .Set(p => p.Value, newOTP.OTP.Trim());
                                    adminSetting.UpdateOne(x => x.Id == otp.Id, updateOption);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
