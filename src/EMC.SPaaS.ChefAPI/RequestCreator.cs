using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EMC.SPaaS.ChefAPI
{
    public enum Endpoint { Node };

    internal class RequestCreator
    {
        

        public HttpRequestMessage Create(Endpoint endpoint, HttpMethod method, string content = null)
        {
            string endpointUri = "https://api.opscode.com/organizations/emc-dwp";
            switch (endpoint)
            {
                case Endpoint.Node:
                    endpointUri += "/nodes";
                    break;
            }

            HttpRequestMessage request = new HttpRequestMessage(method, endpointUri);

            if (string.IsNullOrWhiteSpace(content))
            {
                request.Content = new StringContent(content);
            }

            var helper = new Helper();
            return helper.AddHeaders(request);
        }
    }
}
