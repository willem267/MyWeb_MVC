using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DataAccess.Repository.IRepository;   
using Web.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Web.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;   
            this.dbSet = _db.Set<T>();
<<<<<<< HEAD
            _db.Products.Include(p => p.Category);
=======
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0

        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

<<<<<<< HEAD
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
=======
        public T Get(Expression<Func<T, bool>> filter)
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

<<<<<<< HEAD
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
           IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

            }
=======
        public IEnumerable<T> GetAll()
        {
           IQueryable<T> query = dbSet;
>>>>>>> ef64dee1b4d9957718019ba78e0014a01587bba0
            return query.ToList();
        }

        public void Remove(T entity)
        {
           dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
           dbSet.RemoveRange(entities);
        }
    }
}
