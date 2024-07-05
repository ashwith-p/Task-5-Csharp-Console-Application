
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public partial class Role
{
    [Key]
    public int Id { get; set; }

    [MaxLength(70)]
    public string Name { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public int DepartmentId { get; set; }

    [ForeignKey(nameof(DepartmentId))]
    [NotMapped]
    public virtual Department Department { get; set; }=null!;

    [NotMapped]
    public virtual ICollection<Employee> Employees { get; set; } = [];
}
