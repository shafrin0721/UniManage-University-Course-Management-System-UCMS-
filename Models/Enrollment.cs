using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniManage.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public DateTime EnrolledAt { get; set; }
        public decimal? FinalGrade { get; set; }

        public virtual AppUser Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
