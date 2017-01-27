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
        T Get(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        void Add(T value);
        void Update(T value);
        void Delete(T value);
    }
}
