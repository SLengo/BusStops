using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;

namespace RouteMarksViewer.ViewModels
{
    public class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {
        RelayCommand saveDataGridToE;
        RelayCommand makeLogAboutClick;
        
        IEnumerable<Models.Mark> marks;
        IEnumerable<Models.Route> routes;
        IEnumerable<Models.Map> maps;
        IEnumerable<Models.RouteScheduler> route_scheduler;
        IEnumerable<Models.UserRole> user_roles;
        IEnumerable<Models.User> users;
        IEnumerable<Models.Log> logs;
        IEnumerable<Models.LogType> log_types;

        Models.User currentUser;

        public ApplicationContext CurrentDataBase { get; set; }
        public BaseViewModel()
        {
            CurrentDataBase = new ApplicationContext();
            CurrentDataBase.Marks.Where(
                b => b.IsDeleted != 1
                ).Load();

            CurrentDataBase.Routes.Where(
                b => b.IsDeleted != 1
                ).Load();
            CurrentDataBase.Maps.Where(
                o => o.Mark.IsDeleted != 1
                ).Load();
            CurrentDataBase.RouteSchedulers.Where(
                o => o.IsDeleted != 1 && o.Route.IsDeleted != 1
                ).Load();
            CurrentDataBase.UserRoles.Where(
                o => o.Id != 1
                ).Load();
            

            Marks = CurrentDataBase.Marks.Local.ToBindingList();
            Routes = CurrentDataBase.Routes.Local.ToBindingList();
            Maps = CurrentDataBase.Maps.Local.ToBindingList();
            RouteSchedulers = CurrentDataBase.RouteSchedulers.Local.ToBindingList();
            UserRoles = CurrentDataBase.UserRoles.Local.ToBindingList();

            CurrentUser = (Models.User)Application.Current.Properties["CurrentUser"];
            if (CurrentUser.UserRoleId == 1)
            {
                CurrentDataBase.Users.Where(
                o => o.IsDeleted != 1
                ).Load();
                Users = CurrentDataBase.Users.Local.ToBindingList();
            }
            else
            {
                Users = new List<Models.User>();
            }
        }

        static BaseViewModel()
        {
        }

        public RelayCommand SaveDataGridToE
        {
            get
            {
                return saveDataGridToE ??
                   (saveDataGridToE = new RelayCommand((o) =>
                   {
                       SaveDataGridToEMethod(o as System.Windows.Controls.DataGrid);
                   }
                   ));
            }
        }
        public RelayCommand MakeLogAboutClick
        {
            get
            {
                return makeLogAboutClick ??
                   (makeLogAboutClick = new RelayCommand((logParam) =>
                   {
                       MakeLogAboutClickMethod(logParam as Models.LogViewParameter);
                   }
                   ));
            }
        }


        public Models.User CurrentUser
        {
            get
            {
                return currentUser;
            }
            set
            {
                currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        public IEnumerable<Models.Mark> Marks
        {
            get { return marks; }
            set
            {
                marks = value;
                OnPropertyChanged("Marks");
            }
        }
        public IEnumerable<Models.Route> Routes
        {
            get { return routes; }
            set
            {
                routes = value;
                OnPropertyChanged("Routes");
            }
        }
        public IEnumerable<Models.Map> Maps
        {
            get { return maps; }
            set
            {
                maps = value;
                OnPropertyChanged("Maps");
            }
        }
        public IEnumerable<Models.RouteScheduler> RouteSchedulers
        {
            get { return route_scheduler; }
            set
            {
                route_scheduler = value;
                OnPropertyChanged("RouteSchedulers");
            }
        }
        public IEnumerable<Models.UserRole> UserRoles
        {
            get { return user_roles; }
            set
            {
                user_roles = value;
                OnPropertyChanged("UserRoles");
            }
        }
        public IEnumerable<Models.User> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }
        
        public IEnumerable<Models.Log> Logs
        {
            get { return logs; }
            set
            {
                logs = value;
                OnPropertyChanged("Logs");
            }
        }
        public IEnumerable<Models.LogType> LogTypes
        {
            get { return log_types; }
            set
            {
                log_types = value;
                OnPropertyChanged("LogTypes");
            }
        }

        public void AddEntry<T>(T SelectedItem)
        {
            try
            {
                if(typeof(T) != typeof(Models.Log))
                    MakeLogEntry(5, null, SelectedItem, "adding " + typeof(T).ToString());

                CurrentDataBase.GetDbSet<T>().Add(SelectedItem);
                CurrentDataBase.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void EditEntry<T>(T SelectedItem)
        {
            try
            {
                var original = CurrentDataBase.GetDbSet<T>().Find((SelectedItem as dynamic).Id);
                if (original != null)
                {
                    MakeLogEntry(6, original, SelectedItem, "editing " + typeof(T).ToString());

                    CurrentDataBase.Entry(original).CurrentValues.SetValues(SelectedItem);
                    CurrentDataBase.SaveChanges();
                }
            }
            catch (Exception e)
            {
                if (e is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                {
                    MessageBox.Show("Editing entry must contain \"Id\" field!");
                }
                else
                    MessageBox.Show(e.Message);
            }
        }
        public MessageBoxResult DeleteEntry<T>(T SelectedItem)
        {
            if (SelectedItem == null) return MessageBoxResult.No;
            try
            {
                System.Type type = typeof(T);
                string name_of_deleting_entry = (SelectedItem as dynamic).Name;
                if (type == typeof(Models.RouteScheduler))
                {
                    DataConvertors.DayOfWeekConvertor dayOfWeekConvertor = new DataConvertors.DayOfWeekConvertor();
                    DateTime epoch = new DateTime(1970, 1, 1);
                    string time_scheduler = epoch.AddSeconds(System.Convert.ToDouble((SelectedItem as Models.RouteScheduler).StartTime)).ToLongTimeString();

                    name_of_deleting_entry = (string)dayOfWeekConvertor.Convert(
                        (SelectedItem as Models.RouteScheduler).DayOfWeek, null, null, null) + " " + time_scheduler;
                }

                var original = CurrentDataBase.GetDbSet<T>().Find((SelectedItem as dynamic).Id);
                if (original != null)
                {
                    MessageBoxResult answer = MessageBox.Show(@"Вы действительно хотите удалить """ + name_of_deleting_entry + @"""?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (answer == MessageBoxResult.Yes)
                    {
                        try
                        {
                            original.DeletingDate = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                            original.IsDeleted = 1;
                            MakeLogEntry(7, original, SelectedItem, "deleting " + typeof(T).ToString());
                            CurrentDataBase.Entry(original).CurrentValues.SetValues(original);
                            CurrentDataBase.SaveChanges();
                            CurrentDataBase.Entry(original).State = EntityState.Detached;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                            return MessageBoxResult.No;
                        }
                    }
                    else
                    {
                        return MessageBoxResult.No;
                    }
                }
            }
            catch (Exception e)
            {
                if (e is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                {
                    MessageBox.Show("Deleting entry must contain \"Id\", \"DeletingDate\", \"IsDeleted\", \"Name\" fields!");
                }
                else
                    MessageBox.Show(e.Message);
                return MessageBoxResult.No;
            }
            return MessageBoxResult.Yes;
        }

        public void SaveDataGridToEMethod(System.Windows.Controls.DataGrid _dataGrid)
        {
            SaveFileDialog _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.Filter = "Comma separated values files (*.csv) | *.csv";
            if (_saveFileDialog.ShowDialog() == true)
            {
                _dataGrid.SelectionMode = System.Windows.Controls.DataGridSelectionMode.Extended;
                _dataGrid.SelectAllCells();
                _dataGrid.ClipboardCopyMode = System.Windows.Controls.DataGridClipboardCopyMode.IncludeHeader;
                System.Windows.Input.ApplicationCommands.Copy.Execute(null, _dataGrid);
                String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                _dataGrid.UnselectAllCells();
                System.IO.StreamWriter file1 = new System.IO.StreamWriter(_saveFileDialog.FileName, false, Encoding.UTF8);
                file1.WriteLine(result);
                file1.Close();
                _dataGrid.SelectionMode = System.Windows.Controls.DataGridSelectionMode.Single;
                MessageBox.Show("Файл сохранен!");
            }
        }

        #region logging methods
        public void MakeLogAboutClickMethod(Models.LogViewParameter LogParam)
        {
            MakeLogEntry(LogParam.LogTypeId, null, null, LogParam.Action);
        }
        public void MakeLogEntry(int type_id, object old_value, object new_value, string action)
        {
            try
            {
                Models.Log _log = new Models.Log();
                _log.UserId = CurrentUser.Id;
                _log.LogTypeId = type_id;
                _log.AddingDate = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (old_value != null)
                {
                    _log.OldValue = Newtonsoft.Json.JsonConvert.SerializeObject(old_value);
                }
                if (new_value != null)
                {
                    _log.NewValue = Newtonsoft.Json.JsonConvert.SerializeObject(new_value);
                }
                if (!String.IsNullOrEmpty(action))
                {
                    _log.Action = action;
                }
                AddEntry<Models.Log>(_log);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion


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
