
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace BadgeScan.Droid
{
    [Activity(Label = "BadgeScan", Icon = "@drawable/icon", Theme = "@style/SplashTheme", MainLauncher = true)]
    [IntentFilter(new string[] { Intent.ActionView }, Categories = new string[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "file", DataHost = "*", DataMimeType = "*/*", DataPathPattern = ".*\\\\.badgescan")]
    [IntentFilter(new string[] { Intent.ActionView }, Categories = new string[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "http", DataHost = "*", DataMimeType = "*/*", DataPathPattern = ".*\\\\.badgescan")]
    [IntentFilter(new string[] { Intent.ActionView }, Categories = new string[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "content", DataHost = "*", DataMimeType = "application/badgescan")]
    [IntentFilter(new string[] { Intent.ActionView }, Categories = new string[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "content", DataHost = "*", DataMimeType = "application/octet-stream")]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.AddFlags(ActivityFlags.SingleTop);
            StartActivity(intent);

            if (Intent.ActionView.Equals(Intent.Action) && !string.IsNullOrEmpty(Intent.Type))
            {
                System.Console.WriteLine($"File {Intent.Data.Path}");

                try
                {
                    ContentResolver.OpenInputStream(Intent.Data);

                    var attachment = ContentResolver.OpenInputStream(Intent.Data);
                    if (attachment == null)
                    {
                        Extensions.LoadFile(Intent.Data.Path);

                    }
                    else
                    {
                        var tmp = File.CreateTempFile("tmp", "badgescan");
                        var stream = new System.IO.MemoryStream();
                        attachment.CopyTo(stream);
                        System.IO.File.WriteAllBytes(tmp.Path, stream.ToArray());
                        attachment.Close();
                        Extensions.LoadFile(tmp.Path);
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"SplashActivity.OnCreate Error {e.StackTrace}");
                }
            }
            Finish();
        }
    }
}
