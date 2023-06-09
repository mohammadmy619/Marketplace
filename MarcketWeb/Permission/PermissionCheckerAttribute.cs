using MarcketAppliction.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Permission
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        #region constractor
        private IUserServicecs _UserServicecs;
        private long _permissionId = 0;
        public PermissionCheckerAttribute(long permissionId)
        {
            _permissionId = permissionId;
        }
        #endregion

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _UserServicecs = (IUserServicecs)context.HttpContext.RequestServices.GetService(typeof(IUserServicecs));

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var Email = context.HttpContext.User.Identity.Name;

                if (!_UserServicecs.CheckPermission(_permissionId, Email))
                {
                    context.Result = new RedirectResult("/Login");
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
