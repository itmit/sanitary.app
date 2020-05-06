using FreshMvvm;
using PropertyChanged;
using System.Windows.Input;
using System.Threading.Tasks;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainPageModel : FreshBasePageModel
    {
        public bool IsUserHaveFullAccess
        {
            get
            {
                return App.IsUserHaveFullAccess;
            }
        }
        public MainPageModel()
        {

        }

        public ICommand MenuTappedCommand
        {
            get
            {
                return new FreshAwaitCommand(async (param, tcs) =>
                {
                    var parameter = (string)param;
                    await OnMenuTapped(parameter);

                    tcs.SetResult(true);
                });
            }
        }

        async Task OnMenuTapped(string parameter)
        {
            switch (parameter)
            {
                case "Profile":
                    await CoreMethods.PushPageModel<ProfilePageModel>();
                    break;
                case "Catalog":
                    await CoreMethods.SwitchSelectedTab<DirectoryPageModel>();
                    break;
                case "Estimates":
                    await CoreMethods.SwitchSelectedTab<EstimatesPageModel>();
                    break;
                case "Object":
                    await CoreMethods.SwitchSelectedTab<ObjectPageModel>();
                    break;
                case "Add object":
                    await CoreMethods.SwitchSelectedTab<ObjectPlusPageModel>();
                    break;
                case "Purchase":
                    var test = new Services.PaymentService();
                    await test.MakePurchase();
                    break;
                case "Open Telegram":
                    await Xamarin.Essentials.Launcher.OpenAsync(new System.Uri("tg://resolve?domain=santech_inform"));
                    break;
            }
            return;
        }
    }
}
