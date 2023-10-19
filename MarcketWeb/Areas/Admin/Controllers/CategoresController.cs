using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Catgores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Admin.Controllers
{
    public class CategoresController : AdminBaseController
    {
        private IProductCategoryService _productCategoryService;

        public CategoresController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        public async Task<IActionResult> Index()
        {


            var Catgores = await _productCategoryService.GetProductCategory();

            return View(Catgores);
        }

        [HttpGet("/Admin/Categores/Create/{ParentId}")]
        public IActionResult Create(int? ParentId)
        {
            ViewBag.ParentId = ParentId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductCategoryDTO create)
        {

            var res = await _productCategoryService.CreateProductCategory(create);

            switch (res)
            {
                case CreateProductCategoryResult.Success:

                    TempData[SuccessMessage] = "باموفقیت ذخیره شد";

                    break;


                case CreateProductCategoryResult.Error:
                    TempData[ErrorMessage] = "خطا";

                    break;



            }

            return RedirectToAction("Index");
        }


        [HttpGet("/Admin/Categores/Edit/{id}")]

        public async Task<IActionResult> Edit(long id)
        {
            

            ViewBag.id = id;

            var catgores = await _productCategoryService.GetForEditProductCategorybyId(id);

            return View(catgores);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductCategoryDTO edit)
        {
            //await _productCategoryService.CreateProductCategory(create);
            if (ModelState.IsValid)
            {
                var res = await _productCategoryService.EditProductCategory(edit);
                switch (res)
                {
                    case EditProductCategoryResult.Success:
                        TempData[SuccessMessage] = "باموفقیت ویرایش  شد";

                        break;
                    case EditProductCategoryResult.Error:
                        TempData[ErrorMessage] = "خطا";

                        break;
                }

            }


            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/Categores/DeleteGroupById/{id}")]
        public async Task<IActionResult>  DeleteGroupById(long id)
        {
            
            return View(await _productCategoryService.GetForEditProductCategorybyId(id));
        }


        [HttpPost, ActionName("DeleteGroupById")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  DeleteConfirmed(long GroupID)
        { 

            var delete =await  _productCategoryService.DeleteProductCategorybyId(GroupID);
            switch (delete)
            {
                case true:
                    TempData[SuccessMessage] = "باموفقیت حذف شد";

                    break;
                case false:
                    TempData[ErrorMessage] = "خطا";

                    break;
            }
            return  RedirectToAction("Index");
        }


    }
}
