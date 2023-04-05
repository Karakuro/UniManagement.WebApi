using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniManagement.DAL.Data
{
    public class ExamResult
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int Result { get; set; }

        public Exam Exam { get; set; }
        public Student Student { get; set; }
    }
}
