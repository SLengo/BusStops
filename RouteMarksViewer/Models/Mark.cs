using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;
using Newtonsoft.Json;

namespace RouteMarksViewer.Models
{
    public class Mark : INotifyPropertyChanged, IEquatable<Mark>
    {
        private string mark_serial;
        private string name;
        private int adding_date;
        private Nullable<int> deleting_date;
        private int is_deleted;
        private int mark_number;
        private int route_id;
        private int ethalon_time;
        private int ethalon_window;

        [SQLite.PrimaryKey]
        [SQLite.AutoIncrement]
        [SQLite.Unique]
        [SQLite.NotNull]
        public int Id { get; set; }

        public string MarkSerial
        {
            get { return mark_serial; }
            set
            {
                mark_serial = value;
                OnPropertyChanged("MarkSerial");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public int AddingDate
        {
            get { return adding_date; }
            set
            {
                adding_date = value;
                OnPropertyChanged("AddingDate");
            }
        }
        public Nullable<int> DeletingDate
        {
            get { return deleting_date; }
            set
            {
                deleting_date = value;
                OnPropertyChanged("DeletingDate");
            }
        }
        public int IsDeleted
        {
            get { return is_deleted; }
            set
            {
                is_deleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }
        public int MarkNumber
        {
            get { return mark_number; }
            set
            {
                mark_number = value;
                OnPropertyChanged("MarkNumber");
            }
        }
        [ForeignKey(typeof(Models.Route))]
        public int RouteId
        {
            get { return route_id; }
            set
            {
                route_id = value;
                OnPropertyChanged("RouteId");
            }
        }
        public int EthalonTime
        {
            get { return ethalon_time; }
            set
            {
                ethalon_time = value;
                OnPropertyChanged("EthalonTime");
            }
        }
        public int EthalonWindow
        {
            get { return ethalon_window; }
            set
            {
                ethalon_window = value;
                OnPropertyChanged("EthalonWindow");
            }
        }

        [JsonIgnore]
        [ManyToOne]
        public Models.Route Route { get; set; }


        public bool Equals(Mark other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id) &&
           (
               object.ReferenceEquals(this.MarkSerial, other.MarkSerial) ||
               this.MarkSerial != null &&
               this.MarkSerial.Equals(other.MarkSerial)
           ) &&
           (
               object.ReferenceEquals(this.Name, other.Name) ||
               this.Name != null &&
               this.Name.Equals(other.Name)
           ) &&
           (
            this.AddingDate.Equals(other.AddingDate)
           ) &&
           (
               object.ReferenceEquals(this.DeletingDate, other.DeletingDate) ||
               this.DeletingDate != null &&
               this.DeletingDate.Equals(other.DeletingDate)
           ) &&
           (
           this.IsDeleted.Equals(other.IsDeleted)
           ) &&
           (
           this.MarkNumber.Equals(other.MarkNumber)
           ) &&
           (
           this.RouteId.Equals(other.RouteId)
           ) &&
           (
           this.EthalonTime.Equals(other.EthalonTime)
           ) &&
           (
           this.EthalonWindow.Equals(other.EthalonWindow)
           );

        }

        public static Models.Mark GetCopyOfMark(Models.Mark entry_to_copy)
        {
            return new Models.Mark()
            {
                Id = entry_to_copy.Id,
                AddingDate = entry_to_copy.AddingDate,
                DeletingDate = entry_to_copy.DeletingDate,
                EthalonTime = entry_to_copy.EthalonTime,
                EthalonWindow = entry_to_copy.EthalonWindow,
                IsDeleted = entry_to_copy.IsDeleted,
                MarkNumber = entry_to_copy.MarkNumber,
                MarkSerial = entry_to_copy.MarkSerial,
                Name = entry_to_copy.Name,
                Route = entry_to_copy.Route,
                RouteId = entry_to_copy.RouteId,
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
