using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyfisShop.Entities
{
    public class RepOrderSummaryReport
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public String SPNumber { get; set; }
        public String SPDate { get; set; }
        public Int32 ItemId { get; set; }
        public String Item { get; set; }
        public Decimal Quantity { get; set; }
        public Int32 UnitId { get; set; }
        public Decimal Amount { get; set; }
        public String ShopGroup { get; set; }
        public String ShopOrderStatus { get; set; }
        public String ShopOrderStatusDate { get; set; }
        public String Particulars { get; set; }
    }
}