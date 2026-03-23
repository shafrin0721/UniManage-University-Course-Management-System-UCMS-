using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniManage.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(20)]
        public string Role { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Course> CoursesTaught { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
