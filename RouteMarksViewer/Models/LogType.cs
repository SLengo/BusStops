using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;

namespace RouteMarksViewer.Models
{
    public class LogType : INotifyPropertyChanged
    {
        private string type_description;

        [SQLite.PrimaryKey]
        [SQLite.AutoIncrement]
        [SQLite.Unique]
        [SQLite.NotNull]
        public int Id { get; set; }
        public string TypeDescription
        {
            get { return type_description; }
            set
            {
                type_description = value;
                OnPropertyChanged("TypeDescription");
            }
        }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Models.Log> Logs { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
