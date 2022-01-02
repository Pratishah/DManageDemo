using System;

namespace DManage.ViewModels
{
    public class PlaceorderView
    {
        public Guid OrderedProductID { get; set; }
        public string OrderType { get; set; }
        public int Quantity { get; set; }
        public Guid CustomerID { get; set; }
    }
}