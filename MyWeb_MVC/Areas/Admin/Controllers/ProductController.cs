using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Web.DataAccess.Data;
using Web.DataAccess.Repository.IRepository;
using Web.Models;
using Web.Models.ViewModels;
using Web.Utility;

namespace MyWeb_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
          
            return View(objProductList);
        }
        //create product
        public IActionResult Upsert(int? id)
        {
            
         
            ProductVM productVM = new ()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
               Product = new Product()
            };
            if ((id==null|| id==0))
            {
                //create
               return View(productVM); 
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "ProductImages");
                return View(productVM);

            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile?> files)
        {
            //Server side Validation

            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach(IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath =@"images\products\product-"+ productVM.Product.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);
                        if(!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }
                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        ProductImage productImage = new ProductImage()
                        {
                           ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id
                        };
                        if(productVM.Product.ProductImages == null)
                        {
                            productVM.Product.ProductImages = new List<ProductImage>();
                        }
                        productVM.Product.ProductImages.Add(productImage);
                        
                    }
                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();




                }
                
                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(productVM);
            }
            
        }

        public IActionResult DeleteImage(int imageId)
        {
            var imgToBeDeleted = _unitOfWork.ProductImage.Get(u=>u.Id == imageId);
            int productId = imgToBeDeleted.ProductId;   
            if (imgToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imgToBeDeleted.ImageUrl)){

                    if (!string.IsNullOrEmpty(imgToBeDeleted.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,imgToBeDeleted.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                }
                _unitOfWork.ProductImage.Remove(imgToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Image deleted successfully";
            }
            return RedirectToAction(nameof(Upsert), new {id= productId});
        }
       
        
       
        #region API CALLLs 
        
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);
            if (Directory.Exists(finalPath))
            {
                string[] filepaths = Directory.GetFiles(finalPath);
                foreach (var filepath in filepaths)
                {
                    System.IO.File.Delete(filepath);
                }
                Directory.Delete(finalPath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save(); 
          
            return Json(new { success = true, message = "Deleted successfully" });
        }
        #endregion
    }
}
