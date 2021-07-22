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
    public class LessonAPIController : ControllerBase
    {
        private readonly DPContext _context;

        public LessonAPIController(DPContext context)
        {
            _context = context;
        }

        // GET: api/LessonAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonModel>>> GetLesson()
        {
            return await _context.Lesson.ToListAsync();
        }

        // GET: api/LessonAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<LessonModel>>> GetLessonModel(int id)
        {
            return await _context.Lesson.OrderBy(sp => sp.Id).Where(sp => sp.Status == true && sp.IdCourse == id).ToListAsync();
        }

        // PUT: api/LessonAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLessonModel(int id, LessonModel lessonModel)
        {
            if (id != lessonModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(lessonModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonModelExists(id))
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

        // POST: api/LessonAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LessonModel>> PostLessonModel(LessonModel lessonModel)
        {
            _context.Lesson.Add(lessonModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLessonModel", new { id = lessonModel.Id }, lessonModel);
        }

        // DELETE: api/LessonAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LessonModel>> DeleteLessonModel(int id)
        {
            var lessonModel = await _context.Lesson.FindAsync(id);
            if (lessonModel == null)
            {
                return NotFound();
            }

            _context.Lesson.Remove(lessonModel);
            await _context.SaveChangesAsync();

            return lessonModel;
        }

        private bool LessonModelExists(int id)
        {
            return _context.Lesson.Any(e => e.Id == id);
        }
    }
}
