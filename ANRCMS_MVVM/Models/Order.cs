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

    public short TotalPrice { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
