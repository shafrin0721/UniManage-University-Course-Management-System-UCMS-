using System;
using System.Data.Entity;
using UniManage.Helpers;
using UniManage.Models;

namespace UniManage.DAL
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<UcmsDbContext>
    {
        protected override void Seed(UcmsDbContext context)
        {
            string s1, s2, s3;

            var admin = new AppUser
            {
                FullName = "System Administrator",
                Username = "admin",
                Email = "admin@unimanage.com",
                Role = "Administrator",
                PasswordHash = PasswordHelper.HashPassword("Admin@123", out s1),
                PasswordSalt = s1,
                CreatedAt = DateTime.Now
            };

            var lecturer = new AppUser
            {
                FullName = "Dr. Nimal Perera",
                Username = "lecturer1",
                Email = "lecturer1@unimanage.com",
                Role = "Lecturer",
                PasswordHash = PasswordHelper.HashPassword("Lecturer@123", out s2),
                PasswordSalt = s2,
                CreatedAt = DateTime.Now
            };

            var student = new AppUser
            {
                FullName = "Student One",
                Username = "student1",
                Email = "student1@unimanage.com",
                Role = "Student",
                PasswordHash = PasswordHelper.HashPassword("Student@123", out s3),
                PasswordSalt = s3,
                CreatedAt = DateTime.Now
            };

            context.AppUsers.Add(admin);
            context.AppUsers.Add(lecturer);
            context.AppUsers.Add(student);
            context.SaveChanges();

            var c1 = new Course
            {
                CourseCode = "IT301",
                Title = "Web Application Development",
                Description = "ASP.NET MVC based module.",
                EnrollmentLimit = 40,
                LecturerId = lecturer.Id
            };

            var c2 = new Course
            {
                CourseCode = "IT302",
                Title = "Database Systems",
                Description = "Relational design and SQL.",
                EnrollmentLimit = 35,
                LecturerId = lecturer.Id
            };

            context.Courses.Add(c1);
            context.Courses.Add(c2);
            context.SaveChanges();

            context.Enrollments.Add(new Enrollment
            {
                StudentId = student.Id,
                CourseId = c1.Id,
                EnrolledAt = DateTime.Now
            });

            context.Enrollments.Add(new Enrollment
            {
                StudentId = student.Id,
                CourseId = c2.Id,
                EnrolledAt = DateTime.Now
            });

            context.SaveChanges();

            var a1 = new Assignment
            {
                CourseId = c1.Id,
                Title = "MVC Authentication Module",
                Instructions = "Implement login, registration, and access control.",
                Deadline = DateTime.Now.AddDays(7)
            };

            var a2 = new Assignment
            {
                CourseId = c2.Id,
                Title = "Normalization Exercise",
                Instructions = "Normalize the academic data model.",
                Deadline = DateTime.Now.AddDays(10)
            };

            context.Assignments.Add(a1);
            context.Assignments.Add(a2);
            context.SaveChanges();

            context.Messages.Add(new Message
            {
                FromUserId = lecturer.Id,
                ToUserId = student.Id,
                Subject = "Welcome",
                Body = "Please review the course outline before the first class.",
                SentAt = DateTime.Now,
                IsRead = false
            });

            context.SaveChanges();

            base.Seed(context);
        }
    }
}