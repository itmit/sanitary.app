using PropertyChanged;
using sanitary.app.Models;
using FreshMvvm;
using Xamarin.Forms;
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
    public class ListSubcategoriesPageModel : FreshBasePageModel
    {

        IDirectoryStorageService _directoryStorage;
        Directory _selectedDirectory;
        Directory Directory;

        public ObservableCollection<Directory> Directories { get; set; } = new ObservableCollection<Directory>();

        private List<Directory> AllDirectories { get; set; } = new List<Directory>();

        public ListSubcategoriesPageModel(IDirectoryStorageService directoryStorage)
        {
            _directoryStorage = directoryStorage;
        }

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

        public override void Init(object initData)
        {
            base.Init(initData);
            Directory = (Directory)initData;
            CurrentPage.Title = Directory.Title;
        }

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            if (IsThereInternet() == false)
            {
                return;
            }

            CreateListsAsync();
        }

        async Task CreateListsAsync()
        {
            var updatedList = await _directoryStorage.GetSubDirectoriesAsync(Directory.uuid);

            if (AllDirectories.SequenceEqual(updatedList) == false)
            {
                AllDirectories = await _directoryStorage.GetSubDirectoriesAsync(Directory.uuid);
                Directories = new ObservableCollection<Directory>(AllDirectories.ToList());
            }
        }

        private bool IsThereInternet()
        {
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }

        async void OpenPage(Directory selectedDirectory)
        {
            await CoreMethods.PushPageModel<ListPositionsPageModel>(selectedDirectory);
        }
    }
}
