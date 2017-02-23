using AndroidMultitap.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AndroidMultitap.ViewModels
{
    public class PageAViewModel : BaseViewModel
    {
        public PageAViewModel(Page page)
        {
            Title = "Page A";
            CurrentPage = page;
        }

        #region Commands

        ICommand _showAlertCommand;
        public ICommand ShowAlertCommand
        {
            get {
                if (_showAlertCommand == null)
                    _showAlertCommand = new Command (OnShowAlert, AllowNavigation);
                
                return _showAlertCommand;
            }
        }

        async void OnShowAlert (object obj)
        {
            await RunTask(CurrentPage.DisplayAlert ("Hello", "From intelliAbb", "OK"));
        }

        ICommand _goToPageBCommand;
        public ICommand GoToPageBCommand
        {
            get
            {
                if (_goToPageBCommand == null)
                    _goToPageBCommand = new Command(OnGotoPageB, AllowNavigation);

                return _goToPageBCommand;
            }
        }

        async void OnGotoPageB(object obj)
        {
            await RunTask(NavigateToPage(new PageB()));
        }

        async Task NavigateToPage(Page page)
        {
            await CurrentPage.Navigation.PushAsync(page);
        }
        #endregion
    }

}
