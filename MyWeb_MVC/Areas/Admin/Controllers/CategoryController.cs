using Microsoft.AspNetCore.Mvc;
using Web.DataAccess.Data;
using Web.DataAccess.Repository.IRepository;
using Web.Models;

namespace MyWeb_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
<<<<<<< HEAD:MyWeb_MVC/Areas/Admin/Controllers/CategoryController.cs

=======
        
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0:MyWeb_MVC/Controllers/CategoryController.cs
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
<<<<<<< HEAD:MyWeb_MVC/Areas/Admin/Controllers/CategoryController.cs

=======
            
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0:MyWeb_MVC/Controllers/CategoryController.cs
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        //create category
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //Server side Validation
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Displayorder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        //edit category
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
<<<<<<< HEAD:MyWeb_MVC/Areas/Admin/Controllers/CategoryController.cs
            Category? cat = _unitOfWork.Category.Get(u => u.Id == id);
            if (cat == null)
=======
            Category? cat = _unitOfWork.Category.Get(u=>u.Id==id);
            if(cat == null)
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0:MyWeb_MVC/Controllers/CategoryController.cs
            {
                return NotFound();
            }
            return View(cat);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        //Delete Category
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
<<<<<<< HEAD:MyWeb_MVC/Areas/Admin/Controllers/CategoryController.cs
            Category? cat = _unitOfWork.Category.Get(u => u.Id == id);
=======
            Category? cat = _unitOfWork.Category.Get(u=>u.Id==id);
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0:MyWeb_MVC/Controllers/CategoryController.cs
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
<<<<<<< HEAD:MyWeb_MVC/Areas/Admin/Controllers/CategoryController.cs
            if (obj == null)
=======
            if (obj== null)
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0:MyWeb_MVC/Controllers/CategoryController.cs
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
