using ManageUser.Models;
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
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private User currentUser;
        public AddUser(Models.User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void viewprofileBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewProfile viewProfile = new ViewProfile(currentUser);
            viewProfile.Show();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HomeForAdmin homeForAdmin = new HomeForAdmin(currentUser);
            homeForAdmin.Show();
            Close();
        }

        MUserContext context = new MUserContext();
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check input empty
                if(string.IsNullOrEmpty(txtName.Text)
                    || string.IsNullOrEmpty(txtEmail.Text)
                    || string.IsNullOrEmpty(txtPassword.Text)
                    || string.IsNullOrEmpty(txtRoleid.Text)
                    || string.IsNullOrEmpty(txtPhone.Text))
                {
                    MessageBox.Show("Empty! Please fill all boxes.");
                    return;
                }
                //Check input gmail
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }
                //Check if email already exists in the database 
                if (context.Users.Any(u => u.Email == txtEmail.Text))
                {
                    MessageBox.Show("Email already exists.");
                    return;
                }
                string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
                if (!Regex.IsMatch(txtPassword.Text, passwordPattern))
                {
                    MessageBox.Show("Password must be at least 8 characters long and contain both letters and numbers");
                    return;
                }
                string roleid = txtRoleid.Text.Trim();
                if(roleid != "2" &&  roleid != "1")
                {

                    MessageBox.Show("Invalid roleid! Please enter 1 for Admin, 2 for User");
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

                var newUser = new User
                {
                    UserName = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    PassWord = txtPassword.Text.Trim(),
                    CreatAt = DateOnly.FromDateTime(DateTime.Now),
                    RoleId = int.Parse(roleid),
                };

                context.Users.Add(newUser);
                context.SaveChanges();

                var newUserProfile = new UserProfile
                {
                    UserId = newUser.UserId,
                    PhoneNumber = txtPhone.Text.Trim(),
                    UserName = string.IsNullOrEmpty(txtName.Text.Trim()) ? "N/A" : txtName.Text.Trim(),
                    Sex = sex == "Male",
                    City = "N/A",
                    District = "N/A",
                    Ward = "N/A",
                    Address = "N/A",
                };
                context.UserProfiles.Add(newUserProfile);
                context.SaveChanges();
                txtNotice.Text = "Add user successfully";

                //Clear the content of textbox
                txtName.Clear();
                txtEmail.Clear();
                txtPassword.Clear();
                txtRoleid.Clear();
                txtPhone.Clear();
                txtSex.Clear();

            }
            catch (Exception ex)
            {
                var errorMessage = new StringBuilder("Add fail: " + ex.Message);
                if (ex.InnerException != null)
                {
                    errorMessage.AppendLine(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage.AppendLine(ex.InnerException.InnerException.Message);
                    }
                }
                MessageBox.Show(errorMessage.ToString());
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
