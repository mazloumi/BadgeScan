using System;

namespace BadgeScan
{
    public static class Settings
    {
        public static string Authority = "https://login.windows.net/common/oauth2/authorize";
        public static string Resource = "https://YOURSUBDOMAIN.dynamics.com";
        public static string ApplicationId = "YOURID";
        public static string SearchAttribute = "firstname";
    }
}
