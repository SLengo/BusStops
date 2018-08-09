using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows.Data;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RouteMarksViewer.ViewModels
{
    public class UserManagementViewModel : BaseViewModel
    {
        public Views.UserManagementView CurrentUserManagmentView { get; set; }
        ObservableCollection<Models.User> currnetUsers;

        #region Commands
        RelayCommand addUserCommand;
        RelayCommand editUserCommand;
        RelayCommand deleteUserCommand;
        #endregion

        public UserManagementViewModel()
        {
            CurrnetUsers = new ObservableCollection<Models.User>(Users.Where(
                o => o.IsDeleted == 0));
        }

        public ObservableCollection<Models.User> CurrnetUsers
        {
            get
            {
                return currnetUsers;
            }
            set
            {
                currnetUsers = value;
                OnPropertyChanged("CurrnetMarks");
            }
        }

        public RelayCommand AddUserCommand
        {
            get
            {
                return addUserCommand ??
                   (addUserCommand = new RelayCommand((o) =>
                   {
                       AddUserMethod();
                   }
                   ));
            }
        }
        public RelayCommand EditUserCommand
        {
            get
            {
                return editUserCommand ??
                   (editUserCommand = new RelayCommand((SelectedItem) =>
                   {
                       EditUserMethod(SelectedItem as Models.User);
                   }
                   ));
            }
        }
        public RelayCommand DeleteUserCommand
        {
            get
            {
                return deleteUserCommand ??
                   (deleteUserCommand = new RelayCommand((SelectedItem) =>
                   {
                       DeleteUserMethod(SelectedItem as Models.User);
                   }
                   ));
            }
        }

        public void AddUserMethod()
        {
            ViewModels.AddUserViewModel addUserViewModel = new AddUserViewModel(new Models.User());
            Views.AddUserView addUserView = new Views.AddUserView(addUserViewModel);
            addUserView.Owner = CurrentUserManagmentView;
            MakeLogEntry(8, null, null, "open AddUser. Start adding user");
            if ((bool)addUserView.ShowDialog())
            {
                Models.User user_to_base = Models.User.GetCopyOfUser(addUserViewModel.CurrentUser);
                CurrnetUsers.Add(user_to_base);
                AddEntry<Models.User>(user_to_base);
            }
            else
                MakeLogEntry(8, null, null, "open AddUser. Cancel adding user");
        }
        public void EditUserMethod(Models.User SelectedItem)
        {
            if (SelectedItem == null) return;
            Models.User user_to_view = Models.User.GetCopyOfUser(SelectedItem);
            ViewModels.AddUserViewModel addUserViewModel = new AddUserViewModel(user_to_view);
            Views.AddUserView addUserView = new Views.AddUserView(
                addUserViewModel
                );
            addUserView.Owner = CurrentUserManagmentView;
            MakeLogEntry(8, null, null, "open AddUser. Start editing user");
            if ((bool)addUserView.ShowDialog())
            {
                Models.User user_to_base = Models.User.GetCopyOfUser(addUserViewModel.CurrentUser);
                EditEntry<Models.User>(user_to_base);
            }
            else
                MakeLogEntry(8, null, null, "open AddUser. Cancel editing user");
        }
        public void DeleteUserMethod(Models.User SelectedItem)
        {
            MakeLogEntry(8, null, null, "open AddUser. Start deleting user");
            if (SelectedItem.UserRoleId != 1 && MessageBoxResult.Yes == DeleteEntry<Models.User>(SelectedItem))
            {
                CurrnetUsers.Remove(CurrnetUsers.FirstOrDefault(o => o.Id == SelectedItem.Id));
            }
            else
                MakeLogEntry(8, null, null, "open AddUser. Cancel deleting user");
        }
    }
}
