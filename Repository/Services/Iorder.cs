using DManage.Models;
using DManage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Repository.Services
{
   public interface Iorder
    {

        public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrd(Guid OdrID);



        public Task<Order> PlaceOrder(PlaceorderView placeorderView);
        public Task<bool> VerifyOrder(Guid orderID);
        public Task<bool> AcceptRejectOrder(Guid orderId);



    }
}
