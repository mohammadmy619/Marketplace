using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.UserManege;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Admin.Controllers
{
    public class UserManegeController : AdminBaseController
    {
        private IUserServicecs _UserServicecs;
        public UserManegeController(IUserServicecs UserServicecs)
        {
            _UserServicecs = UserServicecs;

        }

        //[HttpGet("FilterUser")]
        public async Task<IActionResult> FilterUser(FilterUserManege filter)
        {
            var res = await _UserServicecs.filterUserManege(filter);

            return View(res);
        }
        //[HttpGet("GetEditUserAdmin/{userId}")]
        public async Task<IActionResult> GetEditUserAdmin(long userId)
        {
            var res = await _UserServicecs.GetEditUserAdmin(userId);
            ViewData["Roles"] = await _UserServicecs.GetAllActiveRoles();
            if (res == null)
            {
                TempData[WarningMessage] = "حاجی ";
                return RedirectToAction("FilterUser");
                     
            } 

            return View(res);
        }
        [HttpPost("GetEditUserAdmin"),ValidateAntiForgeryToken]
        public async Task<IActionResult> GetEditUserAdmin(GetEditUser edit,IFormFile img)
        {
            var res = await _UserServicecs.EditUserFromAdmin(edit,img);
            ViewData["Roles"] = await _UserServicecs.GetAllActiveRoles();

            switch (res)
            {
                case EditUserFromAdminResult.NotFound:
                    TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                    break;
                case EditUserFromAdminResult.Success:
                    TempData[SuccessMessage] = "ویراش کاربر با موفقیت انجام شد";
                    break;
            }
            return RedirectToAction("FilterUser");
        }

        #region filter roles
        public async Task<IActionResult> FilterRoles(FilterRolesDTO filter)
        {
            return View(await _UserServicecs.FilterRoles(filter));
        }
        #endregion

        #region create Role
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            ViewData["Permissions"] = await _UserServicecs.GetAllActivePermission();

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateOrEditRoleDTO create)
        {
            ViewData["Permissions"] = await _UserServicecs.GetAllActivePermission();

            if (ModelState.IsValid)
            {
                var result = await _UserServicecs.CreateOrEditRole(create);

                switch (result)
                {
                    case CreateOrEditRoleResult.NotFound:
                        break;
                    case CreateOrEditRoleResult.NotExistPermissions:
                        TempData[WarningMessage] = "لطفا نقشی را انتخاب کنید";
                        break;
                    case CreateOrEditRoleResult.Success:
                        TempData[SuccessMessage] = "عملیات افزودن نقش با موفقیت انجام شد";
                        return RedirectToAction("FilterRoles");
                }
            }

            return View(create);
        }
        #endregion


        #region Edit Role
        [HttpGet]
        public async Task<IActionResult> EditRole(long roleId)
        {
            ViewData["Permissions"] = await _UserServicecs.GetAllActivePermission();
            ViewBag.Permissions = await _UserServicecs.GetAllActivePermission();

            return View(await _UserServicecs.GetEditRoleById(roleId));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(CreateOrEditRoleDTO create)
        {
            ViewData["Permissions"] = await _UserServicecs.GetAllActivePermission();
            ViewBag.Permissions = await _UserServicecs.GetAllActivePermission();
            if (create.SelectedPermission==null)
            {
                ViewData["Permissions"] = await _UserServicecs.GetAllActivePermission();
                ViewBag.Permissions = await _UserServicecs.GetAllActivePermission();
                TempData[WarningMessage] = "لطفا نقشی را انتخاب کنید";
                return RedirectToAction("EditRole",new { roleId = create.Id});
            }
            else if (ModelState.IsValid)
            {
                var result = await _UserServicecs.CreateOrEditRole(create);

                switch (result)
                {
                    case CreateOrEditRoleResult.NotFound:
                        TempData[WarningMessage] = "نقشی با مشخصات وارد شده یافت نشد";
                        ViewData["Permissions"] = await _UserServicecs.GetAllActivePermission();

                        ViewBag.Permissions = await _UserServicecs.GetAllActivePermission();

                        break;
                    case CreateOrEditRoleResult.NotExistPermissions:
                        TempData[WarningMessage] = "لطفا نقشی را انتخاب کنید";
                        ViewData["Permissions"] = await _UserServicecs.GetAllActivePermission();
                        ViewBag.Permissions = await _UserServicecs.GetAllActivePermission();


                        break;
                    case CreateOrEditRoleResult.Success:
                        TempData[SuccessMessage] = "عملیات ویرایش نقش با موفقیت انجام شد";
                        ViewBag.Permissions = await _UserServicecs.GetAllActivePermission();

                        return RedirectToAction("FilterRoles");
                }
            }
            ViewBag.Permissions = await _UserServicecs.GetAllActivePermission();

            ViewData["Permissions"] = await _UserServicecs.GetAllActivePermission();

            return View(create);
        }
        #endregion
        #region Delete role
        public async Task<IActionResult> DeleteRole(long roleId)
        {
            TempData[SuccessMessage] = "حذف نقش با موفقیت انجام شد";
            var res =await _UserServicecs.DeleteRole(roleId);

            if (res == false)
            {
                TempData[WarningMessage] = "حاجی ";
                return RedirectToAction("FilterRoles");

            }
            return RedirectToAction("FilterRoles");
        }
        #endregion

    }
    
}
