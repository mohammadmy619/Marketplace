using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Catgores;
using MarcketDataLayer.Entities.Products;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Implementations
{
    public class ProductCategoryService : IProductCategoryService
    {
        #region Constractor 


        private readonly IGenericRepository<ProductCategory> _ProductCategoryRepository;

        public ProductCategoryService(IGenericRepository<ProductCategory> ProductCategoryRepository)
        {
            _ProductCategoryRepository = ProductCategoryRepository;

        }


        #endregion
        #region ProductCategory

        public async Task<List<ProductCategory>> GetProductCategory()
        {
            var Categores = await _ProductCategoryRepository.GetQuery().Where(c => c.IsDelete == false).ToListAsync();

            return Categores;
        }


        public async Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDTO edit)
        {
            if (edit != null)
            {
                var category = await GetForEditProductCategorybyId(edit.Id);
                category.IsActive = edit.IsActive;
                category.UrlName = edit.UrlName;
                category.Title = edit.Title;
                category.ParentId = edit.ParentId;
                category.LastUpdateDate = DateTime.Now;
                category.IsDelete = false;
                _ProductCategoryRepository.EditEntity(category);
                await _ProductCategoryRepository.SaveChanges();
                return EditProductCategoryResult.Success;
            }

            return EditProductCategoryResult.Error;



        }

        public async Task<ProductCategory> GetForEditProductCategorybyId(long id)
        {


            return await _ProductCategoryRepository.GetEntityById(id);


        }


        public async Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryDTO create)
        {

            if (create != null)
            {


                var CreateProductCategory = new ProductCategory
                {

                    IsActive = create.IsActive,
                    Title = create.Title,
                    UrlName = create.UrlName,
                    ParentId = create.ParentId,
                    CreateDate = DateTime.Now,
                    IsDelete = false,
                    LastUpdateDate = DateTime.Now,



                };
                await _ProductCategoryRepository.AddEntity(CreateProductCategory);
                await _ProductCategoryRepository.SaveChanges();

                return CreateProductCategoryResult.Success;
            }

            return CreateProductCategoryResult.Error;



        }

        public async Task<bool> DeleteProductCategorybyId(long id)
        {
            var Delete =await _ProductCategoryRepository.GetQuery().Where(c => c.Id == id || c.ParentId == id).ToListAsync();

            if(Delete !=null && Delete.Any())
            {
                foreach (var item in Delete)
                {
                    _ProductCategoryRepository.DeletePermanent(item);

                  await  _ProductCategoryRepository.SaveChanges();
                }
                return true;
            }
            return false;
        }

        #endregion



    }
}
