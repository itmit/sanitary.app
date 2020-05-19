using FreshMvvm;
using PropertyChanged;
using System.Windows.Input;
using System.Threading.Tasks;
using sanitary.app.Models;
using System.Linq;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainPageModel : FreshBasePageModel
    {
        public bool UserDoesNotHaveFullAccess { get; set; }
        
        public MainPageModel()
        {

        }

        protected override void ViewIsAppearing(object sender, System.EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            Realms.Realm realm = Realms.Realm.GetInstance();
            IQueryable<User> users = realm.All<User>();

            if (users.Count() > 0)
            {
                User CurrentUser = realm.All<User>().Last();

                UserDoesNotHaveFullAccess = CurrentUser.IsUserHaveFullAccess == false;
                App.IsUserHaveFullAccess = CurrentUser.IsUserHaveFullAccess;
            }
            else
            {
                UserDoesNotHaveFullAccess = App.IsUserHaveFullAccess == false;
            }
            
            //UserDoesNotHaveFullAccess = App.IsUserHaveFullAccess == false;
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
