using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyfisShop.Entities
{
    public class TrnShopOrder
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
        public Int32 ShopOrderStatusId { get; set; }
        public String ShopOrderStatusDate { get; set; }
        public Int32 ShopGroupId { get; set; }
        public String Particulars { get; set; }
        public String Status { get; set; }
        public Boolean IsPrinted { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}