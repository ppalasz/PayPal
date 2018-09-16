using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PayPal.Business.BLL
{
    public static class Functions
    {

        public static string GetMsg(Exception e)
        {
            string msg = e.Message;
            msg += e.Source;
            msg += e.StackTrace;
            if (e.InnerException != null)
            {
                msg += "\n------------------------\n";
                msg += GetMsg(e.InnerException);
            }

            return msg;
        }

        public static Exception GetException(Exception e)
        {
            return (e.GetBaseException());
        }

        public static IConfiguration GetConfig()
        {
            string appsettings = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            var builder = new ConfigurationBuilder();

            builder.AddJsonFile(appsettings,
                optional: true,
                reloadOnChange: true);

            return builder.Build();
        }
    }
}
