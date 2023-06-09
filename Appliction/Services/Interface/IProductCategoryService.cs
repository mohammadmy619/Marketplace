using MarcketDataLayer.DTOs.Catgores;
using MarcketDataLayer.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Interface
{
   public interface IProductCategoryService 
    {
        public Task<List<ProductCategory>> GetProductCategory();

        public Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryDTO create);

        public Task<ProductCategory> GetForEditProductCategorybyId(long id);

        public Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDTO edit);

        public Task<bool> DeleteProductCategorybyId(long id);


    }
}
