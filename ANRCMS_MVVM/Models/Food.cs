using System;
using System.Collections.Generic;

namespace ANRCMS_MVVM.Models;

public partial class Food
{
    public int FoodId { get; set; }

    public string FoodVietnameseName { get; set; } = null!;

    public string FoodEnglishName { get; set; } = null!;

    public int FoodPrice { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public string ImageUrl
    {
        get => "Images/" + FoodId + ".png";
    }
}
