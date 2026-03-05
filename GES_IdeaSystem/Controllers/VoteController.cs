using GES_IdeaSystem.Application;
using GES_IdeaSystem.Application.interfaces;
using GES_IdeaSystem.Domain;
using GES_IdeaSystem.Domain.Entities;
using GES_IdeaSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace GES_IdeaSystem.Controllers
{
    [EnableCors("AllowFrontend")]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IRepository<Vote> _repo;
        private readonly IIdeaRepository _ideaRepository;

        public VoteController(IRepository<Vote> repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Vote(Vote vote)
        {
            await _repo.Add(vote);
            await _repo.Save();
            return Ok();
        }


      
    }
}
