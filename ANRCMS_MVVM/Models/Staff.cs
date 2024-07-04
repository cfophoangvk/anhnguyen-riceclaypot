using System;
using System.Collections.Generic;

namespace ANRCMS_MVVM.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string StaffName { get; set; } = null!;

    public string StaffPhone { get; set; } = null!;

    public int BranchId { get; set; }

    public string Password { get; set; } = null!;

    public virtual Branch Branch { get; set; } = null!;
}
