using System;
using Xamarin.Forms;

namespace AndroidMultitap.ViewModels
{
    public class PageBViewModel : BaseViewModel
    {
        public PageBViewModel(Page page)
        {
            Title = "Page B";
            CurrentPage = page;
        }

        public Color PageBackgroundColor => GetRandomColor ();

        Color GetRandomColor()
        {
            var random = new Random ();

            var r = random.Next (255);
            var g = random.Next (255);
            var b = random.Next (255);

            return Color.FromRgb (r, g, b);
        }
    }
}
