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
    public class CommemtPostAPIController : ControllerBase
    {
        private readonly DPContext _context;

        public CommemtPostAPIController(DPContext context)
        {
            _context = context;
        }

        // GET: api/CommemtPostAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommemtPostModel>>> GetCommemtPost()
        {
            return await _context.CommemtPost.ToListAsync();
        }

        // GET: api/CommemtPostAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommemtPostModel>> GetCommemtPostModel(int id)
        {
            var commemtPostModel = await _context.CommemtPost.FindAsync(id);

            if (commemtPostModel == null)
            {
                return NotFound();
            }

            return commemtPostModel;
        }

        // PUT: api/CommemtPostAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommemtPostModel(int id, CommemtPostModel commemtPostModel)
        {
            if (id != commemtPostModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(commemtPostModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommemtPostModelExists(id))
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

        // POST: api/CommemtPostAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommemtPostModel>> PostCommemtPostModel(CommemtPostModel commemtPostModel)
        {
            _context.CommemtPost.Add(commemtPostModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommemtPostModel", new { id = commemtPostModel.Id }, commemtPostModel);
        }

        // DELETE: api/CommemtPostAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommemtPostModel>> DeleteCommemtPostModel(int id)
        {
            var commemtPostModel = await _context.CommemtPost.FindAsync(id);
            if (commemtPostModel == null)
            {
                return NotFound();
            }

            _context.CommemtPost.Remove(commemtPostModel);
            await _context.SaveChangesAsync();

            return commemtPostModel;
        }

        private bool CommemtPostModelExists(int id)
        {
            return _context.CommemtPost.Any(e => e.Id == id);
        }
    }
}
