using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniManagement.DAL.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T? Get(int id);
        T? Get(Func<T, bool> query, string include);
        List<T> GetAll();
        T Create(T entity);
        bool Delete(int id);
        List<T> GetByFilter(Func<T, bool> predicate);
        void DeleteAll();
    }
}
