using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniManagement.DAL.Data;

namespace UniManagement.DAL.Repositories
{
    public class ExamRepository : IRepository<Exam>
    {
        private readonly UniDbContext _ctx;

        public ExamRepository(UniDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<Exam> GetAll()
        {
            return _ctx.Exams.ToList();
        }

        public Exam? Get(int id)
        {
            return _ctx.Exams.SingleOrDefault(e => e.ExamId == id);
        }

        public Exam Create(Exam exam)
        {
            _ctx.Exams.Add(exam);
            _ctx.SaveChanges();
            return exam;
        }

        public bool Delete(int id)
        {
            Exam? exam = Get(id);
            if(exam != null)
            {
                _ctx.Remove(exam);
                return _ctx.SaveChanges() > 0;
            }
            return false;
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
