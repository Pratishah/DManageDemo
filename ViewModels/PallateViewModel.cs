using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DManage.ViewModels
{
    public class PallateViewModel
    {
        public DateTime CreateDatetime { get; set; }
        public Guid PallateId { get; set; }
        public int NodeId { get; set; }
        public int Capacity { get; set; }
        public int ProductTypeId { get; set; }

    }
}
