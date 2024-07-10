using System;
using System.Collections.Generic;

namespace ANRCMS_MVVM.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int BranchId { get; set; }

    public int? Discount { get; set; }

    public short Status { get; set; }

    public DateOnly? OrderDate { get; set; }

    public TimeOnly? InTime { get; set; }

    public TimeOnly? OutTime { get; set; }

    public int TotalPrice { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public string StatusTitle
    {
        get
        {
            switch (Status)
            {
                case 1: return "Đang xử lý";
                case 2: return "Đang giao hàng";
                case 3: return "Chưa thanh toán";
                case 4: return "Đã thanh toán";
                default: return "Không có thông tin";
            }
        }
    }

    public int ActualPrice
    {
        get => Discount != null ? TotalPrice - (int) Discount : TotalPrice;
    }
}
