using ANRCMS_MVVM.Models;
using ANRCMS_MVVM.ViewModel;
using System.Windows;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {
        private Staff Staff { get; set; }
        public StaffWindow(Staff s)
        {
            this.DataContext = new StaffViewModel(s);
            Staff = s;
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
            frMain.Content = new StaffProfile(Staff);
        }
    }
}
