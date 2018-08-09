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
using System.Windows.Threading;

namespace RouteMarksViewer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        #region Commands
        RelayCommand addRouteCommand;
        RelayCommand editRouteCommand;
        RelayCommand deleteRouteCommand;

        RelayCommand openMarkWindowCommand;
        RelayCommand openSchedulerWindowCommand;
        RelayCommand openUsersManagementCommand;
        RelayCommand openLogsViewCommand;
        #endregion

        public Views.StartWindow CurrentWorkWindow { get; set; }

        public MainViewModel()
        {
            MakeLogEntry(3, null, null, "user \"" + CurrentUser.Login + "\" login");
        }

        public RelayCommand AddRouteCommand
        {
            get
            {
                return addRouteCommand ??
                   (addRouteCommand = new RelayCommand((o) =>
                   {
                       AddRouteMethod();
                   }
                   ));
            }
        }
        public RelayCommand EditRouteCommand
        {
            get
            {
                return editRouteCommand ??
                   (editRouteCommand = new RelayCommand((SelectedItem) =>
                   {
                       EditRouteMethod(SelectedItem as Models.Route);
                   }
                   ));
            }
        }
        public RelayCommand DeleteRouteCommand
        {
            get
            {
                return deleteRouteCommand ??
                   (deleteRouteCommand = new RelayCommand((SelectedItem) =>
                   {
                       DeleteRouteMethod(SelectedItem as Models.Route);
                   }
                   ));
            }
        }

        public RelayCommand OpenMarkWindowCommand
        {
            get
            {
                return openMarkWindowCommand ??
                   (openMarkWindowCommand = new RelayCommand((SelectedItem) =>
                   {
                       OpenMarkWindowMethod(SelectedItem as Models.Route);
                   }
                   ));
            }
        }

        public RelayCommand OpenSchedulerWindowCommand
        {
            get
            {
                return openSchedulerWindowCommand ??
                   (openSchedulerWindowCommand = new RelayCommand((SelectedItem) =>
                   {
                       OpenSchedulerWindowMethod(SelectedItem as Models.Route);
                   }
                   ));
            }
        }
        public RelayCommand OpenUsersManagementCommand
        {
            get
            {
                return openUsersManagementCommand ??
                   (openUsersManagementCommand = new RelayCommand((o) =>
                   {
                       OpenUsersManagementMethod();
                   }
                   ));
            }
        }
        public RelayCommand OpenLogsViewCommand
        {
            get
            {
                return openLogsViewCommand ??
                   (openLogsViewCommand = new RelayCommand((o) =>
                   {
                       OpenLogsViewMethod();
                   }
                   ));
            }
        }

        private void OpenLogsViewMethod()
        {
            MakeLogEntry(8, null, null, "open LogsView");
            ViewModels.LogsViewModel logsViewModelViewModel = new LogsViewModel();
            Views.LogsView logsView = new Views.LogsView(logsViewModelViewModel);
            logsView.Owner = CurrentWorkWindow;
            logsView.ShowDialog();
        }

        private void OpenUsersManagementMethod()
        {
            MakeLogEntry(8, null, null, "open UsersManagement");
            ViewModels.UserManagementViewModel userManagementViewModel = new UserManagementViewModel();
            Views.UserManagementView userManagementView = new Views.UserManagementView(userManagementViewModel);
            userManagementView.Owner = CurrentWorkWindow;
            userManagementView.ShowDialog();
        }

        private void OpenSchedulerWindowMethod(Models.Route SelectedItem)
        {
            if (SelectedItem == null) return;
            MakeLogEntry(8, null, null, "open SchedulerWindow");
            ViewModels.SchedulerViewModel schedulerViewModel = new SchedulerViewModel(SelectedItem);
            Views.SchedulerView schedulerView = new Views.SchedulerView(schedulerViewModel);
            schedulerView.Owner = CurrentWorkWindow;
            schedulerView.ShowDialog();
        }


        private void OpenMarkWindowMethod(Models.Route SelectedItem)
        {
            if (SelectedItem == null) return;
            MakeLogEntry(8, null, null, "open MarkWindow");
            ViewModels.MarksViewViewModel marksViewViewModel = new MarksViewViewModel(SelectedItem);
            Views.MarksView marksView = new Views.MarksView(marksViewViewModel);
            marksView.Owner = CurrentWorkWindow;
            marksView.ShowDialog();
        }

        public void AddRouteMethod()
        {
            ViewModels.AddRouteViewModel addRouteViewModel = new AddRouteViewModel(new Models.Route());
            Views.AddRouteView addRouteView = new Views.AddRouteView(
                addRouteViewModel
                );
            addRouteView.Owner = CurrentWorkWindow;
            MakeLogEntry(8, null, null, "open AddRoute. Start adding route");
            if ((bool)addRouteView.ShowDialog())
            {
                Models.Route route_to_base = Models.Route.GetCopyOfRoute(addRouteViewModel.CurrentRoute);
                AddEntry<Models.Route>(route_to_base);
            }
            else
                MakeLogEntry(8, null, null, "open AddRoute. Cancel adding route");
        }
        public void EditRouteMethod(Models.Route SelectedItem)
        {
            if (SelectedItem == null) return;
            Models.Route route_to_view = Models.Route.GetCopyOfRoute(SelectedItem);
            ViewModels.AddRouteViewModel addRouteViewModel = new AddRouteViewModel(route_to_view);
            Views.AddRouteView addRouteView = new Views.AddRouteView(
                addRouteViewModel
                );
            addRouteView.Owner = CurrentWorkWindow;
            MakeLogEntry(8, null, null, "open AddRoute. Start editing route");
            if ((bool)addRouteView.ShowDialog())
            {
                Models.Route route_to_base = Models.Route.GetCopyOfRoute(addRouteViewModel.CurrentRoute);
                EditEntry<Models.Route>(route_to_base);
            }
            else
                MakeLogEntry(8, null, null, "open AddRoute. Cancel editing route");
        }
        public void DeleteRouteMethod(Models.Route SelectedItem)
        {
            MakeLogEntry(8, null, null, "open DeleteRoute. Start deleting route");
            MessageBoxResult res = DeleteEntry<Models.Route>(SelectedItem);
            if(res == MessageBoxResult.No)
                MakeLogEntry(8, null, null, "open DeleteRoute. Cancel deleting route");
        }
        
        
        
    }
}
