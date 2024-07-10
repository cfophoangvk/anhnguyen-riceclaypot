using ANRCMS_MVVM.Models;
using ANRCMS_MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;
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
                rbStatus1.IsEnabled = true;
                rbStatus2.IsEnabled = true;
                rbStatus3.IsEnabled = true;
                rbStatus4.IsEnabled = true;
                short status = ((Order)lvOrders.SelectedItem).Status;
                switch (status)
                {
                    case 1: rbStatus1.IsChecked = true; break;
                    case 2: rbStatus2.IsChecked = true; break;
                    case 3: rbStatus3.IsChecked = true; break;
                    case 4: rbStatus4.IsChecked = true; break;
                }

                tbDiscount.Text = ((Order)lvOrders.SelectedItem).Discount.ToString();

                int orderId = ((Order)lvOrders.SelectedItem).OrderId;
                lvOrderDetail.ItemsSource = AnhnguyenclaypotDbContext.INSTANCE.OrderDetails
                                                                              .Include(x => x.Food)
                                                                              .Where(x => x.OrderId == orderId)
                                                                              .Select(x => new
                                                                              {
                                                                                  x.Food.FoodVietnameseName,
                                                                                  x.Quantity,
                                                                                  x.Food.FoodPrice,
                                                                                  TotalPrice = x.Food.FoodPrice * x.Quantity
                                                                              })
                                                                              .ToList();
                lvOrderDetail.Items.Refresh();
            }
            else
            {
                rbStatus1.IsEnabled = false;
                rbStatus2.IsEnabled = false;
                rbStatus3.IsEnabled = false;
                rbStatus4.IsEnabled = false;
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

        private void btnDiscount_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedItem != null)
            {
                if (MessageBox.Show("Giảm giá cho đơn này?", "", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                {
                    return;
                }
                var currentOrder = (Order)lvOrders.SelectedItem;
                var order = AnhnguyenclaypotDbContext.INSTANCE.Orders.Find(currentOrder.OrderId);
                if (order != null)
                {
                    order.Discount = int.Parse(tbDiscount.Text);
                    AnhnguyenclaypotDbContext.INSTANCE.Update(order);
                    AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                    UpdateOrderInListView(order);
                    MessageBox.Show("Giảm giá thành công!");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn đơn hàng nào!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        private void btnTime_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedItem != null)
            {
                var order = AnhnguyenclaypotDbContext.INSTANCE.Orders.Find(((Order)lvOrders.SelectedItem).OrderId);
                if (order != null)
                {
                    if (order.InTime == null)
                    {
                        if (MessageBox.Show("Cập nhật giờ vào?", "Giờ vào", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                        {
                            order.InTime = TimeOnly.FromDateTime(DateTime.Now);
                            AnhnguyenclaypotDbContext.INSTANCE.Update(order);
                            AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                            UpdateOrderInListView(order);
                            MessageBox.Show("Cập nhật giờ vào thành công!");
                        }
                    }
                    else if (order.OutTime == null)
                    {
                        if (MessageBox.Show("Cập nhật giờ ra?", "Giờ ra", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                        {
                            order.OutTime = TimeOnly.FromDateTime(DateTime.Now);
                            AnhnguyenclaypotDbContext.INSTANCE.Update(order);
                            AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                            UpdateOrderInListView(order);
                            MessageBox.Show("Cập nhật giờ ra thành công!");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn đơn hàng nào!","",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
        }

        private void UpdateOrderInListView(Order order)
        {
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
        }
    }
}
