using DManage.Models;
using DManage.Repository.Services;
using DManage.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Repository.RepoClasses
{
    public class OrderRepository : Iorder
    {
        private readonly DManageContext dmanageContext;
        readonly ILogger<OrderRepository> _log;
        public OrderRepository(DManageContext _dmanageContext, ILogger<OrderRepository> log)

        {
            _log = log;
            dmanageContext = _dmanageContext;
        }
        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await dmanageContext.Orders.ToListAsync();
            return orders;
           
        }

        public async Task<Order> GetOrd(Guid orderId)
        {
            var order= await dmanageContext.Orders.FirstOrDefaultAsync(order => order.OrderId == orderId);
            return order;
        }

        public async Task<Order> PlaceOrder(PlaceorderView placeorderView)
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
            return neworder;

        }

        public async Task<bool> VerifyOrder(Guid orderID)
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

            bool SpaceAvailable = false;

            foreach (var item in joinTable)
            {
                availablespace.Add(item.pallate.PallateId, item.prodInv.Quantity);
                if (orderedQuantity < item.prodInv.Quantity)
                    SpaceAvailable = true;
                break;

            }

            if (SpaceAvailable == false)
            {
                _log.LogWarning($"pallate for productType {OrderedproductTypeID} is fully occupied ");
            }
            else
            {
                
                return SpaceAvailable;
            }
            return SpaceAvailable;

        }

        public async Task<bool> AcceptRejectOrder(Guid orderId)
        {
            try
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
                bool result = PallateTypeCapacity - OccupiedSpaceinPallate > orderedQuantity;
                if (result)
                {
                    _log.LogInformation($" New Order Accepted Space occupied  {OccupiedSpaceinPallate} , available space for this product type : {PallateTypeCapacity - OccupiedSpaceinPallate} in Node1 ");
                }
                else
                {

                    return false;
                }
                return result;

            }
            catch (Exception ex)
            {

                _log.LogWarning($"error occured {ex.Message}",ex);
                return false;
            }

        }
    }
}
