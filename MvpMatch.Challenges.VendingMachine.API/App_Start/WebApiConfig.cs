using MvpMatch.Challenges.VendingMachine.API.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace MvpMatch.Challenges.VendingMachine.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ConfigRouting(config);
            ConfigFormatters(config);
            ConfigFilters(config);
        }

        #region Private Utils

        private static void ConfigRouting(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }

        private static void ConfigFilters(HttpConfiguration config)
        {
            config.Filters.Add(new VendingMachineExceptionFilter());
        }

        private static void ConfigFormatters(HttpConfiguration config)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

            config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(jsonFormatter);
        }

        #endregion
    }
}
