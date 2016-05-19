using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMC.SPaaS.Utility
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public static class GlobalConstants
    {
        public static class CloudProviders
        {
            public class Azure
            {
                public const string Name = "Azure";

                public class ConfigurationKeys
                {
                    public const string TenantId = "AzureClientTenantId";
                    public const string ClientId = "AzureClientID";
                    public const string ClientSecret = "AzureClientSecret";
                    public const string Resource = "AzureClientResource";
                    public const string SubscriptionId = "AzureSubscriptionId";
                    public const string StorageAccountName = "AzureStorageAccountName";
                }
            }
        }
    }
}
