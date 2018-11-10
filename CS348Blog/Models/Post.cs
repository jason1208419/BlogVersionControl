using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSC348Blog.Models
{
    public class Post
    {
        //[Required, Key, ScaffoldColumn(false)]
        public int PostID { get; set; }

        //[Required, ScaffoldColumn(false)]
        public String Creator { get; set; } = "x";

        //[Required, ScaffoldColumn(false)]
        public DateTime CreationDate { get; set; } = DateTime.MinValue;

        //[ScaffoldColumn(false)]
        public String Editor { get; set; }

        //[ScaffoldColumn(false)]
        public DateTime EditDate { get; set; }

        //[Required, MinLength(2), MaxLength(50)]
        public String Title { get; set; }

        //[Required, MinLength(10)]
        public String Content { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
