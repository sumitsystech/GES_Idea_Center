using GES_IdeaSystem.Application.interfaces;
using GES_IdeaSystem.Domain.Entities;
using GES_IdeaSystem.Infrastructure.Data;
using GES_IdeaSystem.Domain.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES_IdeaSystem.Infrastructure.Repository
{
    public class IdeaRepository : IIdeaRepository
    {
        private readonly AppDbContext _context;

        public IdeaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddIdeaAsync(Idea idea)
            => await _context.Ideas.AddAsync(idea);

        public async Task<List<Idea>> GetIdeasAsync(string? search)
        {
            var query = _context.Ideas
                .Include(i => i.Votes)
                .Where(i => i.IsActive);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(i => i.Title.Contains(search));

            return await query.ToListAsync();
        }

       

        public async Task AddVoteAsync(Vote vote)
            => await _context.Votes.AddAsync(vote);

        public async Task<bool> UserAlreadyVoted(int ideaId, int userId)
            => await _context.Votes.AnyAsync(v => v.IdeaId == ideaId && v.UserId == userId);

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();

        public async Task<Idea> GetIdeaWithOwner(int id)
        {
            return await _context.Ideas.FirstOrDefaultAsync(x => x.Id == id);
        }



        public async Task<List<Idea>> GetAllAsync()
        {
            return await _context.Ideas.ToListAsync();
        }

        public async Task<Idea> GetIdeaByIdAsync(int id)
        {
            return await _context.Ideas.FirstOrDefaultAsync(x => x.Id == id);
        }

        //public async Task<Idea> UpdateAsync(Idea idea)
        //{
        //    _context.Ideas.Update(idea);
        //    await _context.SaveChangesAsync();
        //    return idea;
        //}

        public async Task<Idea?> GetByIdAsync(int id)
            => await _context.Ideas.FindAsync(id);


        public async Task<bool> UpdateIdeaAsync(int id, int userId, UpdateIdeaDto dto)
        {
            var idea = await _context.Ideas.FirstOrDefaultAsync(x => x.Id == id);

            if (idea == null)
                return false;

            // 🔐 Only Owner Can Update
            if (idea.CreatedBy != userId)
                throw new UnauthorizedAccessException("Only idea owner can update this idea.");

            // Update Fields
            idea.Title = dto.Title;
            idea.Type = dto.Type;
            idea.Description = dto.Description;
            idea.GESCenterId = dto.GESCenterId;
            idea.CoContributors = dto.CoContributors;
            //idea.ModifiedDate = DateTime.UtcNow;

            _context.Ideas.Update(idea);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Idea> UpdateAsync(int id, int userId, Idea idea)
        {
            var existingIdea = await _context.Ideas.FirstOrDefaultAsync(x => x.Id == id);

            if (existingIdea == null)
                throw new InvalidOperationException("Idea not found.");

            // 🔐 Only Owner Can Update
            if (existingIdea.CreatedBy != userId)
                throw new UnauthorizedAccessException("Only idea owner can update this idea.");

            // Update Fields
            existingIdea.Title = idea.Title;
            existingIdea.Type = idea.Type;
            existingIdea.Description = idea.Description;
            existingIdea.GESCenterId = idea.GESCenterId;
            existingIdea.CoContributors = idea.CoContributors;
            //existingIdea.ModifiedDate = DateTime.UtcNow;

            _context.Ideas.Update(existingIdea);
            await _context.SaveChangesAsync();

            return existingIdea;
        }

        //votecount 
        public async Task<int> GetVoteCount(int ideaId)
        {
            return await _context.Ideas
                .CountAsync(x => x.Id == ideaId);
        }
    }
}
