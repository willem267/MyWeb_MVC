using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DataAccess.Data;
using Web.DataAccess.Repository.IRepository;

namespace Web.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
<<<<<<< HEAD
        public IProductRepository Product {  get; private set; }
=======
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0
        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Category = new CategoryRepository(_db);
<<<<<<< HEAD
            Product = new ProductRepository(_db);
=======
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0
        }

        public void Save()
        {
           _db.SaveChanges();
        }
    }
}
