using System.Windows;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for Homepage.xaml
    /// </summary>
    public partial class Homepage : Window
    {
        public Login LoginPage { get; set; }
        public Register RegisterPage { get; set; }
        public HomepageFoodMenu HomepageFoodMenuPage { get; set; }
        public Homepage()
        {
            InitializeComponent();
            LoginPage = new Login();
            RegisterPage = new Register();
            HomepageFoodMenuPage = new HomepageFoodMenu();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            frameHome.Content = LoginPage;
            btnBack.Visibility = Visibility.Visible;
            btnLogin.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            frameHome.Content = HomepageFoodMenuPage;
            btnLogin.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Collapsed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frameHome.Content = HomepageFoodMenuPage;
        }
    }
}
