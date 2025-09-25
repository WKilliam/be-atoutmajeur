using be_atoutmajeur.Commons;
using be_atoutmajeur.Data;
using be_atoutmajeur.Models.Constant;
using be_atoutmajeur.Models.DTOs.Orders;
using be_atoutmajeur.Models.Entities.Orders;
using be_atoutmajeur.Models.Enums.OrderStatus;
using be_atoutmajeur.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace be_atoutmajeur.Services.Orders;

public class OrdersServices
{
    private readonly AppDbContext _context;

    public OrdersServices(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IApiResponse> CreateOrders(CreateOrderRequestDto request, int currentUserId)
    {
        try
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == currentUserId);
            if (!userExists)
            {
                return new ErrorResponse("User not found", 404);
            }

            var order = new OrdersEntity
            {
                UserId = currentUserId,
                TotalPrice = GarmentPricing.CalculatePrice(request.GarmentType, request.NumberItems),
                NumberItems = request.NumberItems,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrderRef = await GenerateUniqueOrderRef(),
                Status = OrderStatus.Pending,
                CustomerReason = request.CustomerReason,
                CustomerComment = request.CustomerComment,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await GetOrders(currentUserId);
        }
        catch (Exception ex)
        {
            return new ErrorResponse("Error creating order", 500, ex.Message);
        }
    }

    public async Task<IApiResponse> GetOrders(int currentUserId, int page = 1, int pageSize = 10)
    {
        try
        {
            var query = _context.Orders.Where(o => o.UserId == currentUserId);

            var totalCount = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var orderDtos = orders.Select(MapToDto).ToList();

            var pagedResult = new PagedResult<OrdersDto>
            {
                Data = orderDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return new SuccessResponse<PagedResult<OrdersDto>>(pagedResult, "Orders retrieved successfully");
        }
        catch (Exception ex)
        {
            return new ErrorResponse("Error retrieving orders", 500, ex.Message);
        }
    }

    public async Task<IApiResponse> UpdateOrders(int orderId, UpdateOrderRequestDto request, int currentUserId,
        string? userRole = null)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return new ErrorResponse("Order not found", 404);
            }

            if (order.UserId != currentUserId && userRole != "Admin")
            {
                return new ErrorResponse("Access denied", 403);
            }

            if (request.Status.HasValue)
                order.Status = request.Status.Value;
            if (request.EstimatedDate.HasValue)
                order.EstimatedDate = request.EstimatedDate.Value;
            if (!string.IsNullOrEmpty(request.Comment))
                order.CustomerComment = request.Comment;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            var targetUserId = userRole == "Admin" ? order.UserId : currentUserId;
            return await GetOrders(targetUserId);
        }
        catch (Exception ex)
        {
            return new ErrorResponse("Error updating order", 500, ex.Message);
        }
    }

    public async Task<IApiResponse> DeleteOrders(int orderId, int currentUserId)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return new ErrorResponse("Order not found", 404);
            }

            if (order.UserId != currentUserId)
            {
                return new ErrorResponse("Access denied", 403);
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return await GetOrders(currentUserId);
        }
        catch (Exception ex)
        {
            return new ErrorResponse("Error deleting order", 500, ex.Message);
        }
    }

    public async Task<IApiResponse> HistoriqueOrders(int currentUserId, string? userRole = null)
    {
        try
        {
            var query = _context.Orders.AsQueryable();

            if (userRole != "Admin")
            {
                query = query.Where(o => o.UserId == currentUserId);
            }

            var historyStatuses = new[] { OrderStatus.Completed, OrderStatus.Cancelled, OrderStatus.Delivered };
            query = query.Where(o => historyStatuses.Contains(o.Status));

            var orders = await query
                .Include(o => o.User)
                .OrderByDescending(o => o.UpdatedAt)
                .ToListAsync();

            var groupedOrders = orders
                .GroupBy(o => o.Status.ToString())
                .Select(g => new OrdersHistoriqueDto
                {
                    Category = g.Key,
                    Orders = g.Select(MapToDto).ToList(),
                    Count = g.Count(),
                    DeletionWarning = g.Key == "Cancelled"
                        ? "Les commandes annulées seront automatiquement supprimées après 72h"
                        : null
                })
                .ToList();

            return new SuccessResponse<List<OrdersHistoriqueDto>>(groupedOrders,
                "Order history retrieved successfully");
        }
        catch (Exception ex)
        {
            return new ErrorResponse("Error retrieving order history", 500, ex.Message);
        }
    }


    public async Task<IApiResponse> FilterOrders(OrdersFilterRequestDto request, int currentUserId,
        string? userRole = null)
    {
        try
        {
            var query = _context.Orders.AsQueryable();
            if (userRole != "Admin")
            {
                query = query.Where(o => o.UserId == currentUserId);
            }

            if (!string.IsNullOrEmpty(request.OrderRef))
            {
                query = query.Where(o => o.OrderRef.Contains(request.OrderRef));
            }

            if (!string.IsNullOrEmpty(request.GarmentType))
            {
                query = query.Where(o => o.CustomerReason.Contains(request.GarmentType));
            }

            if (request.Status.HasValue)
            {
                query = query.Where(o => o.Status == request.Status.Value);
            }

            var now = DateTime.UtcNow;
            if (request.Days)
            {
                var startDate = now.AddDays(-1);
                query = query.Where(o => o.CreatedAt >= startDate);
            }
            else if (request.Week)
            {
                var startDate = now.AddDays(-7);
                query = query.Where(o => o.CreatedAt >= startDate);
            }
            else if (request.Month)
            {
                var startDate = now.AddMonths(-1);
                query = query.Where(o => o.CreatedAt >= startDate);
            }

            var totalCount = await query.CountAsync();

            // Appliquer la pagination et trier
            var orders = await query
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var orderDtos = orders.Select(MapToDto).ToList();

            var pagedResult = new PagedResult<OrdersDto>
            {
                Data = orderDtos,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return new SuccessResponse<PagedResult<OrdersDto>>(pagedResult, "Filtered orders retrieved successfully");
        }
        catch (Exception ex)
        {
            return new ErrorResponse("Error filtering orders", 500, ex.Message);
        }
    }


    private OrdersDto MapToDto(OrdersEntity order)
        {
            return new OrdersDto
            {
                Id = order.Id,
                OrderRef = order.OrderRef,
                EstimatedDate = order.EstimatedDate,
                Status = order.Status.ToString(),
                TotalPrice = order.TotalPrice,
                NumberItems = order.NumberItems,
                CustomerReason = order.CustomerReason,
                CustomerComment = order.CustomerComment,
                CreatedAt = order.CreatedAt,
            };
        }

        private async Task<string> GenerateUniqueOrderRef()
        {
            string orderRef;
            bool isDuplicate;
            int attempts = 0;
            const int maxAttempts = 10;
            do
            {
                orderRef = OrderRefGenerator.GenerateOrderRefPattern();
                isDuplicate = await _context.Orders
                    .AnyAsync(o => o.OrderRef == orderRef);
                attempts++;
                if (attempts >= maxAttempts)
                {
                    orderRef = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
                    break;
                }
            } while (isDuplicate);

            return orderRef;
        }
    }