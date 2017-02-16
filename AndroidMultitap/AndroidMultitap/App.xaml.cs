using AndroidMultitap.Views;
using Xamarin.Forms;

namespace AndroidMultitap
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new PageA());
        }
    }
}
