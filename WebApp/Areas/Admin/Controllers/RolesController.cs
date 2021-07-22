using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.Data;
using WebApp.Areas.Admin.Models;
using static WebApp.Helper;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly DPContext _context;

        public RolesController(DPContext context)
        {
            _context = context;
        }

        // GET: Admin/Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.Where(sp => sp.Status == true).ToListAsync());
        }

        // GET: Admin/Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolesModel = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rolesModel == null)
            {
                return NotFound();
            }

            return View(rolesModel);
        }

        // Admin/Roles/AddOrEdit
        // GET: Roles/AddOrEdit/5(Update)
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)//Admin/Roles/AddOrEdit/0
        {
            if (id == 0)//insert
                return View(new RolesModel());
            else
            {
                //edit
                var rolesModel = await _context.Roles.FindAsync(id);
                if (rolesModel == null)
                {
                    return NotFound();
                }
                return View(rolesModel);
            }
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id,[Bind("Id,Name")] RolesModel rolesModel)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    _context.Add(rolesModel);
                    await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        _context.Update(rolesModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RolesModelExists(rolesModel.Id))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Roles.Where(sp => sp.Status == true).ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", rolesModel) });
        }

        // GET: Admin/Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolesModel = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rolesModel == null)
            {
                return NotFound();
            }

            return View(rolesModel);
        }

        // POST: Admin/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rolesModel = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(rolesModel);
            await _context.SaveChangesAsync();
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Roles.Where(sp => sp.Status == true).ToList()) });
        }

        private bool RolesModelExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
