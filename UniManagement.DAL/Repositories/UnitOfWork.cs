using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniManagement.DAL.Data;

namespace UniManagement.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniDbContext _ctx;
        public IStudentRepository StudentRepo { get; private set; }
        public IRepository<Exam> ExamRepo { get; private set; }
        public IRepository<ExamResult> ExamResultRepo { get; private set; }

        public UnitOfWork(UniDbContext ctx)
        {
            _ctx = ctx;
            StudentRepo = new StudentRepository(ctx);
            ExamRepo = new ExamRepository(ctx);
            ExamResultRepo = new ExamResultRepository(ctx);
        }

        public bool Commit()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}
