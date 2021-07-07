using System;
using UIKit;
using ValdemoroEn1.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("ValdemoroEn1")]
[assembly: ExportEffect(typeof(WebViewEffect), "WebViewEffect")]

namespace ValdemoroEn1.iOS.Effects
{
    public class WebViewEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var webView = Control as UIWebView;

                webView.Opaque = false;
                webView.BackgroundColor = Color.Transparent.ToUIColor();
                webView.ScrollView.ScrollEnabled = true;
                webView.ScalesPageToFit = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: {0} ", ex.Message);
            }
        }

        protected override void OnDetached() { }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
        }
    }
}