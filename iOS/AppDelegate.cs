using Foundation;
using Lottie.Forms.iOS.Renderers;
using UIKit;

namespace BadgeScan.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            LoadApplication(new App());

            AnimationViewRenderer.Init();

            return base.FinishedLaunching(app, options);
        }
    }
}
