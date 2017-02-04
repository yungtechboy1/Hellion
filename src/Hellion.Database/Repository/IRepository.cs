using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hellion.Database.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        T Get(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        void Add(T value, bool saveContext = false);
        void Update(T value, bool saveContext = false);
        void Delete(T value, bool saveContext = false);
        void Save();
    }
}
