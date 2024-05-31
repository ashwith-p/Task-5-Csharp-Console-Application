using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Employee
{
    public string Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public long? MobileNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateOnly JoiningDate { get; set; }

    public int RoleId { get; set; }

    public int DepartmentId { get; set; }

    public int LocationId { get; set; }

    public string? ManagerId { get; set; }

    public int? ProjectId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    public virtual Location Location { get; set; } = null!;

    public virtual Employee? Manager { get; set; }

    public virtual Project? Project { get; set; }

    public virtual Role Role { get; set; } = null!;
}
