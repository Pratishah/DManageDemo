using DManage.Models;
using DManage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Repository.Services
{
    public interface ISystemManagement
    {

        public Task<ProductType> CreateProductType(string productName);
        public Task<Pallate> EditPallateCapacity(Guid pallateID , int newcapacity);
        public Task<Node> DefineNode(string nodeName, string zone);
        public Task<Pallate> CreatePallate(PallateViewModel pallate);





    }
}
