using be_atoutmajeur.Enums;

namespace be_atoutmajeur.Models.DTOs.Orders;

public class CreateOrderRequestDto
{
    public int UserId { get; set; }
    public DateTime? EstimatedDate { get; set; }
    public int NumberItems { get; set; }
    
    public GarmentType GarmentType { get; set; }
    public string CustomerReason { get; set; } = string.Empty;
    public string CustomerComment { get; set; } = string.Empty;
}