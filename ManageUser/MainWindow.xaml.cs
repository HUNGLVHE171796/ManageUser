using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ManageUser.Models;
using MaterialDesignThemes.Wpf;
using System.Linq;

namespace ManageUser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()            {
            InitializeComponent();
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void signupBtn_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
            this.Close();
        }

        MUserContext context = new MUserContext();
        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = context.Users.FirstOrDefault(u => u.Email.Equals(txtUserName.Text) && u.PassWord.Equals(txtPassword.Password));
            if (user != null && user.RoleId == 1) 
            {
                HomeForAdmin homeForAdmin = new HomeForAdmin(user);
                homeForAdmin.Show();
                this.Close();
            }
            else if (user != null && user.RoleId == 2)
            {
                HomeForUser homeForUser = new HomeForUser(user);
                homeForUser.Show();
                this.Close();
            } else
            { 
                MessageBox.Show("Login fail!");
            }
        }

        private void forgotpassBtn_Click(object sender, RoutedEventArgs e)
        {
            ForgotPass forgotPass = new ForgotPass();
            forgotPass.Show();
            this.Close();
        }
    }
}