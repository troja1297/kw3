using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace kw.Models.ViewModels
{
    public class CommentsInThemeViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string Body { get; set; }

        public DateTime DateTime { get; set; }
        
        public string UserName { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
