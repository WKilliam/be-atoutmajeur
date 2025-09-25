using System.Security.Claims;
using be_atoutmajeur.Core;
using be_atoutmajeur.Models.DTOs.Orders;
using be_atoutmajeur.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace be_atoutmajeur.Controllers.Orders;

[ApiController]
[Route("orders/")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly OrdersServices _ordersService;

    public OrdersController(OrdersServices ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpPost("create/")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto request)
    {
        var currentUserId = User.GetUserId();
        var result = await _ordersService.CreateOrders(request, currentUserId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("all/")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var currentUserId = User.GetUserId();
        var result = await _ordersService.GetOrders(currentUserId, page, pageSize);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpPut("update/{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequestDto request)
    {
        var currentUserId = User.GetUserId();
        var userRole = User.GetUserRole();
        var result = await _ordersService.UpdateOrders(id, request, currentUserId, userRole);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("delete/{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var currentUserId = User.GetUserId();
        var result = await _ordersService.DeleteOrders(id, currentUserId);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpGet("history/")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetOrderHistory()
    {
        var currentUserId = User.GetUserId();
        var userRole = User.GetUserRole();
        var result = await _ordersService.HistoriqueOrders(currentUserId, userRole);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpPost("filter/")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> FilterOrders([FromBody] OrdersFilterRequestDto request)
    {
        var currentUserId = User.GetUserId();
        var userRole = User.GetUserRole();
        var result = await _ordersService.FilterOrders(request, currentUserId, userRole);
        return StatusCode(result.StatusCode, result);
    }
}