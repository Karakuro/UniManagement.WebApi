using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniManagement.DAL.Data;

namespace UniManagement.DAL.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        void DeleteAll();
    }
}
