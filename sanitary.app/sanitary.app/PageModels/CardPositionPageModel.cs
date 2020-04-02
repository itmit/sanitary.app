using PropertyChanged;
using sanitary.app.Models;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sanitary.app.Services;
using System.Windows.Input;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class CardPositionPageModel : FreshBasePageModel
    {
        readonly IDirectoryStorageService _directoryStorage;
        private readonly IObjectStorageService _objectStorage;
        private Directory Directory;
        private Position CurrentPosition;
        private Models.Object _selectedObject;

        public List<Node> ObjectNodes { get; private set; }
        public List<Models.Object> UserObjects { get; private set; }

        public Models.Object SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;
                if (value != null)
                    GetObjectNodesAsync();
            }
        }
        public Node SelectedNode { get; set; }

        public bool IsNodesVisible { get; set; } = false;
        public string PostionName { get; private set; }
        public string PostionImage { get; private set; }

        public ICommand AddMaterialCommand
        {
            get
            {
                return new Xamarin.Forms.Command(AddMaterialToNodeAsync);
            }
        }

        public string UserDescription { get; set; }
        public int PriceEntry { get; set; }
        public int QuantityEntry { get; set; } = 1;

        private async void AddMaterialToNodeAsync()
        {
            Material CreatedMaterial = new Material
            {
                uuid = CurrentPosition.uuid,
                Quantity = QuantityEntry,
                Price = PriceEntry,
                Description = UserDescription,
            };

            if(SelectedObject == null)
            {
                await CoreMethods.DisplayAlert("Внимание", "Не выбран объект для добавления материала", "Ok");
                return;
            }

            if (SelectedNode == null)
            {
                await CoreMethods.DisplayAlert("Внимание", "Не выбран узел для добавления материала", "Ok");
                return;
            }

            var result = await _objectStorage.AddMaterialToNode(CreatedMaterial, SelectedNode.uuid);

            if(result == true)
            {
                await CoreMethods.DisplayAlert("Выполнено", "Материал успешно добавлен", "Ok");
                await CoreMethods.PopToRoot(true);
            }
            else
            {
                await CoreMethods.DisplayAlert("Не выполнено", "Произошла ошибка при добавлении материала", "Ok");
            }
        }

        public CardPositionPageModel(IDirectoryStorageService directoryStorage, IObjectStorageService objectStorage)
        {
            _directoryStorage = directoryStorage;
            _objectStorage = objectStorage;
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

            await GetPositionInfoAsync();
            await GetUserObjectsAsync();
        }

        private async Task GetPositionInfoAsync()
        {
            CurrentPosition = await _directoryStorage.GetSinglePositionAsync(Directory.uuid);
            PostionName = CurrentPosition.Title;
            PostionImage = CurrentPosition.ImagePath;
        }

        private async Task GetUserObjectsAsync()
        {
            UserObjects = await _objectStorage.GetUserObjectsAsync();
        }

        private async Task GetObjectNodesAsync()
        {
            ObjectNodes = await _objectStorage.GetObjectNodesAsync(SelectedObject.uuid);
            IsNodesVisible = true;
        }

        private bool IsThereInternet()
        {
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }
    }
}
