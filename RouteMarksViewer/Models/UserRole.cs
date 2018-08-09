using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;

namespace RouteMarksViewer.Models
{
    public class UserRole : INotifyPropertyChanged, IEquatable<UserRole>
    {
        private string type;
        [SQLite.PrimaryKey]
        [SQLite.AutoIncrement]
        [SQLite.Unique]
        [SQLite.NotNull]
        public int Id { get; set; }
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Models.User> Users { get; set; }

        public bool Equals(UserRole other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id) &&
           (
               object.ReferenceEquals(this.Type, other.Type) ||
               this.Type != null &&
               this.Type.Equals(other.Type)
           );

        }

        public static Models.UserRole GetCopyOfType(Models.UserRole _userRole)
        {
            return new Models.UserRole()
            {
                Id = _userRole.Id,
                Type = _userRole.Type
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
