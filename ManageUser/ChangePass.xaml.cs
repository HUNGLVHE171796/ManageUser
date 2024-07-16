using ManageUser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManageUser
{
    /// <summary>
    /// Interaction logic for ChangePass.xaml
    /// </summary>
    public partial class ChangePass : Window
    {

        private User currentUser;
        public ChangePass(Models.User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadUserData();
        }

        private void viewprofileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser.RoleId == 2)
            {
                HomeForUser homeForUser = new HomeForUser(currentUser);
                homeForUser.Show();
                Close();
            }
            else if (currentUser.RoleId == 1)
            {
                ViewProfile viewProfile = new ViewProfile(currentUser);
                viewProfile.Show();
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeInfo changeInfo = new ChangeInfo(currentUser);
            changeInfo.Show();
            Close();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if(currentUser.RoleId == 2)
            {
                HomeForUser homeForUser = new HomeForUser(currentUser);
                homeForUser.Show();
                Close();
            } else if(currentUser.RoleId == 1)
            {
                ViewProfile viewProfile = new ViewProfile(currentUser);
                viewProfile.Show();
                Close();
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangeInfo changeInfo = new ChangeInfo(currentUser);
            changeInfo.Show();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentPass = txtCurpassword.Password;
            var newPass = txtNewpassword.Password;
            var cfnewPass = txtCfNewpassword.Password;
            bool isValid = true;

            //reset message invalid
            txtCurpasswordError.Text = string.Empty;
            txtNewpasswordError.Text = string.Empty;
            txtCfNewpasswordError.Text = string.Empty;

            //Check input invalid
            if(!ValidateCurPassword(currentPass, out string errorCur))
            {
                txtCurpasswordError.Text = errorCur;
                isValid = false;
            }
            if(!ValidateNewPassword(newPass, out string errorNew))
            {
                txtNewpasswordError.Text = errorNew;
                isValid = false;
            }
            if(!ValidateConfirmPassword(newPass, cfnewPass, out string errorConfirm))
            {
                txtCfNewpasswordError.Text = errorConfirm;
                isValid = false;
            }

            if (!isValid) return;

            //Check currentpass matches with password of user in here 
            var user = context.Users.FirstOrDefault(u => u.UserId == currentUser.UserId);
            if(user.PassWord == currentPass)
            {
                user.PassWord = newPass;
            }
            
            if (context.SaveChanges() > 0)
            {
                txtNotice.Text = "Change password successfully";
            } else
            {
                txtNotice.Text = "Failed to change password.";
            }

            // Clear the content of all PasswordBox controls
            txtCurpassword.Clear();
            txtNewpassword.Clear();
            txtCfNewpassword.Clear();

            // Clear any error messages
            txtCurpasswordError.Text = string.Empty;
            txtNewpasswordError.Text = string.Empty;
            txtCfNewpasswordError.Text = string.Empty;

        }

       

        private bool ValidateConfirmPassword(string newPass, string cfnewPass, out string errorConfirm)
        {
            errorConfirm = string.Empty;
            if (newPass != cfnewPass)
            {
                errorConfirm = "Confirm password does not match the password";
                return false;
            }
            return true;
        }

        private bool ValidateNewPassword(string newPass, out string errorNew)
        {
            errorNew = string.Empty;
            if (string.IsNullOrWhiteSpace(newPass))
            {
                errorNew = "The password cannot be left blank";
                return false;
            }

            if (newPass.Contains(" "))
            {
                errorNew = "The password cannot contain spaces";
                return false;
            }

            string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            if (!Regex.IsMatch(newPass, passwordPattern))
            {
                errorNew = "Password must be at least 8 characters long and contain both letters and numbers";
                return false;
            }
            return true;
        }

        private bool ValidateCurPassword(string currentPass, out string errorCur)
        {
            errorCur = string.Empty;
            var user = context.Users.FirstOrDefault(u => u.UserId == currentUser.UserId);
            if (user == null || user.PassWord != currentPass)
            {
                errorCur = "Current password is incorrect";
                return false;
            } else 
            if (string.IsNullOrEmpty(currentPass))
            {
                errorCur = "Empty!";
                return false;
            }
            return true;
        }

        private void discardBtn_Click(object sender, RoutedEventArgs e)
        {
            // Clear the content of all PasswordBox controls
            txtCurpassword.Clear();
            txtNewpassword.Clear();
            txtCfNewpassword.Clear();

            // Clear any error messages
            txtCurpasswordError.Text = string.Empty;
            txtNewpasswordError.Text = string.Empty;
            txtCfNewpasswordError.Text = string.Empty;
        }

        MUserContext context = new MUserContext();
        public void LoadUserData()
        {
            var userData = context.Users
                .Include(u => u.UserProfiles)
                .FirstOrDefault(u => u.UserId == currentUser.UserId);
            if(userData != null)
            {
                txtN.Text = userData.UserName;
                // Set txtR.Text based on roleid
                txtR.Text = userData.RoleId == 1 ? "Admin" : userData.RoleId == 2 ? "User" : "Unknown";
            }
        }
    }
}
