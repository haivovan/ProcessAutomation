using ProcessAutomation.Main.Ultility;
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
using System.Globalization;
using RestSharp;

namespace ProcessAutomation.Main.Services
{
    public class AccountService
    {
        public RegisterAccountModel GetNewAccount()
        {
            var registerModel = new RegisterAccountModel();
            Uri baseUrl = new Uri(Constant.GET_NEW_ACCOUNT_URL);
            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest("get", Method.GET);
            IRestResponse response = client.Execute(request);

            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = response.Content;
                    if (content == "0")
                        return new RegisterAccountModel();

                    registerModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RegisterAccountModel>(content);
                    if (registerModel == null || registerModel.Id == 0)
                        return new RegisterAccountModel();
                }
            }
            catch
            {
                return new RegisterAccountModel();
            }
           
            return registerModel;
        }
        public string UpdateAccountStatus(int id)
        {
            Uri baseUrl = new Uri(Constant.UPDATE_ACCOUNT_URL);
            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest("get", Method.GET);
            request.AddParameter("id", id);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return string.Empty;
            }
            return "Lỗi server khi update status account";
        }

    }
}
