using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EasyfisShop.ApiControllers
{
    public class ApiMstShopOrderStatusController : ApiController
    {
        // ============
        // Data Context
        // ============
        public Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ======================
        // List Shop Order Status
        // ======================
        [Authorize, HttpGet, Route("api/shopOrderStatus/list")]
        public List<Entities.MstShopOrderStatus> ListShopOrderStatus()
        {
            var shopOrderStatuses = from d in db.MstShopOrderStatus
                                    select new Entities.MstShopOrderStatus
                                    {
                                        Id = d.Id,
                                        ShopOrderStatusCode = d.ShopOrderStatusCode,
                                        ShopOrderStatus = d.ShopOrderStatus,
                                        IsLocked = d.IsLocked,
                                        CreatedBy = d.MstUser.FullName,
                                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                        UpdatedBy = d.MstUser1.FullName,
                                        UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                    };

            return shopOrderStatuses.OrderByDescending(d => d.Id).ToList();
        }

        // =====================
        // Add Shop Order Status
        // =====================
        [Authorize, HttpPost, Route("api/shopOrderStatus/add")]
        public HttpResponseMessage AddShopOrderStatus(Entities.MstShopOrderStatus objShopOrderStatus)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                var userForm = from d in db.MstUserForms where d.UserId == currentUser.FirstOrDefault().Id && d.SysForm.FormName.Equals("ShopOrderStatusList") select d;

                if (!userForm.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No rights."; }
                else if (!userForm.FirstOrDefault().CanAdd) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "No add rights."; }
                else
                {
                    Data.MstShopOrderStatus newShopOrderStatus = new Data.MstShopOrderStatus
                    {
                        ShopOrderStatusCode = objShopOrderStatus.ShopOrderStatusCode,
                        ShopOrderStatus = objShopOrderStatus.ShopOrderStatus,
                        IsLocked = true,
                        CreatedById = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedById = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now
                    };

                    db.MstShopOrderStatus.InsertOnSubmit(newShopOrderStatus);
                    db.SubmitChanges();
                }

                return Request.CreateResponse(responseStatusCode, responseMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // ========================
        // Update Shop Order Status
        // ========================
        [Authorize, HttpPut, Route("api/shopOrderStatus/update")]
        public HttpResponseMessage UpdateShopOrderStatus(Entities.MstShopOrderStatus objShopOrderStatus)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                var userForm = from d in db.MstUserForms where d.UserId == currentUser.FirstOrDefault().Id && d.SysForm.FormName.Equals("ShopOrderStatusList") select d;
                var shopOrderStatus = from d in db.MstShopOrderStatus where d.Id == objShopOrderStatus.Id select d;

                if (!userForm.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No rights."; }
                else if (!userForm.FirstOrDefault().CanEdit) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "No edit rights."; }
                else if (!shopOrderStatus.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "Reference not found."; }
                else
                {
                    var updateShopOrderStatus = shopOrderStatus.FirstOrDefault();
                    updateShopOrderStatus.ShopOrderStatusCode = objShopOrderStatus.ShopOrderStatusCode;
                    updateShopOrderStatus.ShopOrderStatus = objShopOrderStatus.ShopOrderStatus;
                    updateShopOrderStatus.IsLocked = true;
                    updateShopOrderStatus.UpdatedById = currentUser.FirstOrDefault().Id;
                    updateShopOrderStatus.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();
                }

                return Request.CreateResponse(responseStatusCode, responseMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // ========================
        // Delete Shop Order Status
        // ========================
        [Authorize, HttpDelete, Route("api/shopOrderStatus/delete")]
        public HttpResponseMessage DeleteShopOrderStatus(String id)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                var userForm = from d in db.MstUserForms where d.UserId == currentUser.FirstOrDefault().Id && d.SysForm.FormName.Equals("ShopOrderStatusList") select d;
                var shopOrderStatus = from d in db.MstShopOrderStatus where d.Id == Convert.ToInt32(id) select d;

                if (!userForm.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No rights."; }
                else if (!userForm.FirstOrDefault().CanDelete) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "No delete rights."; }
                else if (!shopOrderStatus.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "Reference not found."; }
                else
                {
                    db.MstShopOrderStatus.DeleteOnSubmit(shopOrderStatus.FirstOrDefault());
                    db.SubmitChanges();
                }

                return Request.CreateResponse(responseStatusCode, responseMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
