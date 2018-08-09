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
    public class LogsViewModel : BaseViewModel
    {
        Nullable<DateTime> selectedDateTime;
        Nullable<int> selectedUserId;
        Nullable<int> selectedLogTypeId;

        #region Commands
        RelayCommand applyLogsReport;
        #endregion

        public LogsViewModel()
        {
            CurrentDataBase.LogTypes.Load();
            LogTypes = CurrentDataBase.LogTypes.Local.ToBindingList();
        }

        public Nullable<DateTime> SelectedDateTime
        {
            get { return selectedDateTime; }
            set
            {
                selectedDateTime = value;
                OnPropertyChanged("SelectedDateTime");
            }
        }
        public Nullable<int> SelectedUserId
        {
            get { return selectedUserId; }
            set
            {
                selectedUserId = value;
                OnPropertyChanged("SelectedUserId");
            }
        }
        public Nullable<int> SelectedLogTypeId
        {
            get { return selectedLogTypeId; }
            set
            {
                selectedLogTypeId = value;
                OnPropertyChanged("SelectedLogTypeId");
            }
        }

        public Views.LogsView CurrentLogsViewWindow { get; set; }

        public RelayCommand ApplyLogsReport
        {
            get
            {
                return applyLogsReport ??
                   (applyLogsReport = new RelayCommand((o) =>
                   {
                       ApplyLogsReportMethod();
                   }
                   ));
            }
        }

        private async void ApplyLogsReportMethod()
        {
            await Task.Run(() =>
            {
                Nullable<int> selected_date = null;
                if (selectedDateTime != null)
                {
                    DateTime sel_date = (DateTime)selectedDateTime;
                    selected_date = (Int32)(sel_date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                }
                if (Logs == null)
                    CurrentDataBase.Logs.Load();

                Logs = CurrentDataBase.Logs.Local.ToBindingList().Where(
                    o => o.UserId == (SelectedUserId == null ? o.UserId : selectedUserId) &&
                    o.AddingDate <= (selected_date == null ? o.AddingDate : selected_date + 86400) &&
                    o.AddingDate >= (selected_date == null ? o.AddingDate : selected_date) &&
                    o.LogTypeId == (SelectedLogTypeId == null ? o.LogTypeId : SelectedLogTypeId)
                    );
            });
        }
    }
}
