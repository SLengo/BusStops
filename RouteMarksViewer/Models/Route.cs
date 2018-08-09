using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;
using Newtonsoft.Json;

namespace RouteMarksViewer.Models
{
    public class Route : INotifyPropertyChanged, IEquatable<Route>
    {
        private string name;
        private int adding_date;
        private Nullable<int> deleting_date;
        private int is_deleted;

        [SQLite.PrimaryKey]
        [SQLite.AutoIncrement]
        [SQLite.Unique]
        [SQLite.NotNull]
        public int Id { get; set; }
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
        



        [JsonIgnore]
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Models.Mark> Marks { get; set; }

        

        public bool Equals(Route other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id) &&
           (
               this.AddingDate.Equals(other.AddingDate)
           ) &&
           (object.ReferenceEquals(this.Name, other.Name) ||
               this.Name != null &&
               this.Name.Equals(other.Name))
               &&
           (object.ReferenceEquals(this.DeletingDate, other.DeletingDate) ||
               this.DeletingDate != null &&
               this.DeletingDate.Equals(other.DeletingDate))
               &&
               (
               this.IsDeleted.Equals(other.IsDeleted)
               
           );


        }

        public static Models.Route GetCopyOfRoute(Models.Route route)
        {
            return new Route() {
                AddingDate = route.AddingDate,
                DeletingDate = route.DeletingDate,
                Id = route.Id,
                IsDeleted = route.IsDeleted,
                Marks = route.Marks,
                Name = route.Name,
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
