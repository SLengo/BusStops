using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows.Data;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RouteMarksViewer.ViewModels
{
    public class AddSchedulerViewModel : BaseViewModel
    {
        public Views.AddSchedulerView CurrentAddSchedulerWindow { get; set; }
        ObservableCollection<Models.RouteScheduler> currentRouteSchedulerCollection { get; set; }
        public AddSchedulerViewModel(ObservableCollection< Models.RouteScheduler > routeScheduler)
        {
            CurrentRouteSchedulerCollection = routeScheduler;
        }

        public ObservableCollection<Models.RouteScheduler> CurrentRouteSchedulerCollection
        {
            get { return currentRouteSchedulerCollection; }
            set
            {
                currentRouteSchedulerCollection = value;
                OnPropertyChanged("CurrentRouteScheduler");
            }
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
            if (CurrentRouteSchedulerCollection[0].Id == 0 || !CurrentRouteSchedulerCollection[0].Equals(
                CurrentDataBase.RouteSchedulers.Find(CurrentRouteSchedulerCollection[0].Id)))
            {

                if(currentRouteSchedulerCollection[0].DayOfWeek == 0)
                {
                    MessageBox.Show("Необходимо выбрать хотябы один день недели!", "Добавление расписания", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

                
                if(CurrentRouteSchedulerCollection[0].AddingDate == 0)
                {
                    Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    CurrentRouteSchedulerCollection[0].AddingDate = unixTimestamp;
                }

                if (CurrentRouteSchedulerCollection[0].DayOfWeek.ToString().Length > 1) // select more that one day
                {
                    for (int i = 1; i < CurrentRouteSchedulerCollection[0].DayOfWeek.ToString().Length; i++)
                    {
                        Models.RouteScheduler routeScheduler = Models.RouteScheduler.GetCopyOfRouteScheduler(CurrentRouteSchedulerCollection[0]);
                        if (routeScheduler.Id != 0) routeScheduler.Id = 0;
                        string day_of_week = CurrentRouteSchedulerCollection[0].DayOfWeek.ToString()[i].ToString();
                        routeScheduler.DayOfWeek = Convert.ToInt32( day_of_week );
                        CurrentRouteSchedulerCollection.Add(routeScheduler);
                    }
                    CurrentRouteSchedulerCollection[0].DayOfWeek = Convert.ToInt32(
                        CurrentRouteSchedulerCollection[0].DayOfWeek.ToString()[0].ToString()
                        );
                }
                CurrentAddSchedulerWindow.DialogResult = true;
            }
            else
                CurrentAddSchedulerWindow.DialogResult = false;

        }
        private void CancelCommandMethod()
        {
            CurrentAddSchedulerWindow.DialogResult = false;
        }
    }
}
