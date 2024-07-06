using System.Windows;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Đăng xuất?","",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Homepage h = new Homepage();
                h.Show();
                this.Close();
            }
        }
    }
}
