using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniManagement.DAL.Data;

namespace UniManagement.DAL.Repositories
{
    public class ExamRepository : GenericRepository<Exam>, IRepository<Exam>
    {

        public ExamRepository(UniDbContext ctx) : base(ctx)
        {
        }

        public bool Update(Exam exam)
        {
            Exam? old = Get(exam.ExamId);
            if (old != null)
            {
                old.Title = exam.Title;
                return _ctx.SaveChanges() > 0;
            }
            //else
            //{
            //    exam.ExamId = 0;
            //    return Create(exam);
            //}

            return false;
        }
    }
}
