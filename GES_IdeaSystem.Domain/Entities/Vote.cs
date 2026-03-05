using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES_IdeaSystem.Domain.Entities
{
    //public class Vote
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    [Required]
    //    public int IdeaId { get; set; }

    //    [ForeignKey("IdeaId")]
    //    public virtual Idea Idea { get; set; }

    //    [Required]
    //    public int UserId { get; set; }

    //    [Required]
    //    public bool IsUpVote { get; set; }

    //    [Required]
    //    public DateTime VotedDate { get; set; } = DateTime.UtcNow;
    //}
    public class Vote
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        public int UserId { get; set; }
        public bool IsUpVote { get; set; }
        public DateTime CreatedDate { get; set; }

        public Idea Idea { get; set; }
    }
}
