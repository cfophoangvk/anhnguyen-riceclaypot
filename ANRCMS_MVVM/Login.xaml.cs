using ANRCMS_MVVM.Models;
using System.Windows;
using System.Windows.Controls;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string phone = tbCustomerPhone.Text;
            string password = pwCustomerPassword.Password;
            var customer = AnhnguyenclaypotDbContext.INSTANCE.Customers.Where(x => x.CustomerPhone == phone && x.Password == password).FirstOrDefault();
            if (customer != null)
            {
                CustomerWindow c = new CustomerWindow(customer);
                c.Show();
                Window mainWindow = Window.GetWindow(this);
                mainWindow.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng, vui lòng thử lại!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string phone = tbStaffPhone.Text;
            string password = pwStaffPassword.Password;
            var staff = AnhnguyenclaypotDbContext.INSTANCE.Staff.Where(x => x.StaffPhone == phone && x.Password == password).FirstOrDefault();
            if (phone == "admin" && password == "admin")
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                Window mainWindow = Window.GetWindow(this);
                mainWindow.Close();
            }
            else if (staff != null)
            {
                StaffWindow s = new StaffWindow(staff);
                s.Show();
                Window mainWindow = Window.GetWindow(this);
                mainWindow.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng, vui lòng thử lại!");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Homepage mainWindow = (Homepage)Window.GetWindow(this);
            mainWindow.frameHome.Content = mainWindow.RegisterPage;
        }
    }
}
