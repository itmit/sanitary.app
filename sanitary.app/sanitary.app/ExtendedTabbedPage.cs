using FreshMvvm;

namespace sanitary.app
{
    public class ExtendedTabbedPage : FreshTabbedNavigationContainer
    {
        public ExtendedTabbedPage(string navigationServiceName) : base(navigationServiceName)
        {
        }

        public void NotifyTabReselected()
        {
            CurrentPage.Navigation.PopToRootAsync();
        }
    }
}
