using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace DegreeMapping.Models
{
    public class AppSettings
    {
        public static string URLPath { get { return GetKey("URLPath"); } }

        private static string GetKey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}