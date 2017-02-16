using AndroidMultitap.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AndroidMultitap.Views
{
    public partial class PageB : ContentPage
    {
        public PageB()
        {
            InitializeComponent();
            BindingContext = new PageBViewModel(this);
        }
    }
}
