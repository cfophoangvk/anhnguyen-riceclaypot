using ANRCMS_MVVM.Models;
using ANRCMS_MVVM.ViewModel;
using System.Windows.Controls;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for CustomerFoodMenu.xaml
    /// </summary>
    public partial class CustomerFoodMenu : Page
    {
        public CustomerFoodMenu(Customer c)
        {
            InitializeComponent();
            this.DataContext = new CustomerViewModel(c);
        }
    }
}
