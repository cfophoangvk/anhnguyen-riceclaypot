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
        public CustomerWindow(Customer c)
        {
            this.DataContext = new CustomerViewModel(c);
            Customer = c;
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
            frMain.Content = new CustomerProfile(Customer);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //frMain.Content = new FoodMenu();
        }
    }
}
