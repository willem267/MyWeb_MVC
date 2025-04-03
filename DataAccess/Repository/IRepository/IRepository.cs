using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Web.DataAccess.Repository.IRepository
{
    public interface IRepository <T> where T : class
    {
        //T - is any generic model class
<<<<<<< HEAD
        IEnumerable<T> GetAll(string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
=======
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
