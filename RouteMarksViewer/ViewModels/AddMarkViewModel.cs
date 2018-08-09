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


    public class AddMarkViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public Models.Mark CurrentMark { get; set; }
        public Models.Route CurrentRoute { get; set; }
        public Views.AddMarkView CurrentAddMarkView { get; set; }

        #region Commands
        RelayCommand okCommand;
        RelayCommand cancelCommand;
        #endregion

        public AddMarkViewModel(Models.Mark mark)
        {
            CurrentMark = mark;
        }
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

        public void OkCommandMethod()
        {
            if (CurrentMark.Id == 0 || !CurrentMark.Equals(CurrentDataBase.Marks.Find(CurrentMark.Id)))
            {
                string message = "";
                if (String.IsNullOrEmpty(CurrentMark.MarkSerial))
                {
                    message += "Введите идентификатор метки!\n";
                }
                if (message == "")
                {
                    CurrentMark.AddingDate = CurrentMark.AddingDate == 0 ?
                        (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds : CurrentMark.AddingDate;
                    CurrentMark.RouteId = CurrentRoute.Id;

                    if (CurrentMark.Id == 0)
                    {
                        CurrentMark.Route = null;
                    }

                    CurrentAddMarkView.DialogResult = true;
                }
                else
                    MessageBox.Show(message);
            }
            else
                CurrentAddMarkView.DialogResult = false;
        }

        public void CancelCommandMethod()
        {
            CurrentAddMarkView.DialogResult = false;
        }
    }
}
