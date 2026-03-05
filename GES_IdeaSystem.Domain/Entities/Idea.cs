using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES_IdeaSystem.Domain.Entities
{
    //public class Idea
    //{
    //    public int Id { get; set; }
    //    public string Title { get; set; }
    //    public string Description { get; set; }
    //    public int CreatedBy { get; set; }
    //    public DateTime CreatedDate { get; set; }

    //    public ICollection<Vote> Votes { get; set; }
    //}

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Idea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [StringLength(100)]
        public string Type { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 10)]
        public string Description { get; set; }

        [Required(ErrorMessage = "GES Center Id is required")]
        [StringLength(50)]
        public string GESCenterId { get; set; }

        [Required(ErrorMessage = "Contributor Id is required")]
        [StringLength(50)]
        public string ContributorId { get; set; }

        [StringLength(500)]
        public string CoContributors { get; set; }

        [StringLength(300)]
        public string? AttachmentPath { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation Property
        public virtual List<Vote> Votes { get; set; } = new List<Vote>();
    }
}
