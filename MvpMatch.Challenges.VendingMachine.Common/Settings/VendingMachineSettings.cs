using MvpMatch.Challenges.VendingMachine.Common.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace MvpMatch.Challenges.VendingMachine.Common.Settings
{
    public class VendingMachineSettings
    {
        public static string ConnectionStringName
            => ConfigurationUtils.GetAppSetting(nameof(ConnectionStringName));

        public static string ConnectionString
            => ConfigurationUtils.GetConnectionString(ConnectionStringName);

        public static string SecretKey
            => ConfigurationUtils.GetConnectionString(nameof(SecretKey));
    }
}
