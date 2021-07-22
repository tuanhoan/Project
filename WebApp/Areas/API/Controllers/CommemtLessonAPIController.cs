using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.Data;
using WebApp.Areas.Admin.Models;

namespace WebApp.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CommemtLessonAPIController : ControllerBase
    {
        private readonly DPContext _context;

        public CommemtLessonAPIController(DPContext context)
        {
            _context = context;
        }

        //// GET: api/CommemtLessonAPI
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CommemtLessonModel>>> GetCommemtLesson()
        //{
        //    return await _context.CommemtLesson.ToListAsync();
        //}

        // GET: api/CommemtLessonAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommemtLessonModel>>> GetCommemtLessonModel(int id)
        {
            return await _context.CommemtLesson.Include(sp => sp.User).Where(s => s.IdLesson == id).ToListAsync();
        }

        // PUT: api/CommemtLessonAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommemtLessonModel(int id, CommemtLessonModel commemtLessonModel)
        {
            if (id != commemtLessonModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(commemtLessonModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommemtLessonModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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

        // DELETE: api/CommemtLessonAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommemtLessonModel>> DeleteCommemtLessonModel(int id)
        {
            var commemtLessonModel = await _context.CommemtLesson.FindAsync(id);
            if (commemtLessonModel == null)
            {
                return NotFound();
            }

            _context.CommemtLesson.Remove(commemtLessonModel);
            await _context.SaveChangesAsync();

            return commemtLessonModel;
        }

        private bool CommemtLessonModelExists(int id)
        {
            return _context.CommemtLesson.Any(e => e.Id == id);
        }
    }
}
