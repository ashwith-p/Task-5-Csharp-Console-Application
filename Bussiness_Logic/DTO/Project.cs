
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public partial class Project
{
    [Key]
    public int Id { get; set; }

    [MaxLength(70)]
    public string Name { get; set; } = null!;

    [NotMapped]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
