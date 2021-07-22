using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.Data;
using WebApp.Areas.Admin.Models;

namespace WebApp.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommemtLessonUserAPI : ControllerBase
    {
        private readonly DPContext _context;

        public CommemtLessonUserAPI(DPContext context)
        {
            _context = context;
        }

        // GET: api/CommemtLessonAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommemtLessonModel>>> GetCommemtLesson()
        {
            return await _context.CommemtLesson.ToListAsync();
        }

        // POST: api/CommemtLessonAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommemtLessonModel>> PostCommemtLessonModel(CommemtLessonModel commemtLessonModel)
        {
            _context.CommemtLesson.Add(commemtLessonModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommemtLessonModel", new { id = commemtLessonModel.Id }, commemtLessonModel);
        }

       
    }
}
