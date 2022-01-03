﻿using DManage.Controllers;
using DManage.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Repository.Services
{
   public interface IPallate
    {

        public Task<Models.ProductInventory> ModifyPallateQuantities(Guid pallateID, int quantity);
        public Task<Pallate> MovePallate(Guid pallateID, int nodeID);




    }
}
