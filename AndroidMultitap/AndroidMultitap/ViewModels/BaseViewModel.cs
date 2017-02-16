using AndroidMultitap.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AndroidMultitap.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Properties

        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        bool _canNavigate;
        public bool CanNavigate
        {
            get { return _canNavigate; }
            set
            {
                _canNavigate = value;
                OnPropertyChanged();
            }
        }

        Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region TaskRunner

        protected async Task RunTask(Task task, bool notifyException = false, [CallerMemberName] string caller = "")
        {
            Exception exception = null;

            try
            {
                await TaskRunner.RunSafe(task);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if(exception != null)
            {
                if (notifyException)
                    await CurrentPage.DisplayAlert("Error", (exception.InnerException??exception).Message, "OK");
                else
                    throw exception;
            }
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            PropertyChanged = null;
        }

        #endregion
    }
}
