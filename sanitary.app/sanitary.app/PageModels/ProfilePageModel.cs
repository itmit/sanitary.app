using FreshMvvm;
using PropertyChanged;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using System.Linq;
using sanitary.app.Models;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class ProfilePageModel : FreshBasePageModel
    {
        public User CurrentUser { get; private set; }
        public string UserEmail { get; private set; }

        public ProfilePageModel()
        {

        }

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            if (IsThereInternet() == false)
            {
                return;
            }

            GetUserInfo();
        }

        private void GetUserInfo()
        {
            Realms.Realm realm = Realms.Realm.GetInstance();
            CurrentUser = realm.All<User>().Last();

            UserEmail = CurrentUser.Email;
        }

        private bool IsThereInternet()
        {
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }
    }
}
