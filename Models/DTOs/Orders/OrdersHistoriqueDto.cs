namespace be_atoutmajeur.Models.DTOs.Orders;

public class OrdersHistoriqueDto
{
    public string Category { get; set; } = string.Empty;
    public List<OrdersDto> Orders { get; set; } = new List<OrdersDto>();
    public int Count { get; set; }
    public string? DeletionWarning { get; set; }
}