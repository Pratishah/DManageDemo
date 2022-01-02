using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.Repository.Services
{
    interface ISystemManagement
    {
        public int ProductTypeID { get; set; }
        public string TypeName { get; set; }

        public void DefineProductType() { }
        public void DefinePallateCapacity() { }

        public void DefinePallateType() { }
        public void DefineNode() { }





    }
}
