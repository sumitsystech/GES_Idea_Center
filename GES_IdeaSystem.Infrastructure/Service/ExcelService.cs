using GES_IdeaSystem.Application.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES_IdeaSystem.Infrastructure.Service
{
    public class ExcelService
    {
        private readonly IIdeaRepository _repo;

        public ExcelService(IIdeaRepository repo)
        {
            _repo = repo;
        }

        //public async Task<byte[]> ExportIdeas()
        //{
        //    var data = await _repo.GetIdeasWithVotes();

        //    using var wb = new XLWorkbook();
        //    var ws = wb.Worksheets.Add("Ideas");

        //    ws.Cell(1, 1).Value = "Idea";
        //    ws.Cell(1, 2).Value = "Votes";

        //    int row = 2;

        //    foreach (var item in data)
        //    {
        //        ws.Cell(row, 1).Value = item.Title;
        //        ws.Cell(row, 2).Value = item.VoteCount;
        //        row++;
        //    }

        //    using var ms = new MemoryStream();
        //    wb.SaveAs(ms);
        //    return ms.ToArray();
        //}
    }
}
