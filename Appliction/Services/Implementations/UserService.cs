using MarcketAppliction.Extensions;
using MarcketAppliction.Services.Interface;
using MarcketAppliction.Utils;
using MarcketDataLayer.DTOs.Account;
using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.DTOs.UserManege;
using MarcketDataLayer.Entities.Accuont.RolePermission;
using MarcketDataLayer.Entities.Products;
using MarketPlace.DataLayer.Entities.Accuont.User;
using MarketPlace.DataLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Implementations
{
    public class UserService : IUserServicecs
    {
        #region Cunstractor
        private IGenericRepository<User> _UserRepository;

        private readonly IPasswordHelper _passwordHelper;
        private IGenericRepository<Role> _RoleRepository;
        private IGenericRepository<RolePermission> _RolePermissionRepository;
        private IGenericRepository<UserRole> _UserRoleRepository;
        private IGenericRepository<Permission> _PermissionRepository;
        private IGenericRepository<UserFavorite> _UserFavoritenRepository;
        private IGenericRepository<UserCompare> _UserCompareRepository;



        public UserService(IGenericRepository<User> UserRepository, IPasswordHelper passwordHelper, IGenericRepository<Role> RoleRepository, IGenericRepository<RolePermission> RolePermissionRepository, IGenericRepository<UserRole> UserRoleRepository, IGenericRepository<Permission> PermissionRepository, IGenericRepository<UserFavorite> UserFavoritenRepository, IGenericRepository<UserCompare> UserCompareRepository)
        {
            _UserRepository = UserRepository;
            _passwordHelper = passwordHelper;
            _RoleRepository = RoleRepository;
            _RolePermissionRepository = RolePermissionRepository;
            _UserRoleRepository = UserRoleRepository;
            _PermissionRepository = PermissionRepository;
            _UserFavoritenRepository = UserFavoritenRepository;
            _UserCompareRepository = UserCompareRepository;
        }


        #endregion
        #region account

        public async Task<RegisterUserResult> RegisterUser(RegisterUserDTO register)
        {
            if (!await IsUserExistsByMobileNumberandEmail(register.Mobile,register.Email))
            {
                var user = new User
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Mobile = register.Mobile,
                    Password = _passwordHelper.EncodePasswordMd5(register.Password),
                    MobileActiveCode = new Random().Next(10000, 999999).ToString(),
                    EmailActiveCode = Guid.NewGuid().ToString("N"),
                    Avatar = "avatar.jpg",
                    Email=register.Email,
                    IsMobileActive=true,

                };

                await _UserRepository.AddEntity(user);
                await _UserRepository.SaveChanges();
                // todo: send activation mobile code to user


                return RegisterUserResult.Success;
            }

            return RegisterUserResult.MobileExists;
        }


        public async Task<bool> IsUserExistsByMobileNumberandEmail(string mobile,string Email)
        {
            return await _UserRepository.GetQuery().AsQueryable().AnyAsync(s => s.Mobile == mobile||s.Email==Email);
        }

        public async Task<LoginUserResult> GetUserForLogin(LoginUserDTO login)
        {
            var user = await _UserRepository.GetQuery().AsQueryable().Where(x=>x.Email == login.Email).SingleOrDefaultAsync();
            if (user == null) return LoginUserResult.NotFound;
            if (!user.IsEmailActive) return LoginUserResult.NotActivated;
            if (user.IsBlocked==true) return LoginUserResult.NotActivated;

            if (user.Password != _passwordHelper.EncodePasswordMd5(login.Password)) return LoginUserResult.NotFound;
            return LoginUserResult.Success;
        }
        public async Task<LoginUserResult> GetUserForLoginByEmail(string Email)
        {
            var user = await _UserRepository.GetQuery().AsQueryable().Where(x => x.Email == Email).SingleOrDefaultAsync();
            if (user == null) return LoginUserResult.NotFound;
            if (!user.IsEmailActive) return LoginUserResult.NotActivated;
            if (user.IsBlocked == true) return LoginUserResult.NotActivated;

            
            return LoginUserResult.Success;
        }



        public async Task<User> GetUserByEmail(string Email)
        {
            return await _UserRepository.GetQuery().Where(x=>x.Email==Email).AsQueryable().SingleOrDefaultAsync();
        }
        public async Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDTO forgot)
        {
            var user = await _UserRepository.GetQuery().SingleOrDefaultAsync(s => s.Email == forgot.Email);
            if (user == null) return ForgotPasswordResult.NotFound;
            var newPassword = new Random().Next(1000000, 999999999).ToString();
            user.Password = _passwordHelper.EncodePasswordMd5(newPassword);
            
            _UserRepository.EditEntity(user);
            // todo: send new password to user with sms

            await _UserRepository.SaveChanges();

            return ForgotPasswordResult.Success;
        }
       public async Task<RecoverUserPasswordForEmailDTO> RecoverUserPasswordForEmail(string Email)
        {
            var user = await _UserRepository.GetQuery().Where(s => s.Email == Email).SingleOrDefaultAsync();
            var newPassword = new Random().Next(1000000, 999999999).ToString();
            user.Password = _passwordHelper.EncodePasswordMd5(newPassword);
            _UserRepository.EditEntity(user);
            await _UserRepository.SaveChanges();
            return new RecoverUserPasswordForEmailDTO
            {
                Password = newPassword,
                USerName = user.FirstName
            };
        }

        public async Task<bool> ActiveAccount(string id)
        {
            var user =await _UserRepository.GetQuery().SingleOrDefaultAsync(u => u.EmailActiveCode == id);
            if (user == null || user.IsEmailActive)
                return false;

            user.IsEmailActive = true;
            user.EmailActiveCode =Guid.NewGuid().ToString();

            _UserRepository.EditEntity(user);

           await _UserRepository.SaveChanges();

            return true;
        }

        public async Task<bool> ChangeUserPassword(ChangePasswordDTO changePass, long currentUserId)
        {
            var user = await _UserRepository.GetEntityById(currentUserId);
            if (user != null)
            {
                var newPassword = _passwordHelper.EncodePasswordMd5(changePass.NewPassword);
                if (newPassword != user.Password)
                {
                    user.Password = newPassword;
                    _UserRepository.EditEntity(user);
                    await _UserRepository.SaveChanges();

                    return true;
                }
            }

            return false;
        }


        public async Task<EditUserProfileDTO> GetProfileForEdit(long userId)
        {
            var user = await _UserRepository.GetEntityById(userId);
            if (user == null) return null;

            return new EditUserProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar
            };
        }

        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDTO profile, long userId, IFormFile avatarImage)
        {
            var user = await _UserRepository.GetEntityById(userId);
            if (user == null) return EditUserProfileResult.NotFound;

            if (user.IsBlocked) return EditUserProfileResult.IsBlocked;
            if (!user.IsMobileActive) return EditUserProfileResult.IsNotActive;

            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;

            if (avatarImage != null && avatarImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatarImage.FileName);
                avatarImage.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer, 100, 100, PathExtension.UserAvatarThumbServer, user.Avatar);
                user.Avatar = imageName;
            }

            _UserRepository.EditEntity(user);
            await _UserRepository.SaveChanges();

            return EditUserProfileResult.Success;
        }



        public async Task<bool> IsExistProductFavorite(long productId, long userId)
        {
            return await _UserFavoritenRepository.GetQuery().AsQueryable()
               .AnyAsync(c => c.ProductId == productId && c.UserId == userId &&c.IsDelete==false );
        }
       

        public async Task<bool> AddUserFavorite(long productId, long userId)
        {
            if(await IsExistProductFavorite(productId,userId)==false)
            {
                var Favorite = new UserFavorite
                {
                    ProductId = productId,
                    UserId = userId,
                    IsDelete=false,
                    CreateDate=DateTime.Now,
                    

                };

                await _UserFavoritenRepository.AddEntity(Favorite);
                await _UserFavoritenRepository.SaveChanges();
                return true;
            }
            return false;
          
        }

        public async Task<bool> IsExistProductCompare(long productId, long userId)
        {
            return await _UserCompareRepository.GetQuery().AsQueryable()
                .Where(c => !c.IsDelete && c.ProductId == productId && c.UserId == userId)
                .AnyAsync();
        }

        public async Task AddUserCompare(long productId, long userId)
        {
            //await _UserCompareRepository.AddEntity(userCompare);
            //await _UserCompareRepository.SaveChanges();

            
        }

        public async Task<List<UserCompare>> GetUserCompares(long userId)
        {
            return await _UserCompareRepository.GetQuery().Include(c => c.Product).AsQueryable()
                .Where(c => c.UserId == userId && !c.IsDelete)
                .ToListAsync();
        }

        public async Task<int> UserFavoritCount(long userId)
        {
            var count = await _UserFavoritenRepository.GetQuery()
                .Where(c => c.UserId == userId &&c.IsDelete==false).AsNoTracking()
                .CountAsync();

            if (count == null)
            {
                return 0;
            }
            return count;
        }

        public async Task<List<UserFavorite>> GetUserFavorites(long userId)
        {
            return await _UserFavoritenRepository.GetQuery().Include(c => c.Product).AsQueryable()
                .Where(c => c.UserId == userId).AsNoTracking()
                .ToListAsync();
        }

        public void UpdateUserCompare(UserCompare userCompare)
        {
            _UserCompareRepository.EditEntity(userCompare);

            _UserCompareRepository.SaveChanges();

        }

        public async Task<UserCompare> GetUserCompare(long userId, long productId)
        {
            return await _UserCompareRepository.GetQuery().AsQueryable()
                .Where(c => c.UserId == userId && c.ProductId == productId).AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<UserComparesDTO> UserCompares(UserComparesDTO userCompares)
        {
            var query = _UserCompareRepository.GetQuery().Include(c => c.Product).ThenInclude(c => c.ProductFeatures).AsNoTracking().AsQueryable();



            #region paging
            var pager = Pager.Build(userCompares.PageId, await query.CountAsync(), userCompares.TakeEntity, userCompares.HowManyShowPageAfterAndBefore);
            var allData = await query.Paging(pager).Where(c => !c.IsDelete).ToListAsync();
            #endregion

            return userCompares.SetPaging(pager).SetCompares(allData);
        }

        public async Task<UserFavoritsDTO> UserFavorits(UserFavoritsDTO userFavorits,long userid)
        {
            var query = _UserFavoritenRepository.GetQuery().Where(s=>s.UserId==userid &&s.IsDelete==false)
                .Include(c => c.Product).ThenInclude(s=>s.ProductDiscounts.Where(x=>x.ExpireDate > DateTime.Now))
                .ThenInclude(x=>x.ProductDiscountUses).AsNoTracking().AsQueryable();



            #region paging
            var pager = Pager.Build(userFavorits.PageId, await query.CountAsync(), userFavorits.TakeEntity, userFavorits.HowManyShowPageAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();
            #endregion

            return userFavorits.SetPaging(pager).SetFavorites(allData);
        }

        public async Task RemoveProductInUserCompare(long id)
        {
            var currentUserCompare = await _UserCompareRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(c => c.Id == id);


            if (currentUserCompare != null)
            {

                _UserCompareRepository.DeleteEntity(currentUserCompare);
            }
            await _UserCompareRepository.SaveChanges();

        }

        public async Task RemoveAllRangeUserCompare(long userId)
        {
            var data = await _UserCompareRepository.GetQuery().Where(c => c.UserId == userId).ToListAsync();


            if (data != null && data.Any())
            {
                _UserCompareRepository.ISDeletePermanentEntities(data);
            }
           await _UserCompareRepository.SaveChanges();
        }


        public async Task<bool> DeleteFavorite(long productid, long userId)
        {
            var Deletef = await _UserFavoritenRepository.GetQuery().Where(x => x.ProductId == productid && x.UserId == userId).FirstOrDefaultAsync();
            if(Deletef != null)
            {
                _UserFavoritenRepository.DeletePermanent(Deletef);
                await _UserFavoritenRepository.SaveChanges();
                return true;
            }

            return false;
          
        }


        #endregion



        #region Maneger

        public bool CheckPermission(long permissionId, string Email)
        {
            long userId = _UserRepository.GetQuery().AsQueryable().Single(c => c.Email == Email).Id;

            var userRole = _UserRoleRepository.GetQuery().AsQueryable()
                .Where(c => c.UserId == userId).Select(r => r.RoleId).ToList();


            if (!userRole.Any())
                return false;


            var permissions = _RolePermissionRepository.GetQuery().AsQueryable()
                .Where(c => c.PermissionId == permissionId ).Select(c => c.RoleId).ToList();

            var permissionsParent = _RolePermissionRepository.GetQuery().AsQueryable()
               .Where(c => c.PermissionId == permissionId||c.Permission.ParentId== permissionId).Select(c => c.RoleId).ToList();


            return permissions.Any(c => userRole.Contains(c))|| permissionsParent.Any(c => userRole.Contains(c));
        }




        public async Task<FilterUserManege> filterUserManege(FilterUserManege filter)
        {
            var query = _UserRepository.GetQuery().AsQueryable();






            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetUsers(allEntities);
        }

        public async Task<GetEditUser> GetEditUserAdmin(long Id)
        {
            var GetUser = await _UserRepository.GetQuery().Where(c => c.Id == Id).Include(x=>x.UserRoles).Select(x=> new GetEditUser
            {
                Email = x.Email,
                FirstName = x.FirstName,
                UserID = x.Id,
                Avatar = x.Avatar,
                IsBlocked = x.IsBlocked,
                IsEmailActive = x.IsEmailActive,
                LastName = x.LastName,
                Mobile = x.Mobile,
                IsMobileActive = x.IsMobileActive,
                RoleIds = x.UserRoles.Where(x => x.UserId == Id).Select(s => s.RoleId).ToList()

            }).AsNoTracking().SingleOrDefaultAsync();

            if (GetUser == null) return null;

            return GetUser;
            //x.UserRoles.Where(c => c.UserId == userId).Select(c => c.RoleId).ToList()
        }
        public async Task<EditUserFromAdminResult> EditUserFromAdmin(GetEditUser user, IFormFile img)
        {
            var getuser = await _UserRepository.GetQuery().Where(x => x.Id == user.UserID).SingleOrDefaultAsync();

            if (getuser == null) return EditUserFromAdminResult.NotFound;
            if (img != null && img.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(img.FileName);
                img.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer, 100, 100, PathExtension.UserAvatarThumbServer, user.Avatar);
                getuser.Avatar = imageName;
            }

            getuser.Email = user.Email;
            getuser.FirstName = user.FirstName;
            getuser.LastName = user.LastName;
            if (user.password != null)
            {
                getuser.Password = _passwordHelper.EncodePasswordMd5(user.password);
            }
            getuser.IsEmailActive = user.IsEmailActive;
            getuser.Mobile = user.Mobile;
            getuser.IsBlocked = user.IsBlocked;
            getuser.IsDelete = false;
            getuser.MobileActiveCode = new Random().Next(10000, 999999).ToString();
            getuser.EmailActiveCode = Guid.NewGuid().ToString("N");
            getuser.IsMobileActive = user.IsMobileActive;

            _UserRepository.EditEntity(getuser);

            await RemoveAllUserSelectedRole(user.UserID);


            await AddUserToRole(user.RoleIds, user.UserID);

          
            await _UserRepository.SaveChanges();
            return EditUserFromAdminResult.Success;
        }

        public async Task<FilterRolesDTO> FilterRoles(FilterRolesDTO filter)
        {

            var query = _RoleRepository.GetQuery().Where(x => x.IsDelete == false).AsQueryable();

            #region filter
            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                query = query.Where(c => EF.Functions.Like(c.RoleTitle, $"%{filter.RoleName}%"));
            }
            #endregion



            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion


            return filter.SetPaging(pager).SetRoles(allEntities);


        }
        #region Admin


        public async Task<CreateOrEditRoleResult> CreateOrEditRole(CreateOrEditRoleDTO createOrEditRole)
        {
            if (createOrEditRole.Id != 0)
            {
                var role = await _RoleRepository.GetEntityById(createOrEditRole.Id);

                if (role == null)
                    return CreateOrEditRoleResult.NotFound;

                role.RoleTitle = createOrEditRole.RoleTitle;

                _RoleRepository.EditEntity(role);

                await RemoveAllPermissionSelectedRole(createOrEditRole.Id);

                if (createOrEditRole.SelectedPermission == null)
                {
                    return CreateOrEditRoleResult.NotExistPermissions;
                }
                await AddPermissionToRole(createOrEditRole.SelectedPermission, createOrEditRole.Id);
                await _RolePermissionRepository.SaveChanges();

                return CreateOrEditRoleResult.Success;
            }
            else
            {
                //create

                var newRole = new Role
                {
                    RoleTitle = createOrEditRole.RoleTitle
                };

                await _RoleRepository.AddEntity(newRole);

                await _RoleRepository.SaveChanges();


                if (createOrEditRole.SelectedPermission == null)
                {
                    return CreateOrEditRoleResult.NotExistPermissions;
                }

                await AddPermissionToRole(createOrEditRole.SelectedPermission, newRole.Id);

                await _RoleRepository.SaveChanges();


                return CreateOrEditRoleResult.Success;
            }
        }

        public async Task AddPermissionToRole(List<long> selctedPermission, long roleId)
        {
            if (selctedPermission != null && selctedPermission.Any())
            {
                var rolePermissions = new List<RolePermission>();

                foreach (var permissionId in selctedPermission)
                {
                    rolePermissions.Add(new RolePermission
                    {
                        PermissionId = permissionId,
                        RoleId = roleId,

                    });
                }

                await _RolePermissionRepository.AddRangeEntities(rolePermissions);
            }
        }

        public async Task RemoveAllPermissionSelectedRole(long roleId)
        {
            var allRolePermissions = await _RolePermissionRepository.GetQuery().Where(c => c.RoleId == roleId).ToListAsync();

            if (allRolePermissions.Any())
            {
                _RolePermissionRepository.DeletePermanentEntities(allRolePermissions);
                await _RolePermissionRepository.SaveChanges();
            }
        }

        public void UpdateRole(Role role)
        {
            _RoleRepository.EditEntity(role);
        }

        public async Task<List<Permission>> GetAllActivePermission()
        {
            return await _PermissionRepository.GetQuery().Where(c => !c.IsDelete).ToListAsync();
        }

        public async Task<Role> GetRoleById(long id)
        {
            var role = await _RoleRepository.GetEntityById(id);
            return role;
        }

        public async Task<List<Role>> GetAllActiveRoles()
        {
            return await _RoleRepository.GetQuery().AsQueryable().Where(c => !c.IsDelete).ToListAsync();
        }

        public async Task RemoveAllUserSelectedRole(long userId)
        {
            var allUserRoles = await _UserRoleRepository.GetQuery().AsQueryable().Where(c => c.UserId == userId).ToListAsync();

            if (allUserRoles.Any())
            {
                _UserRoleRepository.DeletePermanentEntities(allUserRoles);

                await _UserRoleRepository.SaveChanges();
            }
        }

        public async Task AddUserToRole(List<long> selctedRole, long userId)
        {
            if (selctedRole != null && selctedRole.Any())
            {
                var userRoles = new List<UserRole>();

                foreach (var roleId in selctedRole)
                {
                    userRoles.Add(new UserRole
                    {
                        RoleId = roleId,
                        UserId = userId
                    });
                }

                await _UserRoleRepository.AddRangeEntities(userRoles);

                await _UserRoleRepository.SaveChanges();
            }
        }

        public async Task<CreateOrEditRoleDTO> GetEditRoleById(long roleId)
        {
            return await _RoleRepository.GetQuery().AsQueryable()
                    .Include(c => c.RolePermissions)
                     .Where(c => c.Id == roleId)
                     .Select(x => new CreateOrEditRoleDTO
                     {
                         Id = x.Id,
                         RoleTitle = x.RoleTitle,
                         SelectedPermission = x.RolePermissions.Select(c => c.PermissionId).ToList()

                     }).SingleOrDefaultAsync();
        }
        public async Task<bool> DeleteRole(long roleId)
        {
            var get = await _RoleRepository.GetQuery().Where(r => r.Id == roleId).Include(c => c.RolePermissions).SingleOrDefaultAsync();
            if (get == null) return false;
            _RoleRepository.DeleteEntity(get);

            await _RoleRepository.SaveChanges();

            var rolePermission = await _RolePermissionRepository.GetQuery().Where(x => x.RoleId == get.Id).ToListAsync();

            _RolePermissionRepository.ISDeletePermanentEntities(rolePermission);
            await _RolePermissionRepository.SaveChanges();
            return true;
        }
        #endregion




        #endregion
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _UserRepository.DisposeAsync();

        }



        #endregion



    }
}
