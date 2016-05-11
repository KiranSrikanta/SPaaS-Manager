using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.Manager
{
    public static class Constants
    {

        public static class AuthenticationSession
        {
            public const string CookieKey = "Authorization";

            public const string HtmlHeader = "Authorization";

            public const string HeaderStartsWith = "bearer";

            public static class Properties
            {
                public const string UserName = "UserName";
                public const string UserId = "UserId";
                public const string Provider = "Provider";
            }
        }
    }
}
