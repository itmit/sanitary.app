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
using Xamarin.Forms;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class DirectoryPageModel : FreshBasePageModel
    {

        IDirectoryStorageService _directoryStorage;
        Directory _selectedDirectory;

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

        private ICommand _searchCommand;

        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>((text) =>
                {
                    SearchCatalogAsync(text);
                }));
            }
        }

        public ObservableCollection<Directory> Directories { get; set; } = new ObservableCollection<Directory>();

        public List<Directory> AllDirectories { get; set; } = new List<Directory>();

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

        public bool IsSearchActivated { get; private set; }

        public DirectoryPageModel(IDirectoryStorageService directoryStorage)
        {
            _directoryStorage = directoryStorage;
        }

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            if (IsThereInternet() == false)
            {
                return;
            }

            CreateListsAsync();

            //Plugin.FirebasePushNotification.CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            //{
            //    CreateListsAsync();
            //};
        }

        private async Task CreateListsAsync()
        {
            var updatedList = await _directoryStorage.GetAllDirectoriesAsync();

            if (AllDirectories.SequenceEqual(updatedList) == false)
            {
                AllDirectories = updatedList;
                Directories = new ObservableCollection<Directory>(AllDirectories.ToList());
            }
        }

        private async void SearchCatalogAsync(string searchText)
        {
            if (searchText.Length >= 1)
            {
                IsSearchActivated = true;
                Directories.Clear();
                var updatedList = await _directoryStorage.SearchDirectoriesAsync(searchText);
                Directories = new ObservableCollection<Directory>(updatedList.ToList());
            }
            else
            {
                IsSearchActivated = false;
                Directories.Clear();
                await CreateListsAsync();
            }
        }

        private bool IsThereInternet()
        {
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }

        private async void OpenPage(Directory selectedDirectory)
        {
            if (IsSearchActivated)
            {
                await CoreMethods.PushPageModel<ListPositionsPageModel>(selectedDirectory);
            }
            else
            {
                await CoreMethods.PushPageModel<ListSubcategoriesPageModel>(selectedDirectory);
            }
        }
    }
}
