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
    public class SystemManagementRepository : ISystemManagement
    {
        private readonly DManageContext dmanageContext;
        readonly ILogger<ProductInventoryRepository> _log;
        public SystemManagementRepository(DManageContext _dmanageContext, ILogger<ProductInventoryRepository> log)
        {
            _log = log;
            dmanageContext = _dmanageContext;
        }

        public async Task<Pallate> CreatePallate(PallateViewModel pallate)
        {
            Pallate newpallate = new Pallate
            {
                CreateDatetime = DateTime.Now,
                PallateId = Guid.NewGuid(),
                NodeId = pallate.NodeId,
                Capacity = pallate.Capacity,
                ProductTypeId = pallate.ProductTypeId
            };

            await dmanageContext.Pallates.AddAsync(newpallate);
            return newpallate;
        }



        public async Task<ProductType> CreateProductType(string productName)
        {
            ProductType productType = new ProductType { TypeName = productName };
            dmanageContext.ProductTypes.Add(productType);
            await dmanageContext.SaveChangesAsync();
            return productType;
        }

        public async Task<Node> DefineNode(string nodeName, string zone)
        {
            Node newNode = new Node
            {
                CratedDateTime = DateTime.Now,
                NodeName = nodeName
            };
            await dmanageContext.Nodes.AddAsync(newNode);
            await dmanageContext.SaveChangesAsync();
            return newNode;
        }

        public async Task<Pallate> EditPallateCapacity(Guid pallateID, int newcapacity)
        {
            var Pallate = await dmanageContext.Pallates.Where(pallate => pallate.PallateId == pallateID).FirstOrDefaultAsync();
            return Pallate;

        }


    }
}

