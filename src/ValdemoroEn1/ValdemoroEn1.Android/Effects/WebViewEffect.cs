using System;
using System.ComponentModel;
using ValdemoroEn1.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("ValdemoroEn1")]
[assembly: ExportEffect(typeof(WebViewEffect), "WebViewEffect")]
namespace ValdemoroEn1.Droid.Effects
{
    public class WebViewEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var webview = Control as Android.Webkit.WebView;

                webview.SetBackgroundColor(Android.Graphics.Color.Transparent);
                webview.Settings.BuiltInZoomControls = true;
                webview.Settings.DisplayZoomControls = false;
                webview.Settings.LoadWithOverviewMode = true;
                webview.Settings.UseWideViewPort = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: {0}", ex.Message);
            }
        }

        protected override void OnDetached() { }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
        }
    }
}