using System;
using System.Diagnostics;
using Foundation;
using Lottie.Forms.iOS.Renderers;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xfx;

namespace BadgeScan.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            XfxControls.Init();

            global::Xamarin.Forms.Forms.Init();

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            LoadApplication(new App());

            AnimationViewRenderer.Init();

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            Debug.WriteLine($"AppDelegate.OpenUrl {url.Path}");
            Extensions.LoadFile(url.Path);
            return true;
        }
    }
}
