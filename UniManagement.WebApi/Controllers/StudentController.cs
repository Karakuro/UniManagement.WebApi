using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniManagement.DAL.Data;
using UniManagement.DAL.Repositories;
using UniManagement.WebApi.Models;

namespace UniManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        //private readonly UniDbContext _ctx;
        private readonly Mapper _map;
        private IRepository<Student> _repo;

        public StudentController(IRepository<Student> repo, Mapper mapper)
        {
            _repo = repo;
            _map = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAll().ConvertAll(_map.MapEntityToModel));
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            Student? stud = _repo.Get(s => s.StudentId == id, "Results.Exam");
            if (stud == null)
                return BadRequest();

            return Ok(_map.MapEntityToModel(stud));
        }

        [HttpPost]
        public IActionResult Create(StudentModel student)
        {
            student.Id = 0;
            var result = _repo.Create(_map.MapModelToEntity(student));
            return Ok(_map.MapEntityToModel(result));
        }
    }
}
