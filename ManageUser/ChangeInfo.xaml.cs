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
using System.Xml.Linq;

namespace ManageUser
{
    /// <summary>
    /// Interaction logic for ChangeInfo.xaml
    /// </summary>
    public partial class ChangeInfo : Window
    {
        private User currentUser;
        public ChangeInfo(Models.User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadUserData();
        }
        
        MUserContext context = new MUserContext();
        public void LoadUserData()
        {
            var userData = context.Users
                .Include(u => u.UserProfiles)
                .FirstOrDefault(u => u.UserId == currentUser.UserId);
            if(userData != null)
            {
                txtName.Text = userData.UserName;
                txtEmail.Text = userData.Email;
                txtN.Text = userData.UserName;
                // Set txtR.Text based on roleid
                txtR.Text = userData.RoleId == 1 ? "Admin" : userData.RoleId == 2 ? "User" : "Unknown";
                var userProfile = userData.UserProfiles.FirstOrDefault();
                if (userProfile != null)
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

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void changepassBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePass changePass = new ChangePass(currentUser);
            changePass.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            //Check input empty
            if(string.IsNullOrEmpty(txtName.Text)
                || string.IsNullOrEmpty(txtPhone.Text)
                || string.IsNullOrEmpty(txtSex.Text)
                || string.IsNullOrEmpty(txtWard.Text)
                || string.IsNullOrEmpty(txtDistrict.Text)
                || string.IsNullOrEmpty(txtCity.Text) 
                || string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Empty! Please fill all boxes.");
                return;
            }
            string phonePattern = @"^\d{10,15}$";
            if (!Regex.IsMatch(txtPhone.Text, phonePattern))
            {
                MessageBox.Show("Invalid format phone number");
                return;
            }
            string sex = txtSex.Text.Trim();
            if (sex != "Male" && sex != "Female")
            {
                MessageBox.Show("Invalid sex! Please enter 'Male' or 'Female'.");
                return;
            }

            //perfom update
            try
            {
                var userData = context.Users
                    .Include(u => u.UserProfiles)
                    .FirstOrDefault(u => u.UserId == currentUser.UserId);
                if(userData != null)
                {
                    userData.UserName = txtName.Text.Trim();
                    var userProfile = userData.UserProfiles.FirstOrDefault();
                    if(userProfile != null)
                    {
                        userProfile.PhoneNumber = txtPhone.Text.Trim();
                        userProfile.Ward = txtWard.Text.Trim();
                        userProfile.District = txtDistrict.Text.Trim();
                        userProfile.City = txtCity.Text.Trim();
                        userProfile.Address = txtAddress.Text.Trim();
                        userProfile.Sex = sex == "Male";
                    } else
                    {
                        //create new Userprofile
                        userProfile = new UserProfile
                        {
                            UserId = currentUser.UserId,
                            UserName = txtName.Text.Trim(),
                            PhoneNumber = txtPhone.Text.Trim(),
                            Ward = txtWard.Text.Trim(),
                            District = txtDistrict.Text.Trim(),
                            City = txtCity.Text.Trim(),
                            Address = txtAddress.Text.Trim(),
                            Sex = sex == "Male"
                        };
                        context.UserProfiles.Add(userProfile);
                    }
                    context.SaveChanges();
                    MessageBox.Show("User information updated successfully.");
                } else
                {
                    MessageBox.Show("User not found.");
                }
            } catch(Exception ex)
            {
                MessageBox.Show("An error occurred while updating the user information: " + ex.Message);
            }

            HomeForUser homeForAdmin = new HomeForUser(currentUser);
            homeForAdmin.Show();
            Close();
        }
    }
}
