using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniManagement.DAL.Data;

namespace UniManagement.DAL.Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected readonly UniDbContext _ctx;

        public GenericRepository(UniDbContext ctx)
        {
            _ctx = ctx;
        }

        public T Create(T entity)
        {
            _ctx.Add(entity);
            return entity;
        }

        public bool Delete(int id)
        {
            _ctx.Remove(Get(id));
            return _ctx.SaveChanges() > 0;
        }

        public T? Get(int id)
        {
            return _ctx.Find<T>(id);
        }

        public T? Get(Func<T, bool> query, string include)
        {
            return _ctx.Set<T>().Include(include).SingleOrDefault(query);
        }

        public List<T> GetByFilter(Func<T, bool> predicate)
        {
            return _ctx.Set<T>().Where(predicate).ToList();
        }

        //public T? Get(int[] ids)
        //{
        //    return _ctx.Find<T>(ids);
        //}

        public List<T> GetAll()
        {
            return _ctx.Set<T>().ToList();
        }
    }
}
