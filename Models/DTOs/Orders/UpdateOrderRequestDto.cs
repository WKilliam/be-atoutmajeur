using be_atoutmajeur.Models.Enums.OrderStatus;

namespace be_atoutmajeur.Models.DTOs.Orders;

public class UpdateOrderRequestDto
{
    public OrderStatus? Status { get; set; }
    public DateTime? EstimatedDate { get; set; }
    public string? Comment { get; set; }
}