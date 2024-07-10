using ANRCMS_MVVM.Models;
using ANRCMS_MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for StaffOrders.xaml
    /// </summary>
    public partial class StaffOrders : Page
    {
        public StaffOrders(Staff s)
        {
            this.DataContext = new StaffViewModel(s);
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvOrders.SelectedItem != null)
            {
                short status = ((Order)lvOrders.SelectedItem).Status;
                switch (status)
                {
                    case 1: rbStatus1.IsChecked = true; break;
                    case 2: rbStatus2.IsChecked = true; break;
                    case 3: rbStatus3.IsChecked = true; break;
                    case 4: rbStatus4.IsChecked = true; break;
                }
            }
        }

        private void rbStatus_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            if (button != null)
            {
                var buttonTag = (sender as RadioButton)?.Tag;
                short buttonStatus = Convert.ToInt16(buttonTag);
                short selectedStatus = ((Order)lvOrders.SelectedItem).Status;
                if (buttonStatus == selectedStatus) return;
                var currentOrder = (Order)lvOrders.SelectedItem;
                var order = AnhnguyenclaypotDbContext.INSTANCE.Orders.Find(currentOrder.OrderId);
                if (order != null)
                {
                    order.Status = buttonStatus;
                    AnhnguyenclaypotDbContext.INSTANCE.Update(order);
                    AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();

                    Order orderToUpdate = new Order
                    {
                        OrderId = order.OrderId,
                        Customer = order.Customer,
                        Discount = order.Discount,
                        Status = order.Status,
                        OrderDate = order.OrderDate,
                        InTime = order.InTime,
                        OutTime = order.OutTime,
                        TotalPrice = order.TotalPrice
                    };
                    var context = (StaffViewModel)this.DataContext;
                    var selectedOrder = context.SelectedOrder;
                    context.OrderList[context.OrderList.IndexOf(selectedOrder)] = orderToUpdate;

                    MessageBox.Show("Đã cập nhật trạng thái!");

                    button.IsChecked = false;
                }
            }
        }
    }
}
