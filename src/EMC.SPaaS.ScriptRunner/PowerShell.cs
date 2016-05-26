using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;

namespace EMC.SPaaS.ScriptRunner
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public static class PowerShell
    {
        static PowerShell()
        {

        }

        public static void RunScriptRemotely(string userName, string password, string targetSystemIP, string command)
        {
            var securePassword = new System.Security.SecureString();
            foreach (char p in password)
                securePassword.AppendChar(p);

            string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
            PSCredential remoteCredential = new PSCredential(userName, securePassword);
            WSManConnectionInfo connectionInfo = new WSManConnectionInfo(true, targetSystemIP, 5986, "/wsman", shellUri, remoteCredential);

            using (Runspace runspace = RunspaceFactory.CreateRunspace(connectionInfo))
            {
                runspace.Open();
                Pipeline pipeline = runspace.CreatePipeline(command);
                var results = pipeline.Invoke();
                //TODO:HOW TO KNOW IF EXECUTION WAS A SUCCESS????
            }
        }
    }
}
