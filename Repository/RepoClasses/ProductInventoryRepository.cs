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
    public class ProductInventoryRepository : IProductInventoryRepository
    {
        private readonly DManageContext dmanageContext;
        readonly ILogger<ProductInventoryRepository> _log;
        public ProductInventoryRepository(DManageContext _dmanageContext, ILogger<ProductInventoryRepository> log)
        {
            _log = log;
            dmanageContext = _dmanageContext;
        }

        public async Task<int> CheckAvailableQuantity(Guid productID)
        {
            var availableQuantity = await dmanageContext.ProductInventories.Where(x => x.ProductId == productID).SumAsync(x => x.Quantity);
            return availableQuantity;

        }
    }
}

