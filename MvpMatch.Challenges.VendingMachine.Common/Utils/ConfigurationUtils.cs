using System.Configuration;

namespace MvpMatch.Challenges.VendingMachine.Common.Utils
{
    public class ConfigurationUtils
    {
        public static string GetConnectionString(string name)
            => ConfigurationManager.ConnectionStrings[name].ConnectionString;

        public static string GetAppSetting(string name)
            => ConfigurationManager.AppSettings[name];
    }
}
