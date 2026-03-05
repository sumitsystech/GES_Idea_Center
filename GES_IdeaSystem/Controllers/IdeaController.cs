using ClosedXML.Excel;
using GES_IdeaSystem.Application;
using GES_IdeaSystem.Application.interfaces;
using GES_IdeaSystem.Domain;
using GES_IdeaSystem.Domain.DTO;
using GES_IdeaSystem.Domain.Entities;
using GES_IdeaSystem.Infrastructure;
using GES_IdeaSystem.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace GES_IdeaSystem.Controllers
{
    [EnableCors("AllowFrontend")]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IdeaController : ControllerBase
    {
        private readonly IIdeaRepository _repo;
        private readonly IWebHostEnvironment _env;

        public IdeaController(IIdeaRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //    => Ok(await _repo.GetIdeasWithVotes());

        [HttpPost]
        public async Task<IActionResult> CreateIdea(
    [FromForm] CreateIdeaDto dto,
    IFormFile? file)
        {
            string? filePath = null;

            if (file != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                filePath = "/uploads/" + fileName;
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var idea = new Idea
            {
                Title = dto.Title,
                Type=dto.Type,
                Description = dto.Description,
                GESCenterId=dto.GESCenterId,
                ContributorId=dto.ContributorId,
                CoContributors=dto.CoContributors,
                AttachmentPath = filePath,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _repo.AddIdeaAsync(idea);
            await _repo.SaveAsync();

            return Ok("Idea Created");
        }


        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateIdea(int id,int userid, UpdateIdeaDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var idea = await _repo.GetByIdAsync(id);

            if (idea == null)
                return NotFound("Idea not found");

            // Update Fields
            idea.Title = dto.Title;
            idea.Type = dto.Type;
            idea.GESCenterId=dto.GESCenterId;
            idea.ContributorId=dto.ContributorId;
            idea.CoContributors=dto.CoContributors;
            idea.Description = dto.Description;
            await _repo.UpdateAsync(id, userId, idea);
            return Ok("Idea updated successfully");
        }
        [Authorize]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllIdeas()
        {
            var ideas = await _repo.GetAllAsync();

            var result = ideas.Select(i => new IdeaResponseDto
            {
                Id = i.Id,
                Title = i.Title,
                Type = i.Type,
                Description = i.Description,
                 GESCneter= i.GESCenterId,
                ContributorId = i.ContributorId,
                 Cocontributors= i.CoContributors,
                VoteCount = i.Votes.Count(),
                CreatedDate = i.CreatedDate
            });

            return Ok(result);
        }

        //[HttpPut("update/{id}")]
        //public async Task<IActionResult> UpdateIdea(int id, UpdateIdeaDto dto)
        //{
        //    if (id != dto.Id)
        //        return BadRequest("Invalid idea id");

        //    var idea = await _repo.GetIdeaByIdAsync(id);

        //    if (idea == null)
        //        return NotFound("Idea not found");

        //    // Get Logged User Id from JWT
        //    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //    // 🔒 OWNER VALIDATION
        //    if (idea.CreatedBy != userId)
        //        return Forbid("Only idea owner can update");

        //    idea.Title = dto.Title;
        //    idea.Type = dto.Type;
        //    idea.GESCenterId = dto.GESCenterId;
        //    idea.ContributorId = dto.ContributorId;
        //    idea.CoContributors = dto.CoContributors;
        //    idea.Description = dto.Description;
           

        //    _repo.UpdateAsync(idea);
        //    await _repo.SaveAsync();

        //    return Ok("Idea updated successfully");
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var idea = await _repo.GetByIdAsync(id);
        //    _repo.de(idea);
        //    await _repo.Save();
        //    return Ok();
        //}


        [HttpPost("vote")]
        public async Task<IActionResult> Vote(VoteDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (await _repo.UserAlreadyVoted(dto.IdeaId, userId))
                return BadRequest("You already voted");

            var vote = new Vote
            {
                IdeaId = dto.IdeaId,
                UserId = userId,
                IsUpVote = dto.IsUpVote,
                CreatedDate = DateTime.UtcNow
            };

            await _repo.AddVoteAsync(vote);
            await _repo.SaveAsync();

            return Ok("Vote Added");
        }

        [HttpGet]
        public async Task<IActionResult> GetIdeas(string? search)
        {
            var ideas = await _repo.GetIdeasAsync(search);

            var result = ideas.Select(i => new
            {
                i.Id,
                i.Title,
                i.Type,
                i.GESCenterId,
                i.ContributorId,
                i.CoContributors,
                i.Description,
                Votes = i.Votes.Count(v => v.IsUpVote)
            });

            return Ok(result);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var ideas = await _repo.GetIdeasAsync(null);

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Ideas");

            worksheet.Cell(1, 1).Value = "Title";
            worksheet.Cell(1, 2).Value = "Type";
            worksheet.Cell(1, 3).Value = "GES Center ID";
            worksheet.Cell(1, 4).Value = "Contributor";
            worksheet.Cell(1, 5).Value = "Co-Contributor";
            worksheet.Cell(1, 6).Value = "Description";
            worksheet.Cell(1, 7).Value = "Votes";

            int row = 2;
            foreach (var idea in ideas)
            {
                worksheet.Cell(row, 1).Value = idea.Title;
                worksheet.Cell(row, 2).Value = idea.Type;
                worksheet.Cell(row, 3).Value = idea.GESCenterId;
                worksheet.Cell(row, 4).Value = idea.ContributorId;
                worksheet.Cell(row, 5).Value = idea.CoContributors;
                worksheet.Cell(row, 6).Value = idea.Description;
                worksheet.Cell(row, 7).Value = idea.Votes.Count(v => v.IsUpVote);
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Ideas.xlsx");
        }

        [HttpGet("count/{ideaId}")]
        public async Task<IActionResult> GetVoteCount(int ideaId)
        {
            int count = await _repo.GetVoteCount(ideaId);

            return Ok(count);
        }
    }
}
