using DManage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OutOrders : ControllerBase
    {
        private readonly DManageContext dmanageContext;
        readonly ILogger<OutOrders> _log;
        public OutOrders(DManageContext dmanageContext, ILogger<OutOrders> log)
        {
            _log = log;
            this.dmanageContext = dmanageContext;
        }
        [HttpGet]
        [Route("all-outbound-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await dmanageContext.Orders.Where(x => x.OrderType == "Out").ToListAsync();
            return Ok(orders);
        }


        [HttpGet]
        [Route("get-outorders/{orderID}")]
        public async Task<IActionResult> GetOrd(Guid orderID)
        {
            var order = await dmanageContext.Orders.FirstOrDefaultAsync(x => x.OrderId == orderID);
            return Ok(order);
        }


        [HttpPost]
        [Route("accept-outorders/{orderID}")]
        public async Task<IActionResult> AcceptOrders(Guid orderID)
        {

            Order order = await dmanageContext.Orders.Include(x => x.OrderNavigation).Where(x => x.OrderId == orderID & x.OrderStatus == "Pending").FirstOrDefaultAsync();
            Guid OrderedProductID = order.OrderNavigation.ProductId;
            int orderedQuantity = order.OrderNavigation.Quantity;
            var AvailableStockQuantity = dmanageContext.ProductInventories.Where(x => x.ProductId == OrderedProductID).FirstOrDefaultAsync().Result.Quantity;
            if (AvailableStockQuantity >= orderedQuantity)
            {
                order.OrderStatus = "Accepted";
                _log.LogError($"Outorder Approved for productID : {OrderedProductID} Quantity : {orderedQuantity}");
                var updateInv = AvailableStockQuantity - orderedQuantity;
                dmanageContext.ProductInventories.Where(x => x.ProductId == OrderedProductID).FirstOrDefaultAsync().Result.Quantity = updateInv;
                await dmanageContext.SaveChangesAsync();
                return CreatedAtAction("GetOrd", new { OrderID = orderID }, order);
            }
            else
            {
                _log.LogError("error occured while verifying order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
