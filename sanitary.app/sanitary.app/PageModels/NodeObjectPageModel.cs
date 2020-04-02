using FreshMvvm;
using PropertyChanged;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using sanitary.app.Models;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class NodeObjectPageModel : FreshBasePageModel
    {
        private Services.IObjectStorageService _objectStorage;

        private Models.Object CurrentObject;
        private List<Models.Object> UserObjects = new List<Models.Object>();

        public ObservableCollection<Grouping<Node, Material>> ObjectNodes { get; set; } = new ObservableCollection<Grouping<Node, Material>>();

        public bool IsRefreshing { get; set; } = false;

        public NodeObjectPageModel(Services.IObjectStorageService objectStorage)
        {
            _objectStorage = objectStorage;
        }

        public ICommand ActivateCopyNodeCommand
        {
            get
            {
                return new Xamarin.Forms.Command((param) =>
                {
                    string nodeUuid = (string)param;
                    ActivateCopyNodeAsync(nodeUuid);
                });
            }
        }

        public ICommand OpenEstimateCommand
        {
            get
            {
                return new Xamarin.Forms.Command((param) =>
                {
                    OpenEstimate();
                });
            }
        }
        
        public ICommand OpenObjectEditCommand
        {
            get
            {
                return new Xamarin.Forms.Command((param) =>
                {
                    OpenObjectEdit();
                });
            }
        }

        public ICommand OpenCatalogCommand
        {
            get
            {
                return new Xamarin.Forms.Command(() =>
                {
                    OpenCatalog();
                });
            }
        }

        public ICommand OpenNodePageCommand
        {
            get
            {
                return new Xamarin.Forms.Command((param) =>
                {
                    Node selectedNode = (Node)param;
                    OpenNodePage(selectedNode);
                });
            }
        }

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

        protected async override void ViewIsAppearing(object sender, System.EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            CreateListsAsync();
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            CurrentObject = (Models.Object)initData;
            CurrentPage.Title = CurrentObject.Name;
        }

        private async Task CreateListsAsync()
        {
            List<Node> updatedList = await GetObjectNodesAsync();

            ObjectNodes = new ObservableCollection<Grouping<Node, Material>>();

            foreach (var item in updatedList)
            {
                var group = new Grouping<Node, Material>(item, item.Materials);

                ObjectNodes.Add(group);
            }
        }

        private async Task<List<Node>> GetObjectNodesAsync()
        {
            List<Node> Nodes = await _objectStorage.GetObjectNodesAsync(CurrentObject.uuid);
            return Nodes;
        }

        private async void OpenEstimate()
        {
            await CoreMethods.PushPageModel<DownloadEstimatePageModel>(CurrentObject, true);
        }

        private async void OpenObjectEdit()
        {
            await CoreMethods.PushPageModel<ObjectPlusPageModel>(CurrentObject);
        }
        
        private async void OpenCatalog()
        {
            await CoreMethods.SwitchSelectedTab<DirectoryPageModel>();
        }

        private async void OpenNodePage(Node selectedNode)
        {
            await CoreMethods.PushPageModel<NodePageModel>(selectedNode);
        }

        private async void ActivateCopyNodeAsync(string nodeUuid)
        {
            if (UserObjects == null | UserObjects.Count <= 0)
            {
                UserObjects = await GetUserObjectsAsync();
            }

            string[] ObjectNamesArray = UserObjects.Select(obj => obj.Name).ToArray();
            string selectedObjectName = await CoreMethods.DisplayActionSheet("Выберите объект", "Cancel", null, ObjectNamesArray.ToArray());

            if(selectedObjectName == "Cancel" | selectedObjectName == null)
            {
                return;
            }

            Models.Object selectedObject = UserObjects.Where(obj => obj.Name == selectedObjectName).First();

            _objectStorage.SendCopyNodeRequestAsync(nodeUuid, selectedObject.uuid);

            await CoreMethods.DisplayAlert("Выполнено", "Узел успешно скопирован", "Ok");
        }

        private async Task<List<Models.Object>> GetUserObjectsAsync()
        {
            List<Models.Object> Objects = await _objectStorage.GetUserObjectsAsync();
            return Objects;
        }
    }
}