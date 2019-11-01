using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class AddVoteCommentViewModel
    {
        
        public AddVoteCommentViewModel()
        {
            Comment = new Comment();

        }

        public Comment Comment { get; set; }
        public string Message { get; set; }



    }
}
