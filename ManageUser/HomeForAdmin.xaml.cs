using ManageUser.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
    /// Interaction logic for HomeForAdmin.xaml
    /// </summary>
    public partial class HomeForAdmin : Window
    {
        private User currentUser;
        private int selectedUserId;
        public HomeForAdmin(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadUserData();
        }

        MUserContext context = new MUserContext();
        public void LoadUserData()
        {
            currentDate.Text = DateTime.Now.ToString("dd MMM yyyy");
            var userData = context.Users.Include(u => u.UserProfiles)
                .FirstOrDefault(u => u.UserId == currentUser.UserId);
            if(userData != null)
            {
                txtName.Text = userData.UserName;
            }
            
            var data = context.Users
                .Include(u => u.UserProfiles)
                .Where(u => u.RoleId != 1)
                .Select(u => new
                {
                    ID = u.UserId,
                    UserName = u.UserName,
                    EmailAddress = u.Email,
                    Password = u.PassWord,
                    SignupDate = u.CreatAt,
                    Role = u.RoleId == 1 ? "Admin" : u.RoleId == 2 ? "User" : "Unknown",
                    PhoneNumber = u.UserProfiles.FirstOrDefault() != null ? u.UserProfiles.FirstOrDefault().PhoneNumber : "N/A"
                })
                .ToList();
                dgUser.ItemsSource = data;           
        }       

        private void dgUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUser.SelectedItem == null)
            {
                // Không có user nào được chọn
                viewprofileBtn.IsEnabled = false; // Vô hiệu hóa nút              
            }
            else
            {
                // Có user được chọn
                viewprofileBtn.IsEnabled = true; // Kích hoạt nút              
            }

            // Reset selectedUserId khi không có user được chọn
            selectedUserId = dgUser.SelectedItem != null ? ((dynamic)dgUser.SelectedItem).ID : 0;

        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.Show();
        }

        private void viewprofileBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewProfile viewProfile = new ViewProfile(currentUser);
            viewProfile.Show();
            Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = context.Users.Include(u => u.UserProfiles)
                .Select(u => new
                {
                    ID = u.UserId,
                    UserName = u.UserName,
                    EmailAddress = u.Email,
                    Password = u.PassWord,
                    SignupDate = u.CreatAt,
                    Role = u.RoleId == 1 ? "Admin" : u.RoleId == 2 ? "User" : "Unknown",
                    PhoneNumber = u.UserProfiles.FirstOrDefault() != null ? u.UserProfiles.FirstOrDefault().PhoneNumber : "N/A"
                })
                .Where(u => u.UserName.Contains(txtSearch.Text) || u.EmailAddress.Contains(txtSearch.Text))
                .ToList();

            dgUser.ItemsSource = data;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddUser addUser = new AddUser(currentUser);
            addUser.Show();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (selectedUserId == 0)
            {
                MessageBox.Show("Please select a user to view.");
                return;
            }
            ViewProfileUser viewProfileUser = new ViewProfileUser(selectedUserId, currentUser);
            viewProfileUser.Show();
            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (selectedUserId == 0)
            {
                MessageBox.Show("Please select a user to edit.");
                return;
            }
            EditProfileUser editProfileUser = new EditProfileUser(selectedUserId, currentUser);
            editProfileUser.Show();
            Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (selectedUserId == 0)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }
            try
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var userToDelete = context.Users.Include(u => u.UserProfiles)
                        .FirstOrDefault(u => u.UserId == selectedUserId);
                    if(userToDelete != null)
                    {
                        var profileToDelete = context.UserProfiles.Where(up => up.UserId == selectedUserId)
                            .ToList();
                        context.UserProfiles.RemoveRange(profileToDelete);
                        context.Users.Remove(userToDelete);
                        context.SaveChanges();
                        LoadUserData();
                        MessageBox.Show("User and associated profiles deleted successfully.");
                    }
                }
               
                } catch(Exception ex)
            {
                MessageBox.Show("Delete fail:" + ex.Message);
            }
        }
    }
}
