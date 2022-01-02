using DManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Repository.Services
{
   public interface Iorder
    {

        public List<Order> GetAllOrders();
        public Order GetOrd(Guid OdrID);

        public Order PlaceOrder(Guid OrderedproductID, string orderType, Guid CustomerID, int quantity);
        // List<Order> GetAllOrders();
       
    }
}
