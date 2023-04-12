using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniManagement.DAL.Data;

namespace UniManagement.DAL.Repositories
{
    public class ExamResultRepository : GenericRepository<ExamResult>, IRepository<ExamResult>
    {
        public ExamResultRepository(UniDbContext ctx) : base(ctx)
        {
        }

        public ExamResult? Get(int examId, int studentId)
        {
            return _ctx.ExamResults.SingleOrDefault(r => r.ExamId == examId && r.StudentId == studentId);
        }

        public bool Update(ExamResult entity)
        {
            throw new NotImplementedException();
        }
    }
}
