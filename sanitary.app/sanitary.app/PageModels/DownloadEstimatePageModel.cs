using FreshMvvm;
using PropertyChanged;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using sanitary.app.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Input;
using sanitary.app.Services;
using Xamarin.Forms;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class DownloadEstimatePageModel : FreshBasePageModel
    {
        private IObjectStorageService _objectStorage;
        private IDownloader downloader = DependencyService.Get<IDownloader>();

        Models.Object Object;

        public int ItemsSummary { get; private set; }
        public ObservableCollection<Material> ObjectMaterials { get; set; } = new ObservableCollection<Material>();

        public ICommand DownloadPDFCommand
        {
            get
            {
                return new Xamarin.Forms.Command((param) =>
                {
                    DownloadPDFAsync();
                });
            }
        }

        public bool IsRefreshing { get; set; } = false;

        public ICommand UpdateListCommand
        {
            get
            {
                return new Xamarin.Forms.Command(async () =>
                {
                    IsRefreshing = true;

                    await CreateListsAsync();

                    IsRefreshing = false;
                });
            }
        }

        public DownloadEstimatePageModel(Services.IObjectStorageService objectStorage)
        {
            _objectStorage = objectStorage;
            downloader.OnFileDownloaded += OnFileDownloaded;
        }

        protected async override void ViewIsAppearing(object sender, System.EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            CreateListsAsync();
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            Object = (Models.Object)initData;
            CurrentPage.Title = Object.Name;
        }

        async Task CreateListsAsync()
        {
            List<Material> materials = await _objectStorage.GetObjectMaterialsAsync(Object.uuid);

            ItemsSummary = materials.Sum(p => p.TotalAmount);

            ObjectMaterials = new ObservableCollection<Material>(materials);
        }

        private async void DownloadPDFAsync()
        {
            var filePath = await _objectStorage.GetEstimatePdfUrlAsync(Object.uuid);
            downloader.DownloadFile(string.Format(Constants.StorageDomainUrl, filePath), "Santeh Downloads");
        }

        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                CoreMethods.DisplayAlert("Выполнено", "Файл успешно сохранен", "Закрыть");
            }
            else
            {
                CoreMethods.DisplayAlert("Не выполнено", "Ошибка при сохранении файла", "Закрыть");
            }
        }
    }
}
