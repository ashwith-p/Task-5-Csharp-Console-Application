using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; }=null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
