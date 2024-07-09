using ANRCMS_MVVM.Models;
using ANRCMS_MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for CustomerProfile.xaml
    /// </summary>
    public partial class CustomerProfile : Page
    {
        public CustomerProfile(Customer c)
        {
            InitializeComponent();
            this.DataContext = new CustomerViewModel(c);
            password.Password = ((CustomerViewModel)this.DataContext).LoggedInCustomer.Password;
            tbPassword.Text = password.Password;
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow w = (CustomerWindow)Window.GetWindow(this);
            w.spUserProfile.Visibility = Visibility.Visible;
            w.frMain.Content = null;
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (password.Visibility == Visibility.Visible)
            {
                tbPasswordLength.Text = password.Password.Length.ToString();
                tbPassword.Text = password.Password;
                ((CustomerViewModel)this.DataContext).EditingCustomer.Password = tbPassword.Text;
            }
        }

        private void tbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPassword.Visibility == Visibility.Visible)
            {
                password.Password = tbPassword.Text;
            }
        }
    }
}
