using PropertyChanged;
using sanitary.app.Models;
using FreshMvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using sanitary.app.Services;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class EstimatesPageModel : FreshBasePageModel
    {
        Object _selectedObject;
        private IObjectStorageService _objectStorage;

        public ObservableCollection<Object> UserObjects { get; set; } = new ObservableCollection<Object>();

        private List<Object> AllObjects { get; set; } = new List<Object>();

        public Object SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;
                if (value != null)
                    OpenPage(value);
            }
        }

        public bool IsRefreshing { get; set; } = false;

        public System.Windows.Input.ICommand UpdateListCommand
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

        public EstimatesPageModel(IObjectStorageService objectStorage)
        {
            _objectStorage = objectStorage;
        }

        protected async override void ViewIsAppearing(object sender, System.EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            CreateListsAsync();
        }

        async Task CreateListsAsync()
        {
            List<Object> updatedList = await GetUserObjectsAsync();
            UserObjects = new ObservableCollection<Object>(updatedList);

            //if (AllObjects.SequenceEqual(updatedList) == false)
            //{
            //    UserObjects = new ObservableCollection<Object>(AllObjects.ToList());
            //}
        }

        private async Task<List<Object>> GetUserObjectsAsync()
        {
            List<Object> Objects = await _objectStorage.GetUserObjectsAsync();
            return Objects;
        }

        async void OpenPage(Object selectedObject)
        {
            await CoreMethods.PushPageModel<DownloadEstimatePageModel>(selectedObject);
        }
    }
}
