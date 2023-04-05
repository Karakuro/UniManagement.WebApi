using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniManagement.DAL.Data;
using UniManagement.WebApi.Models;

namespace UniManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly UniDbContext _ctx;
        private readonly Mapper _map;

        public StudentController(UniDbContext ctx, Mapper mapper)
        {
            _ctx = ctx;
            _map = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_ctx.Students.ToList().ConvertAll(_map.MapEntityToModel));
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            Student? stud = _ctx.Students
                //.Include(s => s.Results)
                .Include("Results.Exam")
                .SingleOrDefault(s => s.StudentId == id);
            if (stud == null)
                return BadRequest();

            return Ok(_map.MapEntityToModel(stud));
        }

        [HttpPost]
        public IActionResult Create(StudentModel student)
        {
            student.Id = 0;
            _ctx.Students.Add(_map.MapModelToEntity(student));
            _ctx.SaveChanges();
            return Ok(student);
        }
    }
}
