using System;
using Xamarin.Forms;
using System.ComponentModel;
using sanitary.app.Services;
using sanitary.app.Droid.Services;
using Android;
using Android.Support.V4.Content;
using Android.Content.PM;
using Android.App;
using Android.Support.V4.App;
using static Android.App.DownloadManager;

[assembly: Dependency(typeof(AndroidDownloader))]
namespace sanitary.app.Droid.Services
{
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;
        const int PERMISSIONS_STORAGE = 111;

        public void DownloadFile(string url, string folder)
        {
            var IsPermissionGranted = CheckPermission();

            if(IsPermissionGranted == false)
            {
                RequestPermission();
                return;
            }

            try
            {
                string fileName = "smeta_" + DateTime.Now.ToString("dd_MM_yy") + ".pdf";

                Request request = new Request(Android.Net.Uri.Parse(url));

                // Store to common external storage:
                request.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, fileName);
                request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
                request.SetDescription(fileName);
                request.SetTitle(fileName);

                DownloadManager dm = (DownloadManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.DownloadService);
                var enqueue = dm.Enqueue(request);
            }
            catch (Exception)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
        }

        private void RequestPermission()
        {
            var thisActivity = Forms.Context as Activity;
            ActivityCompat.RequestPermissions(thisActivity, new string[] { Manifest.Permission.WriteExternalStorage }, PERMISSIONS_STORAGE);
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
            else
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
            }
        }

        private bool CheckPermission()
        {
            const string permission = Manifest.Permission.WriteExternalStorage;
            return ContextCompat.CheckSelfPermission(Android.App.Application.Context, permission) == (int)Permission.Granted;
        }
    }
}