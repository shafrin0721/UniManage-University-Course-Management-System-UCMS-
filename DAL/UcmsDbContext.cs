using System.Data.Entity;
using UniManage.Models;

namespace UniManage.DAL
{
    public class UcmsDbContext : DbContext
    {
        public UcmsDbContext() : base("UcmsConnection") { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}