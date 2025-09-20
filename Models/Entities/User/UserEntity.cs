using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using be_atoutmajeur.Models.Entities.Comment;
using be_atoutmajeur.Models.Entities.Orders;
using be_atoutmajeur.Models.Enums.Roles;

namespace be_atoutmajeur.Models.Entities.User;

[Table("users")]
public class UserEntity
{
    [Key] 
    [Column("id")] 
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    [Column("first_name")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500)]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    [Column("role")]
    public Roles Role { get; set; } = Roles.User;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("last_login")]
    public DateTime? LastLogin { get; set; }
    
    // Relations
    [InverseProperty("User")]
    public virtual ICollection<OrdersEntity> Orders { get; set; } = new List<OrdersEntity>();
    [InverseProperty("Author")]
    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
}