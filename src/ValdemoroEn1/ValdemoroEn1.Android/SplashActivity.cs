using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;

namespace ValdemoroEn1.Droid
{
    [Activity(
          Theme = "@style/splashscreen",
          NoHistory = true,
          MainLauncher = true,
          ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            StartActivity(new Intent(this, typeof(MainActivity)));
            Finish();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

    }
}