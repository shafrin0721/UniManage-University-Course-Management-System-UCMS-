using System;
using System.ComponentModel.DataAnnotations;

namespace UniManage.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }

        [Required, StringLength(120)]
        public string Subject { get; set; }

        [Required, StringLength(2000)]
        public string Body { get; set; }

        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
