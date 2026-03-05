using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES_IdeaSystem.Domain.DTO
{
    public class UpdateIdeaDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string GESCenterId { get; set; }
        public string ContributorId { get; set; }
        public string CoContributors { get; set; }
        //public string? AttachmentPath { get; set; }
    }
}
