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
    [Authorize]
    public class ApiMstShopGroupController : ApiController
    {
        // ============
        // Data Context
        // ============
        public Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===============
        // List Shop Group
        // ===============
        [Authorize, HttpGet, Route("api/shopGroup/list")]
        public List<Entities.MstShopGroup> ListShopGroup()
        {
            var shopGroups = from d in db.MstShopGroups
                             select new Entities.MstShopGroup
                             {
                                 Id = d.Id,
                                 ShopGroupCode = d.ShopGroupCode,
                                 ShopGroup = d.ShopGroup,
                                 IsLocked = d.IsLocked,
                                 CreatedBy = d.MstUser.FullName,
                                 CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                 UpdatedBy = d.MstUser1.FullName,
                                 UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                             };

            return shopGroups.OrderByDescending(d => d.Id).ToList();
        }

        // ==============
        // Add Shop Group
        // ==============
        [Authorize, HttpPost, Route("api/shopGroup/add")]
        public HttpResponseMessage AddShopGroup(Entities.MstShopGroup objShopGroup)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                var userForm = from d in db.MstUserForms where d.UserId == currentUser.FirstOrDefault().Id && d.SysForm.FormName.Equals("ShopGroupList") select d;

                if (!userForm.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No rights."; }
                else if (!userForm.FirstOrDefault().CanAdd) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "No add rights."; }
                else
                {
                    Data.MstShopGroup newShopGroup = new Data.MstShopGroup
                    {
                        ShopGroupCode = objShopGroup.ShopGroupCode,
                        ShopGroup = objShopGroup.ShopGroup,
                        IsLocked = true,
                        CreatedById = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedById = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now
                    };

                    db.MstShopGroups.InsertOnSubmit(newShopGroup);
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

        // =================
        // Update Shop Group
        // =================
        [Authorize, HttpPut, Route("api/shopGroup/update")]
        public HttpResponseMessage UpdateShopGroup(Entities.MstShopGroup objShopGroup)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                var userForm = from d in db.MstUserForms where d.UserId == currentUser.FirstOrDefault().Id && d.SysForm.FormName.Equals("ShopGroupList") select d;
                var shopGroup = from d in db.MstShopGroups where d.Id == objShopGroup.Id select d;

                if (!userForm.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No rights."; }
                else if (!userForm.FirstOrDefault().CanEdit) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "No edit rights."; }
                else if (!shopGroup.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "Reference not found."; }
                else
                {
                    var updateShopGroup = shopGroup.FirstOrDefault();
                    updateShopGroup.ShopGroupCode = objShopGroup.ShopGroupCode;
                    updateShopGroup.ShopGroup = objShopGroup.ShopGroup;
                    updateShopGroup.IsLocked = true;
                    updateShopGroup.UpdatedById = currentUser.FirstOrDefault().Id;
                    updateShopGroup.UpdatedDateTime = DateTime.Now;
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

        // =================
        // Delete Shop Group
        // =================
        [Authorize, HttpDelete, Route("api/shopGroup/delete")]
        public HttpResponseMessage DeleteShopGroup(String id)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
                var userForm = from d in db.MstUserForms where d.UserId == currentUser.FirstOrDefault().Id && d.SysForm.FormName.Equals("ShopGroupList") select d;
                var shopGroup = from d in db.MstShopGroups where d.Id == Convert.ToInt32(id) select d;

                if (!userForm.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No rights."; }
                else if (!userForm.FirstOrDefault().CanDelete) { responseStatusCode = HttpStatusCode.BadRequest; responseMessage = "No delete rights."; }
                else if (!shopGroup.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "Reference not found."; }
                else
                {
                    db.MstShopGroups.DeleteOnSubmit(shopGroup.FirstOrDefault());
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
