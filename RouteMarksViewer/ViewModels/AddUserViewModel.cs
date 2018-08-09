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
    public class AddUserViewModel : BaseViewModel
    {
        public Models.User CurrentUser { get; set; }
        public Views.AddUserView CurrentAddUserView { get; set; }
        string password;
        string password_repeat;

        #region Commands
        RelayCommand okCommand;
        RelayCommand cancelCommand;
        #endregion

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
        public string PasswordRepeat
        {
            get
            {
                return password_repeat;
            }
            set
            {
                password_repeat = value;
                OnPropertyChanged("PasswordRepeat");
            }
        }

        public AddUserViewModel(Models.User user)
        {
            CurrentUser = user;
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

            string message = "";
            if (String.IsNullOrEmpty(CurrentUser.Login))
            {
                message += "Введите логин пользователя!\n";
            }
            if (String.IsNullOrEmpty(CurrentUser.Name))
            {
                message += "Введите имя пользователя!\n";
            }
            if (String.IsNullOrEmpty(Password))
            {
                message += "Введите пароль!\n";
            }
            if (!String.IsNullOrEmpty(Password) && String.IsNullOrEmpty(PasswordRepeat))
            {
                message += "Введите пароль еще раз!\n";
            }
            if (!String.IsNullOrEmpty(Password) && !String.IsNullOrEmpty(PasswordRepeat) &&
                Password != PasswordRepeat)
            {
                message += "Введеные пароли не совпадают!\n";
            }
            if (message == "")
            {
                if (CurrentUser.UserRoleId != 1)
                {
                    CurrentUser.UserRoleId = CurrentUser.UserRole != null ? CurrentUser.UserRole.Id : 0;
                }
                CurrentUser.AddingDate = CurrentUser.AddingDate == 0 ?
                    (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds : CurrentUser.AddingDate;

                string pass_salt = Helpers.PasswordHelper.CreateSalt(10);
                byte[] pass_bytes = Encoding.UTF8.GetBytes(Password);
                byte[] pass_salt_bytes = Encoding.UTF8.GetBytes(pass_salt);
                byte[] hash = Helpers.PasswordHelper.GenerateSaltedHash(pass_bytes, pass_salt_bytes);
                string hash_str_to_base = Convert.ToBase64String(hash);

                CurrentUser.PasswordHash = hash_str_to_base;
                CurrentUser.PasswordSalt = pass_salt;

                if (CurrentUser.Id == 0)
                {
                    CurrentUser.UserRole = null;
                }

                CurrentAddUserView.DialogResult = true;
            }
            else
                MessageBox.Show(message);

        }
        public void CancelCommandMethod()
        {
            CurrentAddUserView.DialogResult = false;
        }
    }
}
