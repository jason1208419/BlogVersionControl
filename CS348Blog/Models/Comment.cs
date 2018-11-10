using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSC348Blog.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public String Creator { get; set; }
        public DateTime CreationTime { get; set; }
        public String Editor { get; set; }
        public DateTime EditDate { get; set; }
        public String Content { get; set; }
        public int ParentCommentID { get; set; }
    }
}
