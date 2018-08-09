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
    public class LoginViewModel : DependencyObject, INotifyPropertyChanged
    {

        IEnumerable<Models.User> users;
        IEnumerable<Models.UserRole> user_roles;
        string login;
        string password;

        public Views.LoginView CurrentLoginWindow { get; set; }

        #region Commands
        RelayCommand okCommand;
        #endregion

        ApplicationContext CurrentDataBase;
        public LoginViewModel()
        {
            CurrentDataBase = new ApplicationContext();
            CurrentDataBase.UserRoles.Load();
            CurrentDataBase.Users.Where(
                o => o.IsDeleted != 1
                ).Load();
            Users = CurrentDataBase.Users.Local.ToBindingList();
            UserRoles = CurrentDataBase.UserRoles.Local.ToBindingList();
            
        }

        public IEnumerable<Models.User> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }
        public IEnumerable<Models.UserRole> UserRoles
        {
            get
            {
                return user_roles;
            }
            set
            {
                user_roles = value;
                OnPropertyChanged("UserRoles");
            }
        }

        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
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

        private void OkCommandMethod()
        {
            if (String.IsNullOrEmpty( Password ) || String.IsNullOrEmpty( Login ))
            {
                MessageBox.Show("Введите логин и пароль!", "Вход", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            Models.User CurrUser = Users.FirstOrDefault(o => o.Login == Login);
            if(CurrUser == null)
            {
                MessageBox.Show("Пользователь с таким именем не найден!", "Вход", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            byte[] pass_bytes = Encoding.UTF8.GetBytes(Password);
            byte[] pass_salt = Encoding.UTF8.GetBytes(CurrUser.PasswordSalt);
            byte[] hash = Helpers.PasswordHelper.GenerateSaltedHash(pass_bytes, pass_salt);
            string hash_str_for_check = Convert.ToBase64String(hash);
            if(hash_str_for_check == CurrUser.PasswordHash)
            {
                Application.Current.Properties["CurrentUser"] = CurrUser;
                Views.StartWindow startWindow = new Views.StartWindow();
                startWindow.Show();
                CurrentLoginWindow.Close();
            }
            else
            {
                MessageBox.Show("Неправильный пароль!", "Вход", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

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
