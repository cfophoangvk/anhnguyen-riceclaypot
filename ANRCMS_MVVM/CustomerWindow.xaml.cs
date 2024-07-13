using ANRCMS_MVVM.Models;
using ANRCMS_MVVM.ViewModel;
using System.Windows;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private Customer Customer { get; set; }
        public CustomerFoodMenu Menu { get; set; }
        public CustomerProfile Profile { get; set; }
        public CustomerOrders CustomerOrder { get; set; }
        public CustomerWindow(Customer c)
        {
            this.DataContext = new CustomerViewModel(c);
            Customer = c;
            Menu = new CustomerFoodMenu(Customer);
            Profile = new CustomerProfile(Customer);
            CustomerOrder = new CustomerOrders(Customer);
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Đăng xuất?", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Homepage h = new Homepage();
                h.Show();
                this.Close();
            }
        }

        private void btnUserProfile_Click(object sender, RoutedEventArgs e)
        {
            spUserProfile.Visibility = Visibility.Collapsed;
            btnCart.Visibility = Visibility.Collapsed;
            frMain.Content = Profile;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frMain.Content = Menu;
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = CustomerOrder;
            btnCart.Visibility = Visibility.Collapsed;
            btnHome.Visibility = Visibility.Visible;
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = Menu;
            btnCart.Visibility = Visibility.Visible;
            btnHome.Visibility = Visibility.Collapsed;
        }
    }
}
