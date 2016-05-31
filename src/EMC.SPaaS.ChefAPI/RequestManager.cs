using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EMC.SPaaS.ChefAPI
{
    public static class RequestManager
    {
        private static readonly RequestCreator RequestFactory;
        private static readonly Helper RequestHelper;

        static RequestManager()
        {
            RequestFactory = new RequestCreator();
            RequestHelper = new Helper();
        }

        public static async Task<string> GET(Endpoint endpoint)
        {
            var request = RequestFactory.Create(endpoint, HttpMethod.Get);
            return await RequestHelper.SendAsync(RequestHelper.AddHeaders(request));
        }

        public static async Task<string> PUT(Endpoint endpoint, string content)
        {
            var request = RequestFactory.Create(endpoint, HttpMethod.Put, content);
            return await RequestHelper.SendAsync(RequestHelper.AddHeaders(request));
        }

        public static async Task<string> POST(Endpoint endpoint, string content)
        {
            var request = RequestFactory.Create(endpoint, HttpMethod.Post, content);
            return await RequestHelper.SendAsync(RequestHelper.AddHeaders(request));
        }

        public static async Task<string> DELETE(Endpoint endpoint, string content)
        {
            var request = RequestFactory.Create(endpoint, HttpMethod.Delete);
            return await RequestHelper.SendAsync(RequestHelper.AddHeaders(request));
        }
    }
}
