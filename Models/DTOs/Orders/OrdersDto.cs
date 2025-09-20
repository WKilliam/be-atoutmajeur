namespace be_atoutmajeur.Models.DTOs.Orders;

public class OrdersDto
{
    public int Id { get; set; }
    public string OrderRef { get; set; }
    public DateTime? EstimatedDate { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public int NumberItems { get; set; }
    public string CustomerReason { get; set; }
    public string CustomerComment { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}