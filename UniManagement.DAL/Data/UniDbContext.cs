using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniManagement.DAL.Data
{
    public class UniDbContext : IdentityDbContext<IdentityUser>
    {
        public UniDbContext() { }

        public UniDbContext(DbContextOptions<UniDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ExamResult>().HasKey(x => new { x.StudentId, x.ExamId });
            //var rel = modelBuilder.Entity<Trainer>().HasMany(t => t.Exams).WithMany(e => e.Trainers);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
    }
}
