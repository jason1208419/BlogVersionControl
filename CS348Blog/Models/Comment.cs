using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSC348Blog.Models
{
    //To get and save data from/to storage for post controller
    //For building a table in database and for user to view
    public class Comment
    {
        [Required,Key]
        public int CommentID { get; set; }

        [Required]
        public string Creator { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        public string Editor { get; set; }
        public DateTime EditDate { get; set; }

        [Required]
        public string Content { get; set; }

        public int ParentCommentID { get; set; }

        [Required,ForeignKey("Comment")]
        public int PostID { get; set; }
    }
}
