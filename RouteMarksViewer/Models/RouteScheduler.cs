using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;
using Newtonsoft.Json;

namespace RouteMarksViewer.Models
{
    public class RouteScheduler : INotifyPropertyChanged, IEquatable<RouteScheduler>
    {
        private int route_id;
        private int day_of_week;
        private int start_time;
        private int window_start_time;
        private int adding_date;
        private Nullable<int> deleting_date;
        private int is_deleted;
        private string name;

        [SQLite.PrimaryKey]
        [SQLite.AutoIncrement]
        [SQLite.Unique]
        [SQLite.NotNull]
        public int Id { get; set; }
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
        public int DayOfWeek
        {
            get { return day_of_week; }
            set
            {
                day_of_week = value;
                OnPropertyChanged("DayOfWeek");
            }
        }
        public int StartTime
        {
            get { return start_time; }
            set
            {
                start_time = value;
                OnPropertyChanged("StartTime");
            }
        }
        public int WindowStartTime
        {
            get { return window_start_time; }
            set
            {
                window_start_time = value;
                OnPropertyChanged("WindowStartTime");
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
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [JsonIgnore]
        [ManyToOne]
        public Models.Route Route { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Equals(RouteScheduler other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id) &&
                (this.AddingDate.Equals(other.AddingDate)) &&
           (
               object.ReferenceEquals(this.DeletingDate, other.DeletingDate) ||
               this.DeletingDate != null &&
               this.DeletingDate.Equals(other.DeletingDate)
           ) && this.RouteId.Equals(other.RouteId)
           && this.DayOfWeek.Equals(other.DayOfWeek)
           && this.StartTime.Equals(other.StartTime)
           && this.WindowStartTime.Equals(other.WindowStartTime)
           && this.AddingDate.Equals(other.AddingDate)
           && this.IsDeleted.Equals(other.IsDeleted) &&
           (
               object.ReferenceEquals(this.Name, other.Name) ||
               this.Name != null &&
               this.Name.Equals(other.Name)
           );

        }

        public static Models.RouteScheduler GetCopyOfRouteScheduler(Models.RouteScheduler _routeScheduler)
        {
            return new Models.RouteScheduler()
            {
                Id = _routeScheduler.Id,
                AddingDate = _routeScheduler.AddingDate,
                DayOfWeek = _routeScheduler.DayOfWeek,
                DeletingDate = _routeScheduler.DeletingDate,
                IsDeleted = _routeScheduler.IsDeleted,
                Route = _routeScheduler.Route,
                RouteId = _routeScheduler.RouteId,
                StartTime = _routeScheduler.StartTime,
                WindowStartTime = _routeScheduler.WindowStartTime,
                Name = _routeScheduler.Name
            };
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
