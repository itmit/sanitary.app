using PropertyChanged;
using sanitary.app.Models;
using FreshMvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using sanitary.app.Services;
using System.Windows.Input;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class ObjectPageModel : FreshBasePageModel
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

        public ICommand DeleteObjectCommand
        {
            get
            {
                return new Xamarin.Forms.Command((param) =>
                {
                    Object selectedObjectToDelete = (Object)param;
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () => 
                    {
                        var confirmResponse = await CoreMethods.DisplayAlert("Внимание", "Вы действительно хотите удалить объект: " + selectedObjectToDelete.Name, "Да", "Нет");
                        if (confirmResponse)
                        {
                            DeleteUserObjectsAsync(selectedObjectToDelete);
                        }
                    });
                });
            }
        }

        public ObjectPageModel(IObjectStorageService objectStorage)
        {
            _objectStorage = objectStorage;
        }

        protected async override void ViewIsAppearing(object sender, System.EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            CreateListsAsync();
        }

        private async Task CreateListsAsync()
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

        private async void DeleteUserObjectsAsync(Models.Object objectToDelete)
        {
            var response = await _objectStorage.DeleteUserObjectsAsync(objectToDelete);
            if (response == true)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    await CoreMethods.DisplayAlert("Выполнено", "Объект удален", "Ок");
                    await CreateListsAsync();
                });
            }
        }

        async void OpenPage(Object selectedObject)
        {
            await CoreMethods.PushPageModel<NodeObjectPageModel>(selectedObject);
        }
    }
}
