using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Entities;

public class RoleCode
{
    [Key]
    public int Id { get; set; }
    public string RoleName { get; set; }
    public ICollection<User>? Users { get; set; }
}
