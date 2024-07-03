using ManageUser.Models;
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
    /// Interaction logic for HomeForAdmin.xaml
    /// </summary>
    public partial class HomeForAdmin : Window
    {
        public HomeForAdmin()
        {
            InitializeComponent();
            LoadData();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.Show();
        }

        MUserContext db = new MUserContext();
        List<User> users = new List<User>();

        public void LoadData()
        {
            var data = db.UserProfiles.Select(p => new
            {
                UserName = p.UserName,
                Sex = p.Sex ? "Male" : "Female",
                Phone = p.PhoneNumber,
                City = p.City,
                District = p.District,
                Ward = p.Ward,
                Address = p.Address,
            })
            .ToList();
            dgUser.ItemsSource = data;

        }

        private void dgUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dgUser.SelectedItem == null)
            {
                return;
            }
            var user = dgUser.SelectedItem as dynamic;
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
