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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly DPContext _context;

        public StudentAPIController(DPContext context)
        {
            _context = context;
        }

        // GET: api/StudentAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentModel>>> GetStudent()
        {
            return await _context.Student.ToListAsync();
        }

        // GET: api/StudentAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentModel>> GetStudentModel(int id)
        {
            var studentModel = await _context.Student.FindAsync(id);

            if (studentModel == null)
            {
                return NotFound();
            }

            return studentModel;
        }

        // PUT: api/StudentAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentModel(int id, StudentModel studentModel)
        {
            if (id != studentModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentModelExists(id))
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

        // POST: api/StudentAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<StudentModel>> PostStudentModel(StudentModel studentModel)
        {
            _context.Student.Add(studentModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentModel", new { id = studentModel.Id }, studentModel);
        }

        // DELETE: api/StudentAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<StudentModel>>> DeleteStudentModel(int id)
        {
            var studentModel = await _context.Student.FindAsync(id);
            if (studentModel == null)
            {
                return NotFound();
            }

            _context.Student.Remove(studentModel);
            await _context.SaveChangesAsync();


            return await _context.Student.ToListAsync();
        }


        private bool StudentModelExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
