using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
        }
    }
}
