using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyfisShop.Entities
{
    public class MstItem
    {
        public Int32 Id { get; set; }
        public String Item { get; set; }
        public String Code { get; set; }
        public String ManualCode { get; set; }
        public Int32 UnitId { get; set; }
    }
}