﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniManagement.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        T? Get(int id);
        T? Get(Func<T, bool> query, string include);
        List<T> GetAll();
        T Create(T entity);
        bool Update(T entity);
        bool Delete(int id);
    }
}