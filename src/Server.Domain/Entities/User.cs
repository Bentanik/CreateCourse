using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? RefreshToken { get; set; }
    public bool Active { get; set; }
    public int RoleCodeId { get; set; }
    [ForeignKey("RoleCodeId")]
    public RoleCode RoleCode { get; set; }
}
