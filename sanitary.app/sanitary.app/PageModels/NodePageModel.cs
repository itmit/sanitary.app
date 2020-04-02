using FreshMvvm;
using PropertyChanged;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using sanitary.app.Models;
using System.Windows.Input;
using System.Linq;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class NodePageModel : FreshBasePageModel
    {
        private readonly Services.IObjectStorageService _objectStorage;
        private Node CurrentNode;
        public ObservableCollection<Material> MaterialList { get; set; }

        public NodePageModel(Services.IObjectStorageService objectStorage)
        {
            _objectStorage = objectStorage;
        }

        public ICommand DeleteMaterialCommand
        {
            get
            {
                return new Xamarin.Forms.Command(async (param) =>
                {
                    string materialUuid = (string)param;
                    await DeleteMaterialAsync(materialUuid);
                });
            }
        }

        private async Task DeleteMaterialAsync(string materialUuid)
        {
            bool result = await _objectStorage.DeleteMaterialFromNodeAsync(materialUuid);

            if(result == true)
            {
                Material itemToRemove = MaterialList.Single(r => r.uuid == materialUuid);
                MaterialList.Remove(itemToRemove);
                await CoreMethods.DisplayAlert("Выполнено", "Материал удален", "Ok");
            }
            else
            {
                await CoreMethods.DisplayAlert("Не выполнено", "Произошла ошибка при удалении материала", "Ok");
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            CurrentNode = (Node)initData;
            CurrentPage.Title = CurrentNode.Name;
            MaterialList = new ObservableCollection<Material>(CurrentNode.Materials);
        }
    }
}