using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeDrive.Helper
{
    public class AppSettingsHelper : ControllerBase
    {
        public static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public static string GetValueFromAppConfig(string key)
        {
            var settings = GetConfig();

            return settings["MySettings:" + key];
        }
    }

}
