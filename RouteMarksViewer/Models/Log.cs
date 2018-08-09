using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;

namespace RouteMarksViewer.Models
{
    public class Log : INotifyPropertyChanged
    {
        private int adding_date;
        private int user_id;
        private int log_type_id;
        private string old_value;
        private string new_value;
        private string action;

        [SQLite.PrimaryKey]
        [SQLite.AutoIncrement]
        [SQLite.Unique]
        [SQLite.NotNull]
        public int Id { get; set; }

        public int AddingDate
        {
            get { return adding_date; }
            set
            {
                adding_date = value;
                OnPropertyChanged("AddingDate");
            }
        }

        [ForeignKey(typeof(Models.User))]
        public int UserId
        {
            get { return user_id; }
            set
            {
                user_id = value;
                OnPropertyChanged("UserId");
            }
        }

        [ForeignKey(typeof(Models.LogType))]
        public int LogTypeId
        {
            get { return log_type_id; }
            set
            {
                log_type_id = value;
                OnPropertyChanged("LogTypeId");
            }
        }
        public string OldValue
        {
            get { return old_value; }
            set
            {
                old_value = value;
                OnPropertyChanged("OldValue");
            }
        }
        public string NewValue
        {
            get { return new_value; }
            set
            {
                new_value = value;
                OnPropertyChanged("NewValue");
            }
        }
        public string Action
        {
            get { return action; }
            set
            {
                action = value;
                OnPropertyChanged("Action");
            }
        }

        [ManyToOne]
        public Models.LogType LogType { get; set; }
        [ManyToOne]
        public Models.User User { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
