using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniManagement.DAL.Data
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Exam> Exams { get; set; }
    }
}
