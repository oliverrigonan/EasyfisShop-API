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
    public class ApiTrnShopOrderLineController : ApiController
    {
        // ============
        // Data Context
        // ============
        public Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ====================
        // List Shop Order Line
        // ====================
        [Authorize, HttpGet, Route("api/shopOrderLine/list/{SPId}")]
        public List<Entities.TrnShopOrderLine> ListShopOrderLine(String SPId)
        {
            var shopOrderLines = from d in db.TrnShopOrderLines
                                 where d.SPId == Convert.ToInt32(SPId)
                                 select new Entities.TrnShopOrderLine
                                 {
                                     Id = d.Id,
                                     ActivityDate = d.ActivityDate.ToShortDateString(),
                                     Activity = d.Activity,
                                     User = d.MstUser.FullName
                                 };

            return shopOrderLines.ToList();
        }

        // ===================
        // Add Shop Order Line
        // ===================
        [Authorize, HttpPost, Route("api/shopOrderLine/add")]
        public HttpResponseMessage AddShopOrderLine(Entities.TrnShopOrderLine objShopOrderLine)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                var userForm = from d in db.MstUserForms where d.UserId == currentUser.FirstOrDefault().Id && d.SysForm.FormName.Equals("ShopOrderDetail") select d;
                var shopOrder = from d in db.TrnShopOrders where d.Id == objShopOrderLine.SPId select d;

                if (!userForm.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No rights."; }
                else if (!userForm.FirstOrDefault().CanAdd) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "No add rights."; }
                else if (shopOrder.FirstOrDefault().IsLocked) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "Add is not allowed if the current document is locked."; }
                else
                {
                    Data.TrnShopOrderLine newShopOrderLine = new Data.TrnShopOrderLine
                    {
                        SPId = objShopOrderLine.SPId,
                        ActivityDate = Convert.ToDateTime(objShopOrderLine.ActivityDate),
                        Activity = objShopOrderLine.Activity,
                        UserId = currentUser.FirstOrDefault().Id
                    };

                    db.TrnShopOrderLines.InsertOnSubmit(newShopOrderLine);
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

        // ======================
        // Update Shop Order Line
        // ======================
        [Authorize, HttpPut, Route("api/shopOrderLine/update")]
        public HttpResponseMessage UpdateShopOrderLine(Entities.TrnShopOrderLine objShopOrderLine)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                var userForm = from d in db.MstUserForms where d.UserId == currentUser.FirstOrDefault().Id && d.SysForm.FormName.Equals("ShopOrderDetail") select d;
                var shopOrderLine = from d in db.TrnShopOrderLines where d.Id == objShopOrderLine.Id select d;

                if (!userForm.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No rights."; }
                else if (!userForm.FirstOrDefault().CanEdit) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "No edit rights."; }
                else if (!shopOrderLine.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "Reference not found."; }
                else if (shopOrderLine.FirstOrDefault().TrnShopOrder.IsLocked) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "Update is not allowed if the current document is locked."; }
                else
                {
                    var updateShopOrderLine = shopOrderLine.FirstOrDefault();
                    updateShopOrderLine.ActivityDate = Convert.ToDateTime(objShopOrderLine.ActivityDate);
                    updateShopOrderLine.Activity = objShopOrderLine.Activity;
                    updateShopOrderLine.UserId = objShopOrderLine.UserId;
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

        // ======================
        // Delete Shop Order Line
        // ======================
        [Authorize, HttpDelete, Route("api/shopOrderLine/update")]
        public HttpResponseMessage DeleteShopOrderLine(String id)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                var userForm = from d in db.MstUserForms where d.UserId == currentUser.FirstOrDefault().Id && d.SysForm.FormName.Equals("ShopOrderDetail") select d;
                var shopOrderLine = from d in db.TrnShopOrderLines where d.Id == Convert.ToInt32(id) select d;

                if (!userForm.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No rights."; }
                else if (!userForm.FirstOrDefault().CanDelete) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "No delete rights."; }
                else if (!shopOrderLine.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "Reference not found."; }
                else if (shopOrderLine.FirstOrDefault().TrnShopOrder.IsLocked) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "Delete is not allowed if the current document is locked."; }
                else
                {
                    db.TrnShopOrderLines.DeleteOnSubmit(shopOrderLine.FirstOrDefault());
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
