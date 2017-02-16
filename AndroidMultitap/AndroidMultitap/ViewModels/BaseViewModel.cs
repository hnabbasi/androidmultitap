using AndroidMultitap.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace AndroidMultitap.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        public BaseViewModel ()
        {
            MessagingCenter.Subscribe<object, bool> (this, "IsBusyUpdated", OnIsBusyUpdated);
        }

        #region Properties

        string _title;          public string Title {             get { return _title; }             set 
            { 
                _title = value; 
                OnPropertyChanged ();
            }         } 

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

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region TaskRunner

        protected async Task RunTask(Task task, bool notifyException = true, [CallerMemberName] string caller = "")
        {
            if (!CanNavigate)
                return;

            Exception exception = null;

            try
            {
                UpdateIsBusy (true);
                await TaskRunner.RunSafe(task);
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
                if (notifyException)
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
            MessagingCenter.Unsubscribe<object, bool> (this, "IsBusyUpdated");
        }

        #endregion

        #region Private Methods

        void OnIsBusyUpdated (object sender, bool isBusy)
        {
            if (IsBusy.Equals (isBusy)) 
                return;

            Debug.WriteLine ($">>> Sender: {sender.ToString()} IsBusy -> {isBusy}");

            IsBusy = isBusy;
            CanNavigate = !isBusy;
        }

        void UpdateIsBusy(bool isBusy)
        {
            MessagingCenter.Send<object, bool> (this, "IsBusyUpdated", isBusy);
        }

        #endregion
    }
}
