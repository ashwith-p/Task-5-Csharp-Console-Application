using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class RoleDetail
{
    public int? RoleId { get; set; }

    public int? LocationId { get; set; }

    public virtual Location? Location { get; set; }

    public virtual Role? Role { get; set; }
}
