using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using FFImageLoading;
using Prism;
using Prism.Ioc;
using System;

namespace ValdemoroEn1.Droid
{
    [Activity(Label = "ValdemoroEn1", Icon = "@mipmap/icon", Theme = "@style/MainTheme",
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Acr.UserDialogs.UserDialogs.Init(this);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            Plugin.GoogleClient.GoogleClientManager.Initialize(this);
            LoadApplication(new App(new AndroidInitializer()));
        }

        /// FFImageLoading image service preserves in heap memory of the device every image newly downloaded 
        /// from url. In order to avoid application crash, you should reclaim memory in low memory situations.
        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            ImageService.Instance.InvalidateMemoryCache();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            base.OnTrimMemory(level);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Plugin.GoogleClient.GoogleClientManager.OnAuthCompleted(requestCode, resultCode, data);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

