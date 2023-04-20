using Microsoft.AspNetCore.Authorization;
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
        private IUnitOfWork _repo;

        public StudentController(IUnitOfWork repo, Mapper mapper)
        {
            _repo = repo;
            _map = mapper;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult GetAll()
        {
            return Ok(_repo.StudentRepo.GetAll().ConvertAll(_map.MapEntityToModel));
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            Student? stud = _repo.StudentRepo.Get(s => s.StudentId == id, "Results.Exam");
            if (stud == null)
                return BadRequest();

            return Ok(_map.MapEntityToModel(stud));
        }

        [HttpGet]
        [Route("Active")]
        public IActionResult GetActiveStudents()
        {
            List<Student> result = _repo.StudentRepo.GetByFilter(s => s.Results.Count > 0);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Create(StudentModel student)
        {
            student.Id = 0;
            var result = _repo.StudentRepo.Create(_map.MapModelToEntity(student));
            return Ok(_map.MapEntityToModel(result));
        }
    }
}
