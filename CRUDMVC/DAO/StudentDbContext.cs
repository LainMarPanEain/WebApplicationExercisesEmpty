using CRUDMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDMVC.DAO
{
    public class StudentDbContext:DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options){ 
        }
        public DbSet<Student> Students { get; set; }//define your student entity
    }
}
