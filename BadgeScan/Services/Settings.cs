using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace BadgeScan
{
    public static class Settings
    {
        public static string Authority
        {
            get
            {
                return CrossSettings.Current.GetValueOrDefault("Authority", "https://login.windows.net/common/oauth2/authorize");
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue("Authority", value);
            }
        }

        public static string Resource
        {
            get
            {
                return CrossSettings.Current.GetValueOrDefault("Resource", string.Empty);
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue("Resource", value);
            }
        }

        public static string ApplicationId
        {
            get
            {
                return CrossSettings.Current.GetValueOrDefault("ApplicationId", string.Empty);
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue("ApplicationId", value);
            }
        }

        public static string SearchAttribute
        {
            get
            {
                return CrossSettings.Current.GetValueOrDefault("SearchAttribute", string.Empty);
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue("SearchAttribute", value);
            }
        }

        public static bool UseScanner
        {
            get
            {
                return CrossSettings.Current.GetValueOrDefault("UseScanner", true);
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue("UseScanner", value);
            }
        }
    }
}