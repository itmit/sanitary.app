using FreshMvvm;
using PropertyChanged;
using sanitary.app.Models;
using sanitary.app.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class ObjectPlusPageModel : FreshBasePageModel
    {
        readonly IObjectStorageService _objectStorage;

        public bool IsEditMode { get; set; } = false;

        public Models.Object CreatedObject { get; set; }

        public string ObjectName { get; set; }

        public string SaveButtonText { get; set; } = "Создать объект";

        public ObservableCollection<Grouping<Node, Material>> ObjectNodes { get; set; } = new ObservableCollection<Grouping<Node, Material>>();

        public Command AddNodeCommand
        {
            get
            {
                return new Command(AddNode);
            }
        }

        public Command CreateObjectCommand
        {
            get
            {
                return new Command(CreateObjectAsync);
            }
        }

        public Command OpenCatalogCommand
        {
            get
            {
                return new Command(OpenCatalogAsync);
            }

        }

        public Command DeleteNodeCommand
        {
            get
            {
                return new Command((param) => 
                {
                    var NodeToDelete = (Grouping<Node, Material>)param;
                    DeleteNodeAsync(NodeToDelete);
                });
            }

        }

        private async void OpenCatalogAsync()
        {
            await CoreMethods.PushPageModel<DirectoryPageModel>();
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            Models.Object passedObject = (Models.Object)initData;

            if(passedObject != null)
            {
                IsEditMode = true;
                CreatedObject = passedObject;
                CurrentPage.Title = CreatedObject.Name;
                SaveButtonText = "Cохранить";
                InitializeObjectToEditAsync();
            }
            else
            {
                IsEditMode = false;
            }
        }

        public ObjectPlusPageModel(IObjectStorageService objectStorage)
        {
            _objectStorage = objectStorage;
            CreatedObject = new Models.Object
            {
                Nodes = new List<Node>()
            };
            ObjectNodes = new ObservableCollection<Grouping<Node, Material>>();
        }

        public void AddNode()
        {
            var newNode = new Node
            {
                Name = "",
                Materials = new List<Material>()
            };

            var group = new Grouping<Node, Material>(newNode, newNode.Materials);
            ObjectNodes.Insert(0, group);
        }

        private async void DeleteNodeAsync(Grouping<Node, Material> nodeToDelete)
        {
            if (IsEditMode)
            {
                var response = await _objectStorage.DeleteNodeAsync(nodeToDelete.GroupKey.uuid);
                if (response == true)
                {
                    ObjectNodes.Remove(nodeToDelete);
                }
            }
            else
            {
                ObjectNodes.Remove(nodeToDelete);
            }
        }

        private async System.Threading.Tasks.Task InitializeObjectToEditAsync()
        {
            Models.Object objectInfo = await _objectStorage.GetObjectFullInfo(CreatedObject.uuid);

            ObjectName = objectInfo.Name;

            foreach (var item in objectInfo.Nodes)
            {
                var group = new Grouping<Node, Material>(item, item.Materials);

                ObjectNodes.Add(group);
            }
        }

        private async void CreateObjectAsync()
        {
            if (CreatedObject == null)
            {
                CreatedObject = new Models.Object();
            }

            CreatedObject.Name = ObjectName;

            var updatedNodeList = new List<Node>();

            foreach (var item in ObjectNodes)
            {
                updatedNodeList.Add(item.GroupKey);
            }

            CreatedObject.Nodes = updatedNodeList;

            if (IsEditMode)
            {
                await UpdateObjectAsync();
            }
            else
            {
                await SendNewObjectAsync();
            }
        }

        private void CleanPage()
        {
            ObjectName = String.Empty;
            CreatedObject = new Models.Object();
            ObjectNodes?.Clear();
        }

        private async System.Threading.Tasks.Task SendNewObjectAsync()
        {
            var response = await _objectStorage.SendNewObjectAsync(CreatedObject);

            if(response == true)
            {
                CleanPage();
                await CoreMethods.SwitchSelectedTab<ObjectPageModel>();
            }
        }

        private async System.Threading.Tasks.Task UpdateObjectAsync()
        {
            var response = await _objectStorage.UpdateObjectAsync(CreatedObject);

            if (response == true)
            {
                await CoreMethods.PopPageModel();
            }
        }
    }
}
