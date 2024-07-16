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
using System.Xml.Linq;

namespace ManageUser
{
    /// <summary>
    /// Interaction logic for HomeForUser.xaml
    /// </summary>
    public partial class HomeForUser : Window
    {
        private User currentUser;
        public HomeForUser(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadUserData();
            
        }

        public void LoadUserData()
        {
            using(var context = new MUserContext())
            {
                var userData = context.Users.Include(u => u.UserProfiles)
                    .FirstOrDefault(u => u.UserId == currentUser.UserId);
                if(userData != null)
                {
                    txtName.Text = userData.UserName;
                    txtEmail.Text = userData.Email;
                    txtN.Text = userData.UserName;
                    // Set txtR.Text based on roleid
                    txtR.Text = userData.RoleId == 1 ? "Admin" : userData.RoleId == 2 ? "User" : "Unknown";
                    var userProfile = userData.UserProfiles.FirstOrDefault();
                    if(userProfile != null)
                    {
                        txtPhone.Text = userProfile.PhoneNumber ?? "";
                        txtSex.Text = userProfile.Sex ? "Male" : "Female";
                        txtWard.Text = userProfile.Ward ?? "";
                        txtCity.Text = userProfile.City ?? "";
                        txtDistrict.Text = userProfile.District ?? "";
                        txtAddress.Text = userProfile.Address ?? "";
                    }
                   
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeInfo changeInfo = new ChangeInfo(currentUser);
            changeInfo.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangeInfo changeInfo = new ChangeInfo(currentUser);
            changeInfo.Show();
            Close();
        }

        private void changepassBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePass changePass = new ChangePass(currentUser);
            changePass.Show();
            Close();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        /*private void Button_Click(object sender, RoutedEvenArgs e) {
         OpenFileDiaLog openFileDiaLog = new OpenFileDiaLog();
        OpenFileDiaLog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*jpeg;*png|All files(*.*)|*.*";
        if(openFileDiaLog.ShowDiaLog() == true) {
        //Load selected image into imgAvatar
        imgAvatar.Source = new BitmapImage(new Uri(new Uri(openFileDiaLog.FileName));
        }
         }*/
    }
}
