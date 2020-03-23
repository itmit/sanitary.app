using System;
using Xamarin.Forms;
using System.IO;
using System.Net;
using System.ComponentModel;
using sanitary.app.Services;
using sanitary.app.Droid.Services;
using Android;
using Android.Support.V4.Content;
using Android.Content.PM;
using Android.App;
using Android.Support.V4.App;

[assembly: Dependency(typeof(AndroidDownloader))]
namespace sanitary.app.Droid.Services
{
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;
        const int PERMISSIONS_STORAGE = 111;

        public void DownloadFile(string url, string folder)
        {
            //string pathToNewFolder = Path.Combine(Android.OS.Environment.DirectoryDownloads, folder);
            var IsPermissionGranted = CheckPermission();

            if(IsPermissionGranted == false)
            {
                RequestPermission();
                return;
            }
            //Directory.CreateDirectory(pathToNewFolder);

            try
            {
                Android.Net.Uri contentUri = Android.Net.Uri.Parse(url);
                DownloadManager.Request currentRequest = new DownloadManager.Request(contentUri);
                currentRequest.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, Path.GetFileName(url));
                currentRequest.AllowScanningByMediaScanner();
                currentRequest.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
                DownloadManager dm = (DownloadManager)Forms.Context.GetSystemService(Android.Content.Context.DownloadService);

                dm.Enqueue(currentRequest);
            }
            catch (Exception)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }

            //try
            //{
            //    WebClient webClient = new WebClient();
            //    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            //    string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
            //    webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
            //}
            //catch (Exception)
            //{
            //    if (OnFileDownloaded != null)
            //        OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            //}
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