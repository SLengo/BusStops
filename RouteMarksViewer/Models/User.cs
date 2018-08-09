using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;
using Newtonsoft.Json;

namespace RouteMarksViewer.Models
{
    public class User : INotifyPropertyChanged, IEquatable<User>
    {
        private string login;
        private string password_hash;
        private string password_salt;
        private string name;
        private int adding_date;
        private Nullable<int> deleting_date;
        private int is_deleted;
        private int role_id;

        [SQLite.PrimaryKey]
        [SQLite.AutoIncrement]
        [SQLite.Unique]
        [SQLite.NotNull]
        public int Id { get; set; }
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }
        public string PasswordHash
        {
            get { return password_hash; }
            set
            {
                password_hash = value;
                OnPropertyChanged("PasswordHash");
            }
        }
        public string PasswordSalt
        {
            get { return password_salt; }
            set
            {
                password_salt = value;
                OnPropertyChanged("PasswordSalt");
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
        [ForeignKey(typeof(Models.UserRole))]
        public int UserRoleId
        {
            get { return role_id; }
            set
            {
                role_id = value;
                OnPropertyChanged("RoleId");
            }
        }

        [JsonIgnore]
        [ManyToOne]
        public Models.UserRole UserRole { get; set; }

        public bool Equals(User other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id) &&
           (
               object.ReferenceEquals(this.DeletingDate, other.DeletingDate) ||
               this.DeletingDate != null &&
               this.DeletingDate.Equals(other.DeletingDate)
           ) 
           && this.UserRoleId.Equals(other.UserRoleId) &&
           (
               object.ReferenceEquals(this.Login, other.Login) ||
               this.Login != null &&
               this.Login.Equals(other.Login)
           )
           && this.AddingDate.Equals(other.AddingDate)
           && this.IsDeleted.Equals(other.IsDeleted) &&
           (
               object.ReferenceEquals(this.Name, other.Name) ||
               this.Name != null &&
               this.Name.Equals(other.Name)
           );

        }

        public static Models.User GetCopyOfUser(Models.User _user)
        {
            return new Models.User()
            {
                Id = _user.Id,
                UserRoleId = _user.UserRoleId,
                UserRole = _user.UserRole,
                AddingDate = _user.AddingDate,
                DeletingDate = _user.DeletingDate,
                IsDeleted = _user.IsDeleted,
                Login = _user.Login,
                Name = _user.Name,
                PasswordHash = _user.PasswordHash,
                PasswordSalt = _user.PasswordSalt
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
