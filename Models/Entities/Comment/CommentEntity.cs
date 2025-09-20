using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using be_atoutmajeur.Models.Entities.Orders;
using be_atoutmajeur.Models.Entities.User;

namespace be_atoutmajeur.Models.Entities.Comment;

[Table("comments")]
public class CommentEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("Order")]
    [Column("order_id")]
    public int OrderId { get; set; }
    
    [Required]
    [ForeignKey("Author")]
    [Column("author_id")]
    public int AuthorId { get; set; }
    
    [Required]
    [StringLength(1000)]
    [Column("content")]
    public string Content { get; set; } = string.Empty;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Relations
    public virtual OrdersEntity Order { get; set; } = null!;
    public virtual UserEntity Author { get; set; } = null!;
}