using MarcketDataLayer.DTOs.Account;
using MarcketDataLayer.DTOs.UserManege;
using MarcketDataLayer.Entities.Accuont.RolePermission;
using MarcketDataLayer.Entities.Products;
using MarketPlace.DataLayer.Entities.Accuont.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Interface
{
   public interface IUserServicecs:IAsyncDisposable
    {
       
        #region account

           Task<RegisterUserResult> RegisterUser(RegisterUserDTO register);
        Task<bool> IsUserExistsByMobileNumberandEmail(string mobile, string Email);
        Task<LoginUserResult> GetUserForLogin(LoginUserDTO login);

        Task<LoginUserResult> GetUserForLoginByEmail(string Email);

        Task<User> GetUserByEmail(string mobile);
        Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDTO forgot);
        Task<RecoverUserPasswordForEmailDTO> RecoverUserPasswordForEmail(string Email);

        Task<bool> ChangeUserPassword(ChangePasswordDTO changePass, long currentUserId);
        Task<EditUserProfileDTO> GetProfileForEdit(long userId);
        Task<EditUserProfileResult> EditUserProfile(EditUserProfileDTO profile, long userId, IFormFile avatarImage);
        Task<bool> ActiveAccount(string id);


        Task<bool> IsExistProductFavorite(long productId, long userId);
        Task<bool> IsExistProductCompare(long productId, long userId);

        Task<bool> AddUserFavorite(long productId, long userId);
        Task AddUserCompare(long productId, long userId);


        Task<List<UserCompare>> GetUserCompares(long userId);
        Task<int> UserFavoritCount(long userId);
        Task<List<UserFavorite>> GetUserFavorites(long userId);
        void UpdateUserCompare(UserCompare userCompare);
        Task RemoveProductInUserCompare(long id);
        Task<UserCompare> GetUserCompare(long userId, long productId);

        Task RemoveAllRangeUserCompare(long userId);

        Task<UserComparesDTO> UserCompares(UserComparesDTO userCompares);
        Task<UserFavoritsDTO> UserFavorits(UserFavoritsDTO userFavorits, long userid);

        Task<bool> DeleteFavorite(long productid, long userId);


        #endregion
        #region
        public bool CheckPermission(long permissionId, string Email);
        Task<FilterUserManege> filterUserManege(FilterUserManege filterUser);

        Task<GetEditUser> GetEditUserAdmin(long Id);

        Task<EditUserFromAdminResult> EditUserFromAdmin(GetEditUser user, IFormFile img);

        Task<FilterRolesDTO> FilterRoles(FilterRolesDTO filter);
        Task AddPermissionToRole(List<long> selctedPermission, long roleId);
        Task RemoveAllPermissionSelectedRole(long roleId);
        Task<CreateOrEditRoleResult> CreateOrEditRole(CreateOrEditRoleDTO createOrEditRole);
        void UpdateRole(Role role);
        Task<List<Permission>> GetAllActivePermission();

        Task<List<Role>> GetAllActiveRoles();
        Task RemoveAllUserSelectedRole(long userId);
        Task AddUserToRole(List<long> selctedRole, long userId);
        Task<CreateOrEditRoleDTO> GetEditRoleById(long roleId);
        Task<bool> DeleteRole(long roleId);
        #endregion
    }
}
