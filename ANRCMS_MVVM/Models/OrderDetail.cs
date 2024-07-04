﻿using System;
using System.Collections.Generic;

namespace ANRCMS_MVVM.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int FoodId { get; set; }

    public int Quantity { get; set; }

    public virtual Food Food { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
