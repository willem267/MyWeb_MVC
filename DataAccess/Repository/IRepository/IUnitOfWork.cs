using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
<<<<<<< HEAD
        IProductRepository Product { get; }
=======
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0
        void Save();
    }
}
