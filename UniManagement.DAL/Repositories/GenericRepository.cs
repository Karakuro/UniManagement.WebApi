using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniManagement.DAL.Data;

namespace UniManagement.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly UniDbContext _ctx;
        internal DbSet<T> _dbSet {  get; set; }

        public GenericRepository(UniDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = _ctx.Set<T>();
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
            return _dbSet.Include(include).SingleOrDefault(query);
        }

        public List<T> GetByFilter(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        //public T? Get(int[] ids)
        //{
        //    return _ctx.Find<T>(ids);
        //}

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public void DeleteAll()
        {
            _dbSet.RemoveRange(_ctx.Set<T>().ToList());
        }

        public virtual bool Update(T entity)
        {
            _dbSet.Update(entity);
            return _ctx.SaveChanges() > 0;
        }
    }
}
