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
    public class CompanyController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
     
        }
        public IActionResult Index()
        {

            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
          
            return View(objCompanyList);
        }
        //create company
        public IActionResult Upsert(int? id)
        {
            
         
           
            if ((id==null|| id==0))
            {
                //create
               return View(new Company()); 
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);

            }
        }
        [HttpPost]
        public IActionResult Upsert(Company companyObj, IFormFile? file)
        {
            //Server side Validation

            if (ModelState.IsValid)
            {
               
                
                if(companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }
               
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                
                return View(companyObj);
            }
            
        }
       
        
       
        #region API CALLLs 
        
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();

            return Json(new { data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyToBeDeleted==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
           
            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save(); 
          
            return Json(new { success = true, message = "Deleted successfully" });
        }
        #endregion
    }
}
