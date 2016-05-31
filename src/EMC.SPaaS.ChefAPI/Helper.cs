using MixLibAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EMC.SPaaS.ChefAPI
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    internal class Helper
    {
        private string PrivateKey { get; set; }
        private string UserName { get; set; }

        public Helper()
        {
            //TODO:MAKE CONFIGURABLE
            PrivateKey = System.IO.File.ReadAllText(@"C:\SPaaS\chef-repo\.chef\ravikiranvs.pem");
            UserName = "ravikiranvs";
        }

        public HttpRequestMessage AddHeaders(HttpRequestMessage request)
        {
            request.SignWithMixLibAuthentication("ravikiranvs", PrivateKey);
            if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
            {
                request.Headers.Add("Content-Type", "application/json");
            }
            request.Headers.Remove("X-Ops-Sign");
            request.Headers.Add("X-Ops-Sign", "algorithm=sha1;version=1.0");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("X-Chef-Version", "11.4.0");
            request.Headers.Add("Host", "api.opscode.com:443");

            return request;
        }

        public async Task<string> SendAsync(HttpRequestMessage request)
        {
            HttpClient client = new HttpClient();

            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
