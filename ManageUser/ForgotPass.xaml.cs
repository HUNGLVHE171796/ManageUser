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
    /// Interaction logic for ForgotPass.xaml
    /// </summary>
    public partial class ForgotPass : Window
    {
        public ForgotPass()
        {
            InitializeComponent();
        }

        MUserContext db = new MUserContext();

        private void changepassBtn_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text;
            var newPassword = txtNewpassword.Password;
            var confirmPassword = txtConfirmnewpass.Password;
            bool isValid = true;

            //reset thong bao loi
            txtForgotEmailError.Text = string.Empty;
            txtForgotPasswordError.Text = string.Empty;
            txtForgotConfirmPasswordError.Text = string.Empty;

            //check input invalid
            if (!ValidateEmail(email, out string emailError))
            {
                txtForgotEmailError.Text = emailError;
                isValid = false;
            }

            if (!ValidateNewPassword(newPassword, out string newpasswordError))
            {
                txtForgotPasswordError.Text = newpasswordError;
                isValid = false;
            }

            if (!ValidateConfirmPassword(newPassword, confirmPassword, out string confirmPasswordError))
            {
                txtForgotConfirmPasswordError.Text = confirmPasswordError;
                isValid = false;
            }

            if (!isValid) return;

            //Check if the email matches in the database
            var user = db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                MessageBox.Show("The email does not exist in the system");
                return;
            }

            //update database
            user.PassWord = newPassword;
            db.SaveChanges();

            MessageBox.Show("Change password successfully");

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private bool ValidateConfirmPassword(string newPassword, string confirmPassword, out string confirmPasswordError)
        {
            confirmPasswordError = string.Empty;
            if(newPassword != confirmPassword)
            {
                confirmPasswordError = "Confirm password does not match the password";
                return false;
            }
            return true;
        }

        private bool ValidateNewPassword(string password, out string newpasswordError)
        {
            newpasswordError = string.Empty;

            if (string.IsNullOrWhiteSpace(password))
            {
                newpasswordError = "The password cannot be left blank";
                return false;
            }

            if (password.Contains(" "))
            {
                newpasswordError = "The password cannot contain spaces";
                return false;
            }

            string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                newpasswordError = "Password must be at least 8 characters";
                return false;
            }

            

            return true;
        }

        

        private bool ValidateEmail(string email, out string emailError)
        {
            emailError = string.Empty;
            if (string.IsNullOrWhiteSpace(email))
            {
                emailError = "Empty! Please enter an email";
            }
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                emailError = "Invalid email format";
                return false;
            }
            return true;
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
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
    }
}
