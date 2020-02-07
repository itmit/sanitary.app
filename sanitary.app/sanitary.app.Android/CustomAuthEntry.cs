using Android.Content;
using static Android.Graphics.Color;
using sanitary.app.Controls;
using sanitary.app.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AuthEntry), typeof(CustomAuthEntry), new []{ typeof(VisualMarker.MaterialVisual) })]
namespace sanitary.app.Droid
{
    public class CustomAuthEntry : MaterialEntryRenderer
    {
		public CustomAuthEntry(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);
			Control?.SetBackgroundColor(Transparent);
		}
	}
}