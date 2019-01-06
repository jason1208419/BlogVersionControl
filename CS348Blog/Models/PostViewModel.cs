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
    public class PostViewModel
    {
        [Required, MinLength(2), MaxLength(50)]
        public String Title { get; set; }

        [Required, MinLength(10), MaxLength(10000)]
        public String Content { get; set; }
    }
}
