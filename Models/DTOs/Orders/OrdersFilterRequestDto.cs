using be_atoutmajeur.Models.Enums.OrderStatus;

namespace be_atoutmajeur.Models.DTOs.Orders;

public class OrdersFilterRequestDto
{
    public string? OrderRef { get; set; }
    public string? GarmentType { get; set; }
    public OrderStatus? Status { get; set; }
    public bool Days { get; set; } = false;
    public bool Week { get; set; } = false;
    public bool Month { get; set; } = false;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}