using System;
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
                Console.WriteLine($"Error {ex.Message}: {ex.StackTrace}");
            }

        }
    }
}
