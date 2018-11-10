using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSC348Blog.Models
{
    public class CommentViewModel
    {
        [Required]
        public int PostID { get; set; }

        [Required]
        public int MainCommentID { get; set; }

        [Required]
        public String Content { get; set; }

        public String ReplyTo { get; set; }
    }
}
