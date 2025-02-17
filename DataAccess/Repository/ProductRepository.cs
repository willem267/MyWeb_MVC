using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DataAccess.Data;
using Web.DataAccess.Repository.IRepository;
using Web.Models;

namespace Web.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       
        public void Update(Product obj)
        {
           var objFromdb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromdb != null)
            {
                objFromdb.Title = obj.Title;
                objFromdb.Author = obj.Author;
                objFromdb.ISBN = obj.ISBN;
                objFromdb.ListPrice = obj.ListPrice;
                objFromdb.Price = obj.Price;
                objFromdb.Price50 = obj.Price50;
                objFromdb.Price100 = obj.Price100;
                objFromdb.Description = obj.Description;
                if (obj.ImageUrl != null)
                {
                    objFromdb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
    
   
}
