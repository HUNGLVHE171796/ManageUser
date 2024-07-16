using ManageUser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ViewProfileUser.xaml
    /// </summary>
    public partial class ViewProfileUser : Window
    {
        private User currentUser;
        private int selectedUserId;
        public ViewProfileUser(int userid, User user)
        {
            InitializeComponent();
            currentUser = user;
            selectedUserId = userid;
            LoadUserData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HomeForAdmin homeForAdmin = new HomeForAdmin(currentUser);
            homeForAdmin.Show();
            Close();
        }

        private void viewprofileBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewProfile viewProfile = new ViewProfile(currentUser);
            viewProfile.Show();
            Close();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
        
        MUserContext context = new MUserContext();
        public void LoadUserData()
        {
            var userData = context.Users
                .Include(u => u.UserProfiles)
                .FirstOrDefault(u => u.UserId == selectedUserId);
            if(userData != null)
            {
                txtName.Text = userData.UserName;
                txtEmail.Text = userData.Email;
                txtRole.Text = userData.RoleId == 1 ? "Admin" : userData.RoleId == 2 ? "User" : "Other";
                txtPass.Text = userData.PassWord;
                txtDate.Text = userData.CreatAt.ToString("dd MMM yyyy");
                var userProfile = userData.UserProfiles.FirstOrDefault();
                if(userProfile != null)
                {
                    txtPhone.Text = userProfile.PhoneNumber;
                    txtSex.Text = userProfile.Sex ? "Male" : "Female";
                    txtWard.Text = userProfile.Ward;
                    txtCity.Text = userProfile.City;
                    txtDistrict.Text = userProfile.District;
                    txtAddress.Text = userProfile.Address;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HomeForAdmin homeForAdmin = new HomeForAdmin(currentUser);
            homeForAdmin.Show();
            Close();
        }
    }
}
