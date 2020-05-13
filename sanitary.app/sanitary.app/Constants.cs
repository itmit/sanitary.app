using Xamarin.Forms;

namespace sanitary.app
{
    public static class Constants
    {
        // URL of REST service
        public static string RestUrl = "https://santeh.app/api/{0}";
        public static string ImageDomainUrl = "https://santeh.app{0}";
        public static string StorageDomainUrl = "https://santeh.app/{0}";

        public static string AppId
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        return "";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// These Ids are test Ids from https://developers.google.com/admob/android/test-ads
        /// </summary>
        /// <value>The banner identifier.</value>
        public static string BannerId
        {

            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        return "ca-app-pub-3940256099942544/6300978111";
                    default:
                        return "ca-app-pub-3940256099942544/6300978111";
                }
            }
        }
    }
}
