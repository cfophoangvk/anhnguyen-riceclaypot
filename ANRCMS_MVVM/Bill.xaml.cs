using ANRCMS_MVVM.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Windows;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for Bill.xaml
    /// </summary>
    public partial class Bill : Window
    {
        public Bill(int orderId)
        {
            InitializeComponent();

            Order currentOrder = AnhnguyenclaypotDbContext.INSTANCE.Orders.First(x => x.OrderId == orderId);
            tbBillNumber.Text = orderId.ToString("D6");
            tbBillDate.Text = currentOrder.OrderDate.ToString("dd/MM/yyyy");

            if (currentOrder.InTime != null && currentOrder.OutTime != null)
            {
                gridBillTime.Visibility = Visibility.Visible;
                tbBillInTime.Text = currentOrder.InTime?.ToString("HH:mm");
                tbBillOutTime.Text = currentOrder.OutTime?.ToString("HH:mm");
            }

            dgBillOrderDetail.ItemsSource = AnhnguyenclaypotDbContext.INSTANCE.OrderDetails.Include(x => x.Food)
                                                                                           .Where(x => x.OrderId == orderId)
                                                                                           .AsEnumerable()
                                                                                           .Select((x, i) => new
                                                                                           {
                                                                                               BillID = i + 1,
                                                                                               x.Food.FoodVietnameseName,
                                                                                               x.Quantity,
                                                                                               x.Food.FoodPrice,
                                                                                               x.TotalAmount
                                                                                           })
                                                                                           .ToList();
            tbBillTotalPrice.Text = currentOrder.TotalPrice.ToString("N3") + " ₫";
            tbBillDiscount.Text = currentOrder.Discount?.ToString("N3") + " ₫";
            tbBillActualPrice.Text = (currentOrder.TotalPrice - currentOrder.Discount)?.ToString("N3") + " ₫";

            var orderBranch = AnhnguyenclaypotDbContext.INSTANCE.Branches.Find(currentOrder.BranchId);
            if (orderBranch != null)
            {
                tbBranchID.Text = orderBranch.BranchId.ToString();
                tbBranchName.Text = orderBranch.BranchName.ToString();
            }
        }
    }
}
