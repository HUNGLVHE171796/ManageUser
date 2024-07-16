using ManageUser.Models;
using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void backLogin(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        MUserContext db = new MUserContext();
        private void signupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check input empty
                if (string.IsNullOrEmpty(txtUsername.Text)
                    || string.IsNullOrEmpty(txtEmail.Text)
                    || string.IsNullOrEmpty(txtPassword.Password)
                    || string.IsNullOrEmpty(txtConfirmpass.Password))
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
                if (db.Users.Any(u => u.Email == txtEmail.Text))
                {
                    MessageBox.Show("Email already exists.");
                    return;
                }
                //check input pass
                string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
                if (!Regex.IsMatch(txtPassword.Password, passwordPattern))
                {
                    MessageBox.Show("Password must be at least 8 characters long and contain both letters and numbers");
                    return;
                }
                //Check confirm password
                if (txtPassword.Password != txtConfirmpass.Password)
                {
                    MessageBox.Show("Password does not match the confirm password");
                    return;
                }

                //Create User want add
                User u = new User()
                {
                    UserName = txtUsername.Text,
                    Email = txtEmail.Text,
                    PassWord = txtPassword.Password,
                    CreatAt = DateOnly.FromDateTime(DateTime.Now),
                    RoleId = 2,
                };

                db.Users.Add(u);
                //Save changes to the database
                db.SaveChanges(true);

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Sign Up fail:" + ex.Message);
            }
        }
    }
}
