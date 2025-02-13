using Microsoft.AspNetCore.Mvc;
using MyWeb_MVC.Data;
using MyWeb_MVC.Models;

namespace MyWeb_MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        // Đây là constructor, nơi dependency được inject
        public CategoryController(ApplicationDbContext db)
        {
            _db = db; // Gán đối tượng db được inject vào biến thành viên _db
        }
        public IActionResult Index()
        {
            // Sử dụng _db để lấy danh sách các Category từ cơ sở dữ liệu
            List<Category> objCategoryList = _db.Categories.ToList();
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
            //Server Validation
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Displayorder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        //edit category
        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Category? cat = _db.Categories.Find(id);
            if(cat == null)
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
                _db.Categories.Update(obj);
                _db.SaveChanges();
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
            Category? cat = _db.Categories.Find(id);
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if(obj== null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
