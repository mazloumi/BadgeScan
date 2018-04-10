using System;
using System.Diagnostics;
using UIKit;

namespace BadgeScan.iOS
{
    public class Application
    {
        static void Main(string[] args)
        {
            try
            {
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}: {ex.StackTrace}");
            }

        }
    }
}
