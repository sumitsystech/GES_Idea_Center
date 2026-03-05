using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES_IdeaSystem.Domain;
using GES_IdeaSystem.Domain.Entities;

namespace GES_IdeaSystem.Application.interfaces
{

    public interface IIdeaRepository
    {
        Task AddIdeaAsync(Idea idea);
        Task<List<Idea>> GetIdeasAsync(string? search); 
        Task<Idea?> GetByIdAsync(int id);
     
      
        Task<Idea> GetIdeaWithOwner(int id);
        
         Task<Idea> UpdateAsync(int id, int userId, Idea idea);

        Task<List<Idea>> GetAllAsync();

        //Vote related api's
        Task AddVoteAsync(Vote vote);
        Task<bool> UserAlreadyVoted(int ideaId, int userId);

        
        Task SaveAsync();

        //vote count
        Task<int> GetVoteCount(int ideaId);

    }
}
