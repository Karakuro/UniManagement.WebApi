using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using UniManagement.DAL.Data;

namespace UniManagement.DAL.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(UniDbContext ctx) : base(ctx)
        {
        }

        public void DeleteAllStudents()
        {
            throw new NotImplementedException();
        }

        public override bool Update(Student entity)
        {
            _ctx.Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
            _ctx.Update(entity);
            return _ctx.SaveChanges() > 0;
        }

        //public override bool Update(Student entity)
        //{
        //    //Student? oldStud = Get(entity.StudentId);
        //    //if (oldStud == null) return false;

        //    //oldStud.Surname = entity.Surname;
        //    ///*






        //    // */
        //    //oldStud.Name = entity.Name;
        //    _dbSet.Update(entity);

        //    return _ctx.SaveChanges() > 0;

        //}
    }
}
