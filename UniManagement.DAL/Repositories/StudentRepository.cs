﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniManagement.DAL.Data;

namespace UniManagement.DAL.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IRepository<Student>
    {
        public StudentRepository(UniDbContext ctx) : base(ctx)
        {
        }

        public bool Update(Student entity)
        {
            throw new NotImplementedException();
        }
    }
}