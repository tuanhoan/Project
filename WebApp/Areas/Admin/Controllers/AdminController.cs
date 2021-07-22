using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class AdminController : Controller
    {
        private readonly DPContext _context;

        public AdminController(DPContext context)
        {
            _context = context;
        }

        // GET: Admin/Admin
        public async Task<IActionResult> Index(string search)
        {
            if (search != null)
            {
                var adminContext = _context.Admin.Where(sp => sp.Status == true && sp.FullName.Contains(search)).Include(a => a.User);
                return View(await adminContext.ToListAsync());
            }
            var dPContext = _context.Admin.Where(sp => sp.Status == true).Include(a => a.User);
            return View(await dPContext.ToListAsync());
        }

        // GET: Admin/Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminModel = await _context.Admin
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminModel == null)
            {
                return NotFound();
            }

            return View(adminModel);
        }

        // GET: Admin/Admin/Create
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewData["IdUser"] = new SelectList(_context.User.Where(sp => sp.Status == true), "Id", "AccountName");
                return View(new AdminModel());
            }
            else
            {
                var adminModel = await _context.Admin.FindAsync(id);
                if (adminModel == null)
                {
                    return NotFound();
                }
                ViewData["IdUser"] = new SelectList(_context.User.Where(sp => sp.Status == true), "Id", "AccountName", adminModel.IdUser);
                return View(adminModel);
            }
        }

        // POST: Admin/Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id,[Bind("Id,FullName,Email,Address,Phone,IdUser")] AdminModel adminModel)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    _context.Add(adminModel);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        _context.Update(adminModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AdminModelExists(adminModel.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                var dPContext = _context.Admin.Where(sp => sp.Status == true).Include(a => a.User);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", dPContext.ToList()) });
            }
            ViewData["IdUser"] = new SelectList(_context.User.Where(sp => sp.Status == true), "Id", "AccountName", adminModel.IdUser);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", adminModel)});
        }


        // GET: Admin/Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminModel = await _context.Admin
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminModel == null)
            {
                return NotFound();
            }

            return View(adminModel);
        }

        // POST: Admin/Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminModel = await _context.Admin.FindAsync(id);
            _context.Admin.Remove(adminModel);
            await _context.SaveChangesAsync();
            var dPContext = _context.Admin.Where(sp => sp.Status == true).Include(a => a.User);
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", dPContext.ToList()) });
        }

        //Get:Admin/Admin/Profile/id
        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var adminModel = await _context.Admin.Where(sp => sp.IdUser == id).FirstOrDefaultAsync();
            if(adminModel == null)
            {
                AdminModel createAdmin = new AdminModel();
                createAdmin.IdUser = (int)id; 
            }
            ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName", id);
            ViewData["Img"] = _context.User.Find(id).Img;
            return View(adminModel);
        }

        //Post:Admin/Admin/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(int id,[Bind("Id,FullName,Email,Address,Phone,IdUser")] AdminModel adminModel, IFormFile ful)
        {
            if(id != adminModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminModel);
                    var admin = await _context.Admin.FindAsync(id);
                    if (ful != null)
                    {
                        var userModel = await _context.User.FindAsync(admin.IdUser);
                        string path = null;
                        if ( userModel.Img != "NoImg.jpg")
                        {
                            path= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/User", userModel.Img);
                            System.IO.File.Delete(path);
                        }
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/User",
                        userModel.Id + "." + ful.FileName.Split('.')[ful.FileName.Split('.').Length - 1]);
                        HttpContext.User.Claims.ToList()[0] = new Claim("img", userModel.Img);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await ful.CopyToAsync(stream);
                        }
                        userModel.Img = userModel.Id + "." + ful.FileName.Split('.')[ful.FileName.Split('.').Length - 1];
                        _context.Update(userModel);
                    }
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if (AdminModelExists(adminModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Profile",id);
            }
            ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName", adminModel.Id);
            ViewData["Img"] = _context.User.Find(adminModel.IdUser).Img;
            return View(adminModel);
        }

        private bool AdminModelExists(int id)
        {
            return _context.Admin.Any(e => e.Id == id);
        }
    }
}
