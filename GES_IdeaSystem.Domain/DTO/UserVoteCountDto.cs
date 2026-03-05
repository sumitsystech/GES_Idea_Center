using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES_IdeaSystem.Domain.DTO
{
    public class UserVoteCountDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public int VoteCount { get; set; }
    }
}
