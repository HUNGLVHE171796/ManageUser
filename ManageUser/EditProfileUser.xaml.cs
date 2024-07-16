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
    /// Interaction logic for EditProfileUser.xaml
    /// </summary>
    public partial class EditProfileUser : Window
    {
        private User currentUser;
        private int selectedUserId;
        public EditProfileUser(int userid, User user)
        {
            InitializeComponent();
            currentUser = user;
            selectedUserId = userid;
            LoadUserData();
        }

        MUserContext context = new MUserContext();
        public void LoadUserData()
        {
            var userData = context.Users
         .Include(u => u.UserProfiles)
         .Include(u => u.Role)
         .FirstOrDefault(u => u.UserId == selectedUserId);

            if (userData != null)
            {
                txtName.Text = userData.UserName;
                txtEmail.Text = userData.Email;
                txtPass.Text = userData.PassWord;
                txtDate.Text = userData.CreatAt.ToString("dd MMM yyyy");

                var userProfile = userData.UserProfiles.FirstOrDefault();
                if (userProfile != null)
                {
                    txtPhone.Text = userProfile.PhoneNumber;
                    var sexOptions = new List<string> { "Male", "Female" };
                    txtSex.ItemsSource = sexOptions;
                    txtSex.SelectedItem = userProfile.Sex ? "Male" : "Female";
                    txtWard.Text = userProfile.Ward;
                    txtCity.Text = userProfile.City;
                    txtDistrict.Text = userProfile.District;
                    txtAddress.Text = userProfile.Address;
                }

                var roles = context.Roles.ToList();
                txtRole.ItemsSource = roles;
                txtRole.DisplayMemberPath = "RoleName";
                txtRole.SelectedValuePath = "RoleId";
                txtRole.SelectedValue = userData.RoleId;

                
            }

        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            //Check input empty
            if (string.IsNullOrEmpty(txtName.Text)
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
           
            string roleid = txtRole.Text.Trim();
            string selectedSex = txtSex.SelectedItem.ToString();
            var selectedRole = (Role)txtRole.SelectedItem;

            //perform update
            try
            {
                var userData = context.Users
                    .Include(u => u.UserProfiles)
                    .FirstOrDefault(u => u.UserId == selectedUserId);
                if (userData != null)
                {
                    userData.RoleId = selectedRole.RoleId;
                    var userProfile = userData.UserProfiles.FirstOrDefault();
                    if (userProfile != null)
                    {

                        userProfile.PhoneNumber = txtPhone.Text.Trim();
                        userProfile.Ward = txtWard.Text.Trim();
                        userProfile.District = txtDistrict.Text.Trim();
                        userProfile.City = txtCity.Text.Trim();
                        userProfile.Address = txtAddress.Text.Trim();
                        userProfile.Sex = selectedSex == "Male";
                    }
                    else
                    {
                        //create new Userprofile
                        userProfile = new UserProfile
                        {
                            UserId = selectedUserId,
                            UserName = txtName.Text.Trim(),
                            PhoneNumber = txtPhone.Text.Trim(),
                            Ward = txtWard.Text.Trim(),
                            District = txtDistrict.Text.Trim(),
                            City = txtCity.Text.Trim(),
                            Address = txtAddress.Text.Trim(),
                            Sex = selectedSex == "Male"
                        };
                        context.UserProfiles.Add(userProfile);
                    }
                    context.SaveChanges();
                    MessageBox.Show("User information updated successfully.");
                }
                else
                {
                    MessageBox.Show("User not found.");
                }
            } catch(Exception ex)
            {
                MessageBox.Show("An error occurred while updating the user information: " + ex.Message);
            }
            ViewProfileUser viewProfileUser = new ViewProfileUser(selectedUserId, currentUser);
            viewProfileUser.Show();
            Close();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HomeForAdmin homeForAdmin = new HomeForAdmin(currentUser);
            homeForAdmin.Show();
            Close();
        }
    }
}
