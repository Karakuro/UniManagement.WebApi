using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniManagement.DAL.Data
{
    public class Exam
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        public List<ExamResult> Results { get; set; }
        public List<Trainer> Trainers { get; set; }
    }
}
