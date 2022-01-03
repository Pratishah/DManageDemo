using DManage.Models;
using DManage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Repository.Services
{
   public interface IPallate
    {

        public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrd(Guid OdrID);

        public Task<Order> PlaceOrder(Guid OrderedproductID, string orderType, Guid CustomerID, int quantity);
        // List<Order> GetAllOrders();


        public Task<Order> PlaceOrder(PlaceorderView placeorderView);
        public Task VerifyOrder(Guid orderID);
        public Task AcceptRejectOrder(Guid orderId);



    }
}
