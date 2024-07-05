
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public partial class RoleDetail
{
    public int? RoleId { get; set; }

    public int? LocationId { get; set; }

    [ForeignKey(nameof(LocationId))]
    [NotMapped]
    public virtual Location? Location { get; set; }

    [ForeignKey(nameof(RoleId))]
    [NotMapped]
    public virtual Role? Role { get; set; }
}
