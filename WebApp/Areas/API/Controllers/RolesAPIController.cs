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
    public class RolesAPIController : ControllerBase
    {
        private readonly DPContext _context;

        public RolesAPIController(DPContext context)
        {
            _context = context;
        }

        // GET: api/RolesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolesModel>>> GetRoles()
        {
            return await _context.Roles.Where(s => s.Status == true).ToListAsync();
        }

        // GET: api/RolesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolesModel>> GetRolesModel(int id)
        {
            var rolesModel = await _context.Roles.FindAsync(id);

            if (rolesModel == null)
            {
                return NotFound();
            }

            return rolesModel;
        }

        // PUT: api/RolesAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRolesModel(int id, RolesModel rolesModel)
        {
            if (id != rolesModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(rolesModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolesModelExists(id))
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

        // POST: api/RolesAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RolesModel>> PostRolesModel(RolesModel rolesModel)
        {
            _context.Roles.Add(rolesModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRolesModel", new { id = rolesModel.Id }, rolesModel);
        }

        // DELETE: api/RolesAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RolesModel>> DeleteRolesModel(int id)
        {
            var rolesModel = await _context.Roles.FindAsync(id);
            if (rolesModel == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(rolesModel);
            await _context.SaveChangesAsync();

            return rolesModel;
        }

        private bool RolesModelExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
