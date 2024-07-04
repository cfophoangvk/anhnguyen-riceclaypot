using System.Windows;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for Homepage.xaml
    /// </summary>
    public partial class Homepage : Window
    {
        public Homepage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            frameHome.Content = new Login();
            btnBack.Visibility = Visibility.Visible;
            btnLogin.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            frameHome.Content = new FoodMenu();
            btnLogin.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Collapsed;
        }
    }
}
