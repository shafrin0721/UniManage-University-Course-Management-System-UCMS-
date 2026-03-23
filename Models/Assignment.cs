using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniManage.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [Required, StringLength(120)]
        public string Title { get; set; }

        [StringLength(1500)]
        public string Instructions { get; set; }

        public DateTime Deadline { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
