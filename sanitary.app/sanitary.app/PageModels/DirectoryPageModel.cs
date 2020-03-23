using PropertyChanged;
using sanitary.app.Models;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using sanitary.app.Services;
using System.Windows.Input;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class DirectoryPageModel : FreshBasePageModel
    {

        IDirectoryStorageService _directoryStorage;
        Directory _selectedDirectory;
        bool isInitialized;
        private bool DirectoriesUpdateStopped;

        public ICommand SelectedItemCommand
        {
            get
            {
                return new Xamarin.Forms.Command((param) =>
                {
                    Directory directory = (Directory)param;
                    OpenPage(directory);
                });
            }
        }

        public ObservableCollection<Directory> Directories { get; set; } = new ObservableCollection<Directory>();

        private List<Directory> AllDirectories { get; set; } = new List<Directory>();

        public Directory SelectedDirectory
        {
            get { return _selectedDirectory; }
            set
            {
                _selectedDirectory = value;
                if (value != null)
                    OpenPage(value);
            }
        }

        public DirectoryPageModel(IDirectoryStorageService directoryStorage)
        {
            _directoryStorage = directoryStorage;
            isInitialized = true;
        }

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            if (IsThereInternet() == false)
            {
                DirectoriesUpdateStopped = true;
                return;
            }

            CreateListsAsync();

            //Plugin.FirebasePushNotification.CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            //{
            //    CreateListsAsync();
            //};
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);

            DirectoriesUpdateStopped = true;
        }

        async Task CreateListsAsync()
        {
            var updatedList = await _directoryStorage.GetAllDirectoriesAsync();

            if (AllDirectories.SequenceEqual(updatedList) == false)
            {
                AllDirectories = await _directoryStorage.GetAllDirectoriesAsync();
                Directories = new ObservableCollection<Directory>(AllDirectories.ToList());
            }
        }

        private bool IsThereInternet()
        {
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }

        async void OpenPage(Directory selectedDirectory)
        {
            await CoreMethods.PushPageModel<ListSubcategoriesPageModel>(selectedDirectory);
        }
    }
}
