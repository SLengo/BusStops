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
    public class SchedulerViewModel : BaseViewModel
    {
        ObservableCollection<Models.RouteScheduler> currnetRouteScheduler;

        ObservableCollection<DataGridSchedulerDetail> currnetDetailRouteScheduler;

        public Views.SchedulerView CurrentSchedulerWindow { get; set; }
        public Models.Route CurrentRoute { get; set; }
        public SchedulerViewModel(Models.Route route)
        {
            CurrentRoute = route;

            CurrnetRouteScheduler = new ObservableCollection<Models.RouteScheduler>(RouteSchedulers.Where(
                o => o.IsDeleted == 0 && o.RouteId == CurrentRoute.Id
                ).OrderBy(o => o.StartTime).OrderBy(o => o.DayOfWeek));

            CurrnetDetailRouteScheduler = new ObservableCollection<DataGridSchedulerDetail>();

            foreach (Models.RouteScheduler rsch in CurrnetRouteScheduler)
            {
                AddSchedulerEntryToDetail(rsch);
            }
        }

        #region helper methods for updating UI and custom collections
        private void AddSchedulerEntryToDetail(Models.RouteScheduler rsch)
        {
            DataGridSchedulerDetail dataGridSchedulerDetail =
                    CurrnetDetailRouteScheduler.FirstOrDefault(o => o.DayOfWeek == rsch.DayOfWeek);
            if (dataGridSchedulerDetail == null)
            {
                dataGridSchedulerDetail = new DataGridSchedulerDetail();
                dataGridSchedulerDetail.DayOfWeek = rsch.DayOfWeek;
                dataGridSchedulerDetail.RouteSchedulerDetail = new ObservableCollection<Models.RouteScheduler>();
                dataGridSchedulerDetail.RouteSchedulerDetail.Add(rsch);
                CurrnetDetailRouteScheduler.Add(dataGridSchedulerDetail);
            }
            else
            {
                dataGridSchedulerDetail.RouteSchedulerDetail.Add(rsch);
            }
            dataGridSchedulerDetail.RouteSchedulerDetail = new ObservableCollection<Models.RouteScheduler>(
                dataGridSchedulerDetail.RouteSchedulerDetail.OrderBy(o => o.StartTime)
                );
            CurrnetDetailRouteScheduler = new ObservableCollection<DataGridSchedulerDetail>(CurrnetDetailRouteScheduler.OrderBy(o => o.DayOfWeek));
        }
        private void EditOdRemoveSchedulerEntryInDetail(Models.RouteScheduler rsch, int mode)
        {
            foreach (DataGridSchedulerDetail item in CurrnetDetailRouteScheduler)
            {
                if(rsch.DayOfWeek == item.DayOfWeek)
                {
                    foreach (Models.RouteScheduler innerScheduler in item.RouteSchedulerDetail)
                    {
                        if(innerScheduler.Id == rsch.Id)
                        {
                            if (mode == 1) // edit
                            {
                                item.RouteSchedulerDetail.Remove(innerScheduler);
                                item.RouteSchedulerDetail.Add(rsch);
                                break;
                            }
                            else if (mode == 2) // remove
                            {
                                item.RouteSchedulerDetail.Remove(innerScheduler);
                                break;
                            }
                        }
                    }
                }
                item.RouteSchedulerDetail = new ObservableCollection<Models.RouteScheduler>(
                                item.RouteSchedulerDetail.OrderBy(o => o.StartTime)
                );
            }
        }
        private Models.RouteScheduler FindSelectedRouteSchedulerInLB(object SelectedItem)
        {
            System.Windows.Controls.ListBox lb =
                       Helpers.UIHelper.FindChild<System.Windows.Controls.ListBox>(SelectedItem as System.Windows.Controls.DataGrid, "SchedulerLB");
            Models.RouteScheduler routeSchedulerSelected = null;
            if (lb.SelectedItem == null && lb.Items.Count > 0)
                routeSchedulerSelected = lb.Items[0] as Models.RouteScheduler;
            else
                routeSchedulerSelected = lb.SelectedItem as Models.RouteScheduler;
            return routeSchedulerSelected;
        }
        private void CurrentRouteScheduler_Changed()
        {
            CustomControls.SchedulerVisual.UpdateUI(CurrentSchedulerWindow.CurrentSchedulerVisual);
        }
        #endregion

        public ObservableCollection<DataGridSchedulerDetail> CurrnetDetailRouteScheduler
        {
            get
            {
                return currnetDetailRouteScheduler;
            }
            set
            {
                currnetDetailRouteScheduler = value;
                OnPropertyChanged("CurrnetDetailRouteScheduler");
            }
        }

        public ObservableCollection<Models.RouteScheduler> CurrnetRouteScheduler
        {
            get
            {
                return currnetRouteScheduler;
            }
            set
            {
                currnetRouteScheduler = value;
                OnPropertyChanged("CurrnetRouteScheduler");
            }
        }

        RelayCommand addSchedulerCommand;
        RelayCommand editSchedulerCommand;
        RelayCommand deleteSchedulerCommand;

        public RelayCommand AddSchedulerCommand
        {
            get
            {
                return addSchedulerCommand ??
                   (addSchedulerCommand = new RelayCommand((o) =>
                   {
                       AddSchedulerMethod();
                   }
                   ));
            }
        }
        public RelayCommand EditSchedulerCommand
        {
            get
            {
                return editSchedulerCommand ??
                   (editSchedulerCommand = new RelayCommand((SelectedItem) =>
                   {
                       EditSchedulerMethod(
                           FindSelectedRouteSchedulerInLB(SelectedItem)
                           );
                   }
                   ));
            }
        }
        public RelayCommand DeleteSchedulerCommand
        {
            get
            {
                return deleteSchedulerCommand ??
                   (deleteSchedulerCommand = new RelayCommand((SelectedItem) =>
                   {
                       DeleteSchedulerMethod(
                           FindSelectedRouteSchedulerInLB(SelectedItem)
                           );
                   }
                   ));
            }
        }

        void AddSchedulerMethod()
        {
            ObservableCollection<Models.RouteScheduler> RouteSchedulerList = new ObservableCollection<Models.RouteScheduler>();
            RouteSchedulerList.Add(new Models.RouteScheduler());
            ViewModels.AddSchedulerViewModel addSchedulerViewModel = new AddSchedulerViewModel(RouteSchedulerList);
            Views.AddSchedulerView addSchedulerView = new Views.AddSchedulerView(
                addSchedulerViewModel
                );
            addSchedulerView.Owner = CurrentSchedulerWindow;
            MakeLogEntry(8, null, null, "open AddScheduler. Start adding scheduler");
            if ((bool)addSchedulerView.ShowDialog())
            {
                foreach (Models.RouteScheduler item in addSchedulerViewModel.CurrentRouteSchedulerCollection)
                {
                    Models.RouteScheduler routeScheduler = Models.RouteScheduler.GetCopyOfRouteScheduler(item);
                    routeScheduler.RouteId = CurrentRoute.Id;
                    AddEntry<Models.RouteScheduler>(routeScheduler);
                    CurrnetRouteScheduler.Add(routeScheduler);
                    CurrnetRouteScheduler = new ObservableCollection<Models.RouteScheduler>(
                        CurrnetRouteScheduler.OrderBy(o => o.StartTime).OrderBy(o => o.DayOfWeek));
                    AddSchedulerEntryToDetail(routeScheduler);
                }
                CurrentRouteScheduler_Changed();
            }
            else
                MakeLogEntry(8, null, null, "open AddScheduler. Cancel adding scheduler");
        }
        void EditSchedulerMethod(Models.RouteScheduler SelectedItem)
        {
            if (SelectedItem == null) return;
            ObservableCollection<Models.RouteScheduler> RouteSchedulerList = new ObservableCollection<Models.RouteScheduler>();
            RouteSchedulerList.Add(
                Models.RouteScheduler.GetCopyOfRouteScheduler(SelectedItem)
                );
            ViewModels.AddSchedulerViewModel addSchedulerViewModel = new AddSchedulerViewModel(RouteSchedulerList);
            Views.AddSchedulerView addSchedulerView = new Views.AddSchedulerView(
                addSchedulerViewModel
                );
            addSchedulerView.Owner = CurrentSchedulerWindow;
            MakeLogEntry(8, null, null, "open AddScheduler. Start editing scheduler");
            if ((bool)addSchedulerView.ShowDialog())
            {
                foreach (Models.RouteScheduler item in addSchedulerViewModel.CurrentRouteSchedulerCollection)
                {
                    Models.RouteScheduler routeScheduler = Models.RouteScheduler.GetCopyOfRouteScheduler(item);
                    if (routeScheduler.Id == 0)
                    {
                        AddEntry<Models.RouteScheduler>(routeScheduler);
                        CurrnetRouteScheduler.Add(routeScheduler);
                        CurrentSchedulerWindow.CurrentSchedulerVisual.CurrentRouteScheduler = CurrnetRouteScheduler;
                        AddSchedulerEntryToDetail(routeScheduler);
                    }
                    else
                    {
                        EditEntry<Models.RouteScheduler>(routeScheduler);
                        EditOdRemoveSchedulerEntryInDetail(routeScheduler, 1);
                    }
                }
                CurrnetRouteScheduler = new ObservableCollection<Models.RouteScheduler>(
                       CurrnetRouteScheduler.OrderBy(o => o.StartTime).OrderBy(o => o.DayOfWeek));
                CurrentRouteScheduler_Changed();
            }
            else
                MakeLogEntry(8, null, null, "open AddScheduler. Cancel editing scheduler");
        }
        void DeleteSchedulerMethod(Models.RouteScheduler SelectedItem)
        {
            MakeLogEntry(8, null, null, "open AddScheduler. Start deleting scheduler");
            if (DeleteEntry<Models.RouteScheduler>(SelectedItem) == MessageBoxResult.Yes)
            {
                CurrnetRouteScheduler.Remove(SelectedItem);
                EditOdRemoveSchedulerEntryInDetail(SelectedItem, 2);
                CurrentRouteScheduler_Changed();
            }
            else
                MakeLogEntry(8, null, null, "open AddScheduler. Cancel deleting scheduler");
        }

    }



    public class DataGridSchedulerDetail : INotifyPropertyChanged
    {
        int day_of_week;
        ObservableCollection< Models.RouteScheduler > routeSchedulerDetail;

        public int DayOfWeek
        {
            get { return day_of_week; }
            set
            {
                day_of_week = value;
                OnPropertyChanged("DayOfWeek");
            }
        }
        public ObservableCollection<Models.RouteScheduler> RouteSchedulerDetail
        {
            get { return routeSchedulerDetail; }
            set
            {
                routeSchedulerDetail = value;
                OnPropertyChanged("RouteSchedulerDetail");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

}
