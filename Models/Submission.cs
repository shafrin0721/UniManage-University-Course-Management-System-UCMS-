using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniManage.Models
{
    public class Submission
    {
        public int Id { get; set; }

        [ForeignKey("Assignment")]
        public int AssignmentId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Required, StringLength(2000)]
        public string SubmissionText { get; set; }

        [StringLength(1000)]
        public string Feedback { get; set; }

        public decimal? Grade { get; set; }
        public DateTime SubmittedAt { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual AppUser Student { get; set; }
    }
}
