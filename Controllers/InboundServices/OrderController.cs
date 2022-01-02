using DManage.Models;
using DManage.ViewModels;
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
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DManageContext dmanageContext;
        readonly ILogger<OrderController> _log;
        public OrderController(DManageContext _dmanageContext, ILogger<OrderController> log)

        {
            _log = log;
            dmanageContext = _dmanageContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var y = await dmanageContext.Orders.ToListAsync();
            return Ok(y);
        }


        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> GetOrd(Guid orderId)
        {
            var orders = await dmanageContext.Orders.FirstOrDefaultAsync(order => order.OrderId == orderId);
            return Ok(orders);
        }

        [HttpPost]
        [Route("placeOrder")]
        public async Task<IActionResult> PlaceOrder(PlaceorderView placeorderView)
        {

            Order neworder = new Order
            {
                CustomerId = placeorderView.CustomerID,
                OrderDate = DateTime.Now,
                OrderType = placeorderView.OrderType,
                OrderStatus = "Pending",
                OrderNavigation = new OrderQuantity { Id = Guid.NewGuid(), ProductId = placeorderView.OrderedProductID, Quantity = placeorderView.Quantity }
            };

            dmanageContext.Orders.Add(neworder);

            var newobj = await dmanageContext.SaveChangesAsync();
            _log.LogInformation("new order Placed : " + neworder.OrderId);
            return StatusCode(StatusCodes.Status201Created, neworder);

        }


        [HttpGet]
        [Route("verify/{orderID}")]
        public async Task<IActionResult> VerifyOrder(Guid orderID)
        {
            Dictionary<Guid, int> availablespace = new Dictionary<Guid, int>();

            Order order = await dmanageContext.Orders.Include(navigation => navigation.OrderNavigation).Where(order => order.OrderId == orderID & order.OrderStatus == "Pending").FirstOrDefaultAsync();
            int orderedQuantity = order.OrderNavigation.Quantity;
            var OrderedproductID = order.OrderNavigation.ProductId;
            var OrderedproductTypeID = dmanageContext.ProductMasters.Where(x => x.ProductId == OrderedproductID).FirstOrDefault().ProductTypeId;


            var SameTypePallateList = dmanageContext.Pallates.Where(x => x.ProductTypeId == OrderedproductTypeID);
            var joinTable = dmanageContext.Pallates.Join(dmanageContext.ProductInventories,
                                                            pallate => pallate.PallateId,
                                                            prodInv => prodInv.PallateId,
                                                            (pallate, prodInv) => new
                                                            {
                                                                pallate,
                                                                prodInv
                                                            });

            Boolean SpaceAvailable = false;

            foreach (var item in joinTable)
            {
                availablespace.Add(item.pallate.PallateId, item.prodInv.Quantity);
                if (orderedQuantity < item.prodInv.Quantity)
                    SpaceAvailable = true;
                break;

            }
            return Ok(SpaceAvailable);
        }


        [HttpPost]
        [Route("Accept-reject-order/{orderId}")]
        public async Task<IActionResult> AcceptRejectOrder(Guid orderId)
        {
            Dictionary<Guid, int> availablespace = new Dictionary<Guid, int>();

            Order x = await dmanageContext.Orders.Include(x => x.OrderNavigation).Where(x => x.OrderId == orderId & x.OrderStatus == "Pending").FirstOrDefaultAsync();
            int orderedQuantity = x.OrderNavigation.Quantity;
            var OrderedproductID = x.OrderNavigation.ProductId;
            var OrderedproductTypeID = dmanageContext.ProductMasters.Where(x => x.ProductId == OrderedproductID).FirstOrDefault().ProductTypeId;


            var SameTypePallateList = dmanageContext.Pallates.Where(pallate => pallate.ProductTypeId == OrderedproductTypeID);

            var JoinedPallateProductInventory = from pallate in dmanageContext.Pallates
                                                join Pid in dmanageContext.ProductInventories
                                                on pallate.PallateId equals Pid.PallateId
                                                where (pallate.ProductTypeId == OrderedproductTypeID)
                                                select new { ID = pallate.PallateId, quantity = Pid.Quantity };

            var OccupiedSpaceinPallate = JoinedPallateProductInventory.Sum(joinedtable => joinedtable.quantity);
            var PallateTypeCapacity = SameTypePallateList.Sum(joinedtable => joinedtable.Capacity);

            if (PallateTypeCapacity - OccupiedSpaceinPallate > orderedQuantity)
            {
                return Ok($"Order Accepted Space occupied  {OccupiedSpaceinPallate} , available space for this product type are {PallateTypeCapacity - OccupiedSpaceinPallate} in Node1 ");
            }
            else
            {
                return Ok($"Order rejected because pallate capacity is full");
            }
        }
    }
}
