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
            CurrentPage = page;
        }

        #region Commands

        ICommand _goToPageB;
        public ICommand GoToPageBCommand
        {
            get
            {
                if (_goToPageB == null)
                    _goToPageB = new Command(OnGotoPageB);

                return _goToPageB;
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
