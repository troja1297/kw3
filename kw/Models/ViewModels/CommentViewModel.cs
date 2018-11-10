using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kw.Models.ViewModels
{
    public class CommentViewModel
    {
        public IEnumerable<Comment> Comments { get; set; }
    }
}
