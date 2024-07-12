using ANRCMS_MVVM.Models;
using ANRCMS_MVVM.ViewModel;
using System.Windows.Controls;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for CustomerOrders.xaml
    /// </summary>
    public partial class CustomerOrders : Page
    {
        private Customer Customer { get; set; }
        public CustomerOrders(Customer c)
        {
            InitializeComponent();
            Customer = c;
            this.DataContext = new CustomerViewModel(Customer);
        }
    }
}
