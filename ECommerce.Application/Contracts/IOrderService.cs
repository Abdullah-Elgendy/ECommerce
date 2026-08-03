using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUserAsync(string email, CancellationToken ct = default);
        Task<Result<OrderToReturnDto>> GetOrderForUserByIdAndEmailAsync(Guid id, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default);
    }
}
