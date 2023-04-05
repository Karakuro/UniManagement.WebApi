using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using UniManagement.DAL.Data;

namespace UniManagement.WebApi.Models
{
    public class Mapper
    {
        public StudentModel MapEntityToModel(Student entity)
        {
            StudentModel model = new StudentModel();
            model.Id = entity.StudentId;
            model.Name = entity.Name;
            model.Surname = entity.Surname;
            if(entity.Results != null) //&& entity.Results.All(e => e.Exam != null))
            {
                model.Exams = entity.Results.ConvertAll(MapEntityToModel);
            }
            return model;
        }

        public ExamModel MapEntityToModel(ExamResult entity)
        {
            //if (entity.Exam == null)
            //    return null;
            ExamModel model = new ExamModel();
            model.Id = entity.ExamId;
            model.Title = entity.Exam?.Title ?? "Titolo non disponibile";
            model.Score = entity.Result;
            return model;
        }

        public ExamModel MapEntityToModel(Exam entity)
        {
            ExamModel model = new ExamModel();
            return model;
        }

        public Student MapModelToEntity(StudentModel model)
        {
            Student entity = new Student();
            entity.StudentId = model.Id;
            entity.Name = model.Name;
            entity.Surname= model.Surname;
            return entity;
        }
    }
}
