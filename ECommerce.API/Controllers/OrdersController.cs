using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region POST CreateOrder
        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto, CancellationToken ct = default)
        {
            return ToActionResult(await _orderService.CreateOrderAsync(orderDto, GetEmailFromToken(), ct));
        }
        #endregion

        #region GET GetAllOrders
        [Authorize]
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders(CancellationToken ct = default)
        => ToActionResult(await _orderService.GetAllOrdersForUserAsync(GetEmailFromToken(), ct));
        #endregion

        #region GET GetOrder
        [Authorize]
        [HttpGet("GetOrder/{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrder(Guid id, CancellationToken ct = default)
        => ToActionResult(await _orderService.GetOrderForUserByIdAndEmailAsync(id, GetEmailFromToken(), ct));
        #endregion

        #region GET GetDeliveryMethods
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethods(CancellationToken ct = default)
        => ToActionResult(await _orderService.GetAllDeliveryMethodsAsync(ct));
        #endregion
    }
}
