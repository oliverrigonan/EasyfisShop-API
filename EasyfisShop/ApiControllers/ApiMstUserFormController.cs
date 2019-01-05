using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace EasyfisShop.ApiControllers
{
    public class ApiMstUserFormController : ApiController
    {
        // ============
        // Data Context
        // ============
        public Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ========================
        // List - Current User Form
        // ========================
        [Authorize, HttpGet, Route("api/userForm/currentForm/{formName}")]
        public Entities.MstUserForm CurrentUserForm(String formName)
        {
            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
            if (currentUser.Any())
            {
                var userForms = from d in db.MstUserForms.OrderByDescending(d => d.Id)
                                where d.UserId == currentUser.FirstOrDefault().Id
                                && d.SysForm.FormName.Equals(formName)
                                select new Entities.MstUserForm
                                {
                                    Id = d.Id,
                                    UserId = d.UserId,
                                    FormId = d.FormId,
                                    CanAdd = d.CanAdd,
                                    CanEdit = d.CanEdit,
                                    CanDelete = d.CanDelete,
                                    CanLock = d.CanLock,
                                    CanUnlock = d.CanUnlock,
                                    CanCancel = d.CanCancel,
                                    CanPrint = d.CanPrint
                                };

                return userForms.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}
