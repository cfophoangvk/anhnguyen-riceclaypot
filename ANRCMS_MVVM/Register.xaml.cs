using ANRCMS_MVVM.Models;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void btnEnablePassword_Click(object sender, RoutedEventArgs e)
        {
            password.Visibility = Visibility.Collapsed;
            btnEnablePassword.Visibility = Visibility.Collapsed;
            btnDisablePassword.Visibility = Visibility.Visible;
            tbPassword.Visibility = Visibility.Visible;
            Binding b = new Binding();
            b.Source = tbPassword;
            b.Path = new PropertyPath("Text.Length");
            b.Mode = BindingMode.OneWay;
            tbPasswordLength.SetBinding(TextBlock.TextProperty, b);
        }

        private void btnDisablePassword_Click(object sender, RoutedEventArgs e)
        {
            tbPassword.Visibility = Visibility.Collapsed;
            btnDisablePassword.Visibility = Visibility.Collapsed;
            btnEnablePassword.Visibility = Visibility.Visible;
            password.Visibility = Visibility.Visible;
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (password.Visibility == Visibility.Visible)
            {
                tbPasswordLength.Text = password.Password.Length.ToString();
                tbPassword.Text = password.Password;
            }
        }

        private void tbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPassword.Visibility == Visibility.Visible)
            {
                password.Password = tbPassword.Text;
            }
        }

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Text == "" || tbCustomerName.Text == "" || tbCustomerPhone.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ dữ liệu!");
                return;
            }
            if (!Regex.IsMatch(tbCustomerPhone.Text, @"^\d{10,11}$"))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng!\nSố điện thoại phải có 10 hoặc 11 số!");
                return;
            }
            if (AnhnguyenclaypotDbContext.INSTANCE.Customers.FirstOrDefault(x => x.CustomerPhone == tbCustomerPhone.Text) != null)
            {
                MessageBox.Show("Số điện thoại đã tồn tại!");
                return;
            }
            if (MessageBox.Show("Bạn có muốn tạo tài khoản mới?","",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                AnhnguyenclaypotDbContext.INSTANCE.Add(new Customer
                {
                    CustomerName = tbCustomerName.Text,
                    CustomerPhone = tbCustomerPhone.Text,
                    Address = tbCustomerAddress.Text,
                    Password = tbPassword.Text
                });
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                MessageBox.Show("Thêm thành công!\nVui lòng đăng nhập lại để đặt hàng");
                
                Homepage mainWindow = (Homepage)Window.GetWindow(this);
                mainWindow.frameHome.Content = mainWindow.LoginPage;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Homepage mainWindow = (Homepage)Window.GetWindow(this);
            mainWindow.frameHome.Content = mainWindow.LoginPage;
        }
    }
}
