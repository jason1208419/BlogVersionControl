using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSC348Blog.Models
{
    //To get and save data from/to storage for post controller.
    //Only necessary data is here for user to create or edit
    //to prevent over-posting
    public class CommentViewModel
    {
        [Required]
        public int PostID { get; set; }

        [Required]
        public int ParentCommentID { get; set; }

        [Required, MinLength(2), MaxLength(1000)]
        public string Content { get; set; }

        public string ReplyTo { get; set; }
    }
}
