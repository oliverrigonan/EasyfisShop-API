using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace EasyfisShop.ApiControllers
{
    public class ApiRepOrderSummaryReportController : ApiController
    {
        // ============
        // Data Context
        // ============
        public Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =========================================
        // Dropdown List - Shop Order Status (Field)
        // =========================================
        [Authorize, HttpGet, Route("api/orderSummaryReport/dropdown/list/shopOrderStatus")]
        public List<Entities.MstShopOrderStatus> DropdownListOrderSummaryReportShopOrderStatus()
        {
            var shopOrderStatuses = from d in db.MstShopOrderStatus
                                    select new Entities.MstShopOrderStatus
                                    {
                                        Id = d.Id,
                                        ShopOrderStatusCode = d.ShopOrderStatusCode,
                                        ShopOrderStatus = d.ShopOrderStatus
                                    };

            return shopOrderStatuses.OrderByDescending(d => d.Id).ToList();
        }

        // ========================================
        // Dropdown List - Shop Order Group (Field)
        // ========================================
        [Authorize, HttpGet, Route("api/orderSummaryReport/dropdown/list/shopOrderGroup")]
        public List<Entities.MstShopGroup> DropdownListOrderSummaryReportShopGroup()
        {
            var shopGroups = from d in db.MstShopGroups
                             select new Entities.MstShopGroup
                             {
                                 Id = d.Id,
                                 ShopGroupCode = d.ShopGroupCode,
                                 ShopGroup = d.ShopGroup
                             };

            return shopGroups.OrderByDescending(d => d.Id).ToList();
        }

        // ===============
        // List Shop Order
        // ===============
        [Authorize, HttpGet, Route("api/orderSummaryReport/list/{startDate}/{endDate}/{shopGroupId}/{shopOrderStatusId}")]
        public List<Entities.RepOrderSummaryReport> ListOrderSummaryReport(String startDate, String endDate, String shopGroupId, String shopOrderStatusId)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
            var shopOrders = from d in db.TrnShopOrders
                             where d.BranchId == currentUser.FirstOrDefault().BranchId
                             && d.SPDate >= Convert.ToDateTime(startDate)
                             && d.SPDate <= Convert.ToDateTime(endDate)
                             && d.ShopGroupId == Convert.ToInt32(shopGroupId)
                             && d.ShopOrderStatusId == Convert.ToInt32(shopOrderStatusId)
                             select new Entities.RepOrderSummaryReport
                             {
                                 Id = d.Id,
                                 SPDate = d.SPDate.ToShortDateString(),
                                 SPNumber = d.SPNumber,
                                 Item = d.MstArticle.Article,
                                 Quantity = d.Quantity,
                                 Amount = d.Amount,
                                 Particulars = d.Particulars,
                                 ShopGroup = d.MstShopGroup.ShopGroup,
                                 ShopOrderStatus = d.MstShopOrderStatus.ShopOrderStatus,
                                 ShopOrderStatusDate = d.ShopOrderStatusDate.ToShortDateString(),
                             };

            return shopOrders.OrderByDescending(d => d.Id).ToList();
        }
    }
}
