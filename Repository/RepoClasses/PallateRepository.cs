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
    public class PallateRepository : IPallate
    {
        private readonly DManageContext dmanageContext;
        readonly ILogger<PallateRepository> _log;
        public PallateRepository(DManageContext _dmanageContext, ILogger<PallateRepository> log)
        {
            _log = log;
            dmanageContext = _dmanageContext;
        }

        public async Task<ProductInventory> ModifyPallateQuantities(Guid pallateID, int quantity)
        {
            var inventory = dmanageContext.ProductInventories.Where(x => x.PallateId == pallateID).FirstOrDefault();
            inventory.Quantity = quantity;
            await dmanageContext.SaveChangesAsync();
            return inventory;
        }

        public async Task<Pallate> MovePallate(Guid pallateID, int nodeID)
        {
            
            

                var pallate = dmanageContext.Pallates.Where(pallate => pallate.PallateId == pallateID).FirstOrDefault();
                _log.LogInformation($"Pallate {pallateID} moved from {pallate.NodeId} to new Node : {nodeID}");
                pallate.NodeId = nodeID;
                await dmanageContext.SaveChangesAsync();

                return pallate;
            
           

        }

    }
}

