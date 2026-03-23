using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniManage.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string CourseCode { get; set; }

        [Required, StringLength(120)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Range(1, 500)]
        public int EnrollmentLimit { get; set; }

        [StringLength(20)]
        public string PrerequisiteCourseCode { get; set; }

        [ForeignKey("Lecturer")]
        public int? LecturerId { get; set; }

        public virtual AppUser Lecturer { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
