using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace RouteMarksViewer.ViewModels
{
    public class MarksViewViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public Views.MarksView CurrentMarksView { get; set; }
        public Models.Route CurrentRoute { get; set; }
        ObservableCollection<Models.Mark> currnetMarks;
        ObservableCollection<Models.Map> currentMapCollection;

        #region Commands
        RelayCommand addMarkCommand;
        RelayCommand editMarkCommand;
        RelayCommand deleteMarkCommand;
        //RelayCommand addMarkFromDeviceCommand;

        RelayCommand showMapCommand;
        RelayCommand saveMapCommand;
        #endregion

        public MarksViewViewModel(Models.Route route)
        {
            CurrentRoute = route;
            CurrnetMarks = new ObservableCollection<Models.Mark>(Marks.Where(
                o => o.IsDeleted == 0 && o.RouteId == route.Id
                ).OrderBy(o => o.MarkNumber));
            if (CurrentMapCollection == null)
            {
                CurrentMapCollection = UpdateMapCollection();
            }
        }

        public void UpdateMapVisual()
        {
            ObservableCollection<Models.Map> observableCollectionForMap = UpdateMapCollection();
            for (int i = CurrentMapCollection.Count - 1; i >= 0; i--)
            {
                //delete removed
                if (!observableCollectionForMap.Contains(CurrentMapCollection[i]))
                    CurrentMapCollection.Remove(CurrentMapCollection[i]);
            }

            foreach (Models.Map item in observableCollectionForMap)
            {
                // add
                if(!CurrentMapCollection.Contains(item))
                    CurrentMapCollection.Add(item);
            }
            CustomControls.RouteMap.UpdateUI(CurrentMarksView.CurrentRouteMap);
        }
        public ObservableCollection<Models.Map> UpdateMapCollection()
        {
            ObservableCollection<Models.Map> observableCollectionForMap =
                    new ObservableCollection<Models.Map>();
            foreach (Models.Mark mark in CurrnetMarks)
            {
                Models.Map map = Maps.FirstOrDefault(o => o.MarkId == mark.Id);
                if (map == null)
                {
                    map = new Models.Map()
                    {
                        MarkId = mark.Id,
                        CoordX = 0,
                        CoordY = 0,
                        DefaultHeight = 0,
                        DefaultWidth = 0
                    };
                }
                observableCollectionForMap.Add(map);
            }
            return observableCollectionForMap;
        }

        public ObservableCollection<Models.Mark> CurrnetMarks
        {
            get
            {
                return currnetMarks;
            }
            set
            {
                currnetMarks = value;
                OnPropertyChanged("CurrnetMarks");
            }
        }
        public ObservableCollection<Models.Map> CurrentMapCollection
        {
            get
            {
                return currentMapCollection;
            }
            set
            {
                currentMapCollection = value;
                OnPropertyChanged("CurrentMapCollection");
            }
        }

        public RelayCommand AddMarkCommand
        {
            get
            {
                return addMarkCommand ??
                   (addMarkCommand = new RelayCommand((o) =>
                   {
                       AddMarkCommandMethod();
                   }
                   ));
            }
        }
        public RelayCommand EditMarkCommand
        {
            get
            {
                return editMarkCommand ??
                   (editMarkCommand = new RelayCommand((SelectedItem) =>
                   {
                       EditMarkCommandMethod(SelectedItem as Models.Mark);
                   }
                   ));
            }
        }
        public RelayCommand DeleteMarkCommand
        {
            get
            {
                return deleteMarkCommand ??
                   (deleteMarkCommand = new RelayCommand((SelectedItem) =>
                   {
                       DeleteMarkMethod(SelectedItem as Models.Mark);
                   }
                   ));
            }
        }

        public RelayCommand ShowMapCommand
        {
            get
            {
                return showMapCommand ??
                   (showMapCommand = new RelayCommand((o) =>
                   {
                       ShowMapCommandMethod();
                   }
                   ));
            }
        }
        public RelayCommand SaveMapCommand
        {
            get
            {
                return saveMapCommand ??
                   (saveMapCommand = new RelayCommand((o) =>
                   {
                       SaveMapCommandMethod();
                   }
                   ));
            }
        }

        //public RelayCommand AddMarkFromDeviceCommand
        //{
        //    get
        //    {
        //        return addMarkFromDeviceCommand ??
        //           (addMarkFromDeviceCommand = new RelayCommand((o) =>
        //           {
        //               AddMarkFromDeviceCommandMethod();
        //           }
        //           ));
        //    }
        //}

        public void AddMarkCommandMethod()
        {
            Models.Mark mark = new Models.Mark();
            mark.MarkNumber = CurrnetMarks.Count > 0 ? CurrnetMarks.Last().MarkNumber + 1 : 1;

            ViewModels.AddMarkViewModel addMarkViewModel = new AddMarkViewModel(mark);
            addMarkViewModel.CurrentRoute = CurrentRoute;
            Views.AddMarkView addMarkView = new Views.AddMarkView(addMarkViewModel);
            MakeLogEntry(8, null, null, "open AddMark. Start adding mark");
            if ((bool)addMarkView.ShowDialog())
            {
                Models.Mark mark_to_base = Models.Mark.GetCopyOfMark(addMarkViewModel.CurrentMark);
                AddEntry<Models.Mark>(mark_to_base);
                CurrnetMarks.Add(mark_to_base);
                Models.Map map = new Models.Map()
                {
                    MarkId = mark_to_base.Id,
                    CoordX = 0,
                    CoordY = 0,
                    DefaultHeight = 0,
                    DefaultWidth = 0
                };
                AddEntry<Models.Map>(map);
                UpdateMapVisual();
            }
            else
                MakeLogEntry(8, null, null, "open AddMark. Cancel adding mark");
        }
        public void EditMarkCommandMethod(Models.Mark SelectedItem)
        {
            if (SelectedItem == null || (CurrentUser.UserRoleId != 1 && CurrentUser.UserRoleId != 2)) return;
            Models.Mark mark_to_view = Models.Mark.GetCopyOfMark(SelectedItem);
            ViewModels.AddMarkViewModel addMarkViewModel = new AddMarkViewModel(mark_to_view);
            addMarkViewModel.CurrentRoute = CurrentRoute;
            Views.AddMarkView addMarkView = new Views.AddMarkView(addMarkViewModel);
            addMarkView.Owner = CurrentMarksView;
            MakeLogEntry(8, null, null, "open AddMark. Start editing mark");
            if ((bool)addMarkView.ShowDialog())
            {
                Models.Mark mark_to_base = Models.Mark.GetCopyOfMark(addMarkViewModel.CurrentMark);
                EditEntry<Models.Mark>(mark_to_base);
                UpdateMapVisual();
            }
            else
                MakeLogEntry(8, null, null, "open AddMark. Cancel editing mark");
        }
        public void DeleteMarkMethod(Models.Mark SelectedItem)
        {
            MakeLogEntry(8, null, null, "open AddMark. Start deleting mark");
            if (DeleteEntry<Models.Mark>(SelectedItem) == MessageBoxResult.Yes)
            {
                CurrnetMarks.Remove(CurrnetMarks.FirstOrDefault(o => o.Id == SelectedItem.Id));
                UpdateMapVisual();
            }
            else
                MakeLogEntry(8, null, null, "open AddMark. Cancel deleting mark");
        }

        public void ShowMapCommandMethod()
        {
            if (CurrentMarksView.RouteMarksGridSplitter.Visibility == Visibility.Collapsed)
            {
                CurrentMarksView.RouteMarksGridSplitter.Width = 5;
                CurrentMarksView.RouteMarksGridSplitter.Visibility = Visibility.Visible;
                CurrentMarksView.CurrentRouteMap.Visibility = Visibility.Visible;
                if (CurrentMapCollection == null)
                {
                    CurrentMapCollection = UpdateMapCollection();
                }
                MakeLogEntry(8, null, null, "open Map");
            }
            else
            {
                CurrentMarksView.mgrid_col_2.Width = GridLength.Auto; 
                CurrentMarksView.RouteMarksGridSplitter.Width = Double.NaN;
                CurrentMarksView.CurrentRouteMap.Width = Double.NaN;
                CurrentMarksView.RouteMarksGridSplitter.Visibility = Visibility.Collapsed;
                CurrentMarksView.CurrentRouteMap.Visibility = Visibility.Collapsed;
                MakeLogEntry(8, null, null, "close Map");
            }
        }
        public void SaveMapCommandMethod()
        {
            if(currentMapCollection != null)
            {
                foreach (Models.Map map in currentMapCollection)
                {
                    if (Maps.FirstOrDefault(o => o.MarkId == map.MarkId) == null)
                        AddEntry<Models.Map>(map);
                    else
                        EditEntry<Models.Map>(map);
                }
                MakeLogEntry(8, null, null, "save Map");
            }
        }

      /*  public async void AddMarkFromDeviceCommandMethod()
        {
            if (!_OneWireDeviceManager.IsDeviceOnBase)
            {
                MessageBox.Show("Устройство не подключено!", "Контрольный проход", MessageBoxButton.OK, MessageBoxImage.Stop); return;
            }
            Models.Reader reader = CurrentDataBase.Readers.FirstOrDefault(o => o.ReaderSerialNumber == _OneWireDeviceManager.DeviceId && o.IsDeleted == 0);
            if (reader == null)
            {
                MessageBox.Show("Устройство не добавлено в базу данных! Сначала добавьте его на вкладке \"Считыватели\"\n(кнопка \"Добавить подключенное устройство\")!", "Контрольный проход", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            MessageBoxResult res = MessageBox.Show("Вы действительно хотите считать с устройства подготовленный контрольный проход?", "Контрольный проход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(res == MessageBoxResult.Yes)
            {
                _OneWireDeviceManager.GetAllMarkDataFromDeviceStorage();
                if (_OneWireDeviceManager.CurrentReadMarkBytes.Where(o => o.EntryType == TypeOfDeviceStorageEntry.Mark).Count() == 0)
                {
                    MessageBox.Show("На устройстве нет ни одной записанной метки!", "Контрольный проход", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

                // checking for containing marks on other routes
                string response = "";
                for (int i = 0; i < _OneWireDeviceManager.CurrentReadMarkBytes.Count; i++)
                {
                    Models.Mark mark = Marks.FirstOrDefault(o => o.MarkSerial == _OneWireDeviceManager.CurrentReadMarkBytes[i].MarkId &&
                                                            o.IsDeleted == 0);
                    if (mark != null)
                    {
                        response += "Метка - " + _OneWireDeviceManager.CurrentReadMarkBytes[i].MarkId + "; Маршрут - " + mark.Route.Name + "\n";
                    }
                }
                if(response != "")
                {
                    MessageBox.Show("Некоторые метки уже задействованы в других маршрутах!\n" + 
                        response, "Контрольный проход", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }


                int mark_num = 1;
                await Task.Run(() =>
                {
                    for (int i = 0; i < _OneWireDeviceManager.CurrentReadMarkBytes.Count; i++)
                    {
                        if (_OneWireDeviceManager.CurrentReadMarkBytes[i].EntryType == TypeOfDeviceStorageEntry.Mark)
                        {
                            Models.Mark mark_to_base = new Models.Mark();
                            mark_to_base.MarkSerial = _OneWireDeviceManager.CurrentReadMarkBytes[i].MarkId;
                            mark_to_base.AddingDate = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                            mark_to_base.RouteId = CurrentRoute.Id;
                            mark_to_base.ReaderId = reader.Id;
                            mark_to_base.Route = null;
                            mark_to_base.Reader = null;
                            mark_to_base.MarkNumber = mark_num;

                            AddEntry<Models.Mark>(mark_to_base);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                CurrnetMarks.Add(mark_to_base);
                            });
                            Models.Map map = new Models.Map()
                            {
                                MarkId = mark_to_base.Id,
                                CoordX = 0,
                                CoordY = 0,
                                DefaultHeight = 0,
                                DefaultWidth = 0
                            };
                            AddEntry<Models.Map>(map);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                UpdateMapVisual();
                            });
                            mark_num++;
                        }
                    }
                });
                MessageBox.Show("Контрольный проход добавлен! Установите временные интервалы и задайте имена меткам", "Контрольный проход", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        } */

        #region DragAndDrop DataGrid rows with marks for changing its marknumber

        public delegate Point GetPosition(IInputElement element);
        TextBlock NoteTextBlock = new TextBlock();

        Point p;
        public Point P
        {
            get
            {
                return p;
            }
            set
            {
                p = value;
                OnPropertyChanged("P");
            }
        }
        int rowIndex = -1;
        public int Row_select
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
                OnPropertyChanged("Row_select");
            }
        }
        int row_want_to = -1;
        public int Row_want_to
        {
            get
            {
                return row_want_to;
            }
            set
            {
                row_want_to = value;
                OnPropertyChanged("Row_want_to");
            }
        }

        public async void MarksDataGrid_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (CurrentUser != null && (CurrentUser.UserRoleId != 1 && CurrentUser.UserRoleId != 2)) return;
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                P = e.GetPosition(sender as System.Windows.Controls.DataGrid);
                System.Windows.Controls.DataGrid MarksDataGrid = sender as System.Windows.Controls.DataGrid;
                if (Row_select < 0)
                {
                    Row_select = GetCurrentRowIndex(e.GetPosition, MarksDataGrid);
                    if (rowIndex < 0)
                        return;
                }
                Row_want_to = GetCurrentRowIndex(e.GetPosition, MarksDataGrid);
                if (Row_select >= 0 && Row_want_to >= 0 && Row_select < CurrnetMarks.Count && Row_want_to < CurrnetMarks.Count
                    && Row_select != Row_want_to)
                {
                    System.Windows.Input.Mouse.SetCursor(System.Windows.Input.Cursors.Hand);
                    NoteTextBlock.Text = "\"" + CurrnetMarks[Row_select].Name + "\" поместить вместо \"" + CurrnetMarks[row_want_to].Name + "\"";
                    NoteTextBlock.Background = Brushes.Black;
                    NoteTextBlock.Foreground = Brushes.White;
                    NoteTextBlock.FontSize = 20;
                    NoteTextBlock.VerticalAlignment = VerticalAlignment.Top;
                    NoteTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    CurrentMarksView.MainGrid.Children.Remove(NoteTextBlock);
                    CurrentMarksView.MainGrid.Children.Add(NoteTextBlock);
                    Grid.SetRow(NoteTextBlock, 1);
                    NoteTextBlock.Margin = new Thickness(P.X, P.Y, 0, 0);
                }
                else
                {
                    CurrentMarksView.MainGrid.Children.Remove(NoteTextBlock);
                    System.Windows.Input.Mouse.SetCursor( System.Windows.Input.Cursors.No);
                }
            }
            else
            {
                System.Windows.Controls.DataGrid MarksDataGrid = sender as System.Windows.Controls.DataGrid;
                if ((Row_select < 0 || Row_want_to < 0) ||
                   (Row_want_to == Row_select) ||
                   (Row_want_to == MarksDataGrid.Items.Count))
                {
                    SetRowStatmentDefault();
                    return;
                }

                // place where need change mark number
                Models.Mark changedProduct = CurrnetMarks[Row_select];
                CurrnetMarks.RemoveAt(Row_select);
                CurrnetMarks.Insert(Row_want_to, changedProduct);
                SetRowStatmentDefault();
                await Task.Run(() => {
                    for (int i = 0; i < CurrnetMarks.Count; i++)
                    {
                        CurrnetMarks[i].MarkNumber = i + 1;
                        EditEntry<Models.Mark>(CurrnetMarks[i]);
                    }
                });
            }
        }

        public void MarksDataGrid_Drop(object sender, DragEventArgs e)
        {
            //System.Windows.Controls.DataGrid MarksDataGrid = sender as System.Windows.Controls.DataGrid;
            //if (Row_select < 0)
            //{
            //    SetRowStatmentDefault();
            //    return;
            //}
            //int index = GetCurrentRowIndex(e.GetPosition, MarksDataGrid);
            //if (index < 0)
            //{
            //    SetRowStatmentDefault();
            //    return;
            //}
            //if (index == Row_select)
            //{
            //    SetRowStatmentDefault();
            //    return;
            //}
            //if (index == MarksDataGrid.Items.Count)
            //{
            //    SetRowStatmentDefault();
            //    return;
            //}
            //// TODO  place where need change mark number
            //Models.Mark changedProduct = CurrnetMarks[Row_select];
            //CurrnetMarks.RemoveAt(Row_select);
            //CurrnetMarks.Insert(index, changedProduct);
            //for (int i = 0; i < CurrnetMarks.Count; i++)
            //{
            //    CurrnetMarks[i].MarkNumber = i + 1;
            //    EditEntry<Models.Mark>(CurrnetMarks[i]);
            //}
            //SetRowStatmentDefault();
        }
        void SetRowStatmentDefault()
        {
            Row_select = -1;
            CurrentMarksView.MainGrid.Children.Remove(NoteTextBlock);
            System.Windows.Input.Mouse.SetCursor(System.Windows.Input.Cursors.Arrow);
        }
        private bool GetMouseTargetRow(Visual theTarget, GetPosition position)
        {
            if (theTarget == null) return false;
            Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
            Point point = position((IInputElement)theTarget);
            
            return rect.Contains(point);
        }

        private System.Windows.Controls.DataGridRow GetRowItem(int index, System.Windows.Controls.DataGrid MarksDataGrid)
        {
            if (MarksDataGrid.ItemContainerGenerator.Status
                    != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                return null;
            return MarksDataGrid.ItemContainerGenerator.ContainerFromIndex(index)
                                                            as System.Windows.Controls.DataGridRow;
        }

        private int GetCurrentRowIndex(GetPosition pos, System.Windows.Controls.DataGrid MarksDataGrid)
        {
            int curIndex = -1;
            for (int i = 0; i < MarksDataGrid.Items.Count; i++)
            {
                System.Windows.Controls.DataGridRow itm = GetRowItem(i, MarksDataGrid);
                if (GetMouseTargetRow(itm, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
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
