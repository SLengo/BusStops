using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RouteMarksViewer.ViewModels
{
    public class AddRouteViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public Models.Route CurrentRoute { get; set; }
        public Views.AddRouteView CurrentAddRouteView { get; set; }

        public AddRouteViewModel(Models.Route route)
        {
            CurrentRoute = route;
        }

        #region Commands
        RelayCommand okCommand;
        RelayCommand cancelCommand;
        #endregion

        public RelayCommand OkCommand
        {
            get
            {
                return okCommand ??
                   (okCommand = new RelayCommand((o) =>
                   {
                       OkCommandMethod();
                   }
                   ));
            }
        }

        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                   (cancelCommand = new RelayCommand((o) =>
                   {
                       CancelCommandMethod();
                   }
                   ));
            }
        }

        private void OkCommandMethod()
        {
            if (CurrentRoute.Id == 0 || !CurrentRoute.Equals(CurrentDataBase.Routes.Find(CurrentRoute.Id)))
            {
                if (!String.IsNullOrEmpty(CurrentRoute.Name))
                {
                    Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    CurrentRoute.AddingDate = CurrentRoute.AddingDate == 0 ? unixTimestamp : CurrentRoute.AddingDate;
                    CurrentAddRouteView.DialogResult = true;
                }
                else
                    MessageBox.Show("Введите имя маршрута!");
            }
            else
                CurrentAddRouteView.DialogResult = false;

        }
        private void CancelCommandMethod()
        {
            CurrentAddRouteView.DialogResult = false;
        }

        #region INotifyPropertyChanged requirements
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
