using Android.App;
using Android.OS;
using System.Threading;

namespace sanitary.app.Droid
{
	[Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Thread.Sleep(1000);
			StartActivity(typeof(MainActivity));
		}
	}
}
