using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading;

namespace AndroidMultitap.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Properties

        string _title;        
        public string Title 
        {
            get { return _title; }
            set 
            { 
                _title = value; 
                OnPropertyChanged ();
            }
        }
        
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

        bool _canNavigate = true;
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

        public Func<object,bool> AllowNavigation => ShouldAllowNavigation;

        bool ShouldAllowNavigation (object arg)
        {
            // Add logic to determine if the UI should be disabled 
            // while executing a task that is observing this action.
            return CanNavigate;
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

        protected async Task RunTask(Task task, bool handleException = true, bool lockNavigation = true, CancellationTokenSource token = default(CancellationTokenSource), [CallerMemberName] string caller = "")
        {
            if (token != null && token.IsCancellationRequested)
                return;
            
            Exception exception = null;

            try
            {
                UpdateIsBusy (true, lockNavigation);

                await task;

                UpdateIsBusy (false);

            } catch (TaskCanceledException) {
                Debug.WriteLine ($"{caller} Task Cancelled");
            } catch (AggregateException e) {
                var ex = e.InnerException;
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                exception = ex;
            } catch (Exception ex) {
                exception = ex;
            }

            if(exception != null)
            {
                if (handleException)
                    await CurrentPage.DisplayAlert("Error", (exception.InnerException??exception).Message, "OK");
                else
                    throw exception;

                UpdateIsBusy (false);
            }
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            PropertyChanged = null;
        }

        #endregion

        #region Private Methods

        void UpdateIsBusy(bool isBusy, bool lockNavigation = true)
        {
            IsBusy = isBusy;
            CanNavigate = !lockNavigation;
        }

        #endregion
    }
}
