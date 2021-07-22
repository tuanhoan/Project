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
    public class CourseAPIController : ControllerBase
    {
        private readonly DPContext _context;

        public CourseAPIController(DPContext context)
        {
            _context = context;
        }

        // GET: api/CourseAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseModel>>> GetCourse()
        {
            return await _context.Course.ToListAsync();
        }

        // GET: api/CourseAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseModel>> GetCourseModel(int id)
        {
            var courseModel = await _context.Course.FindAsync(id);

            if (courseModel == null)
            {
                return NotFound();
            }

            return courseModel;
        }
        // GET: api/CourseAPI/5
        [HttpGet("name/{search}")]
        public async Task<ActionResult<IEnumerable<CourseModel>>> GetCourseModel(string search)
        {
            var courseModel = await _context.Course.Where(s => s.Title.Contains(search)).ToListAsync();

            if (courseModel == null)
            {
                return NotFound();
            }

            return courseModel;
        }

        // PUT: api/CourseAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseModel(int id, CourseModel courseModel)
        {
            if (id != courseModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(courseModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseModelExists(id))
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

        // POST: api/CourseAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CourseModel>> PostCourseModel(CourseModel courseModel)
        {
            _context.Course.Add(courseModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseModel", new { id = courseModel.Id }, courseModel);
        }

        // DELETE: api/CourseAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseModel>> DeleteCourseModel(int id)
        {
            var courseModel = await _context.Course.FindAsync(id);
            if (courseModel == null)
            {
                return NotFound();
            }

            _context.Course.Remove(courseModel);
            await _context.SaveChangesAsync();

            return courseModel;
        }

        private bool CourseModelExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
