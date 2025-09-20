using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using be_atoutmajeur.Models.Entities.Comment;
using be_atoutmajeur.Models.Entities.User;
using be_atoutmajeur.Models.Enums.OrderStatus;

namespace be_atoutmajeur.Models.Entities.Orders;

[Table("orders")]
public class OrdersEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    [Column("order_number")]
    public string OrderRef { get; set; } = string.Empty;
    
    [Required]
    [ForeignKey("User")]
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("estimated_date")]
    public DateTime? EstimatedDate { get; set; }
    
    [Required]
    [Column("status")]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    
    [Column("total_price", TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }
    
    [Column("number_items")]
    public int NumberItems { get; set; }
    
    [StringLength(500)]
    [Column("customer_reason")]
    public string CustomerReason { get; set; } = string.Empty;
    
    [StringLength(1000)]
    [Column("customer_comment")]
    public string CustomerComment { get; set; } = string.Empty;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Relations
    public virtual UserEntity User { get; set; } = null!;
    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
}