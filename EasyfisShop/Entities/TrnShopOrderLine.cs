using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyfisShop.Entities
{
    public class TrnShopOrderLine
    {
        public Int32 Id { get; set; }
        public Int32 SPId { get; set; }
        public String ActivityDate { get; set; }
        public String Activity { get; set; }
        public Int32 UserId { get; set; }
        public String User { get; set; }
    }
}