namespace UniManagement.WebApi.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<ExamModel>? Exams { get; set; }
    }
}
