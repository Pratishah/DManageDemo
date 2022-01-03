using DManage.Controllers;
using DManage.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Repository.Services
{
   public interface IProductInventoryRepository
    {

        public Task<int> CheckAvailableQuantity(Guid productID);
       




    }
}
