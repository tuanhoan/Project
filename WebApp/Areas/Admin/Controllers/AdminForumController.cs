using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class AdminForumController : Controller
    {
        private readonly DPContext _context;

        public AdminForumController(DPContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminForum
        public async Task<IActionResult> Index()
        {
            var dPContext = _context.AdminForum.Where(sp => sp.Status == true).Include(a => a.User);
            return View(await dPContext.ToListAsync());
        }

        // GET: Admin/AdminForum/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminForumModel = await _context.AdminForum
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminForumModel == null)
            {
                return NotFound();
            }

            return View(adminForumModel);
        }

        // GET: Admin/AdminForum/Create
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewData["IdUser"] = new SelectList(_context.User.Where(sp => sp.Status == true), "Id", "AccountName");
                return View(new AdminForumModel());
            }
            else
            {
                var adminForumModel = await _context.AdminForum.FindAsync(id);
                if (adminForumModel == null)
                {
                    return NotFound();
                }
                ViewData["IdUser"] = new SelectList(_context.User.Where(sp => sp.Status == true), "Id", "AccountName",adminForumModel.Id);
                return View(adminForumModel);
            }
        }

        // POST: Admin/AdminForum/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id,[Bind("Id,FullName,Email,Address,Phone,IdUser")] AdminForumModel adminForumModel)
        {

            if (ModelState.IsValid)
            {
                // Insert
                if (id == 0)
                {
                    _context.Add(adminForumModel);
                    await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        _context.Update(adminForumModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AdminForumModelExists(adminForumModel.Id))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }
                var dPContext = _context.AdminForum.Where(sp => sp.Status == true).Include(a => a.User);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", dPContext.ToList()) });
            }
            ViewData["IdUser"] = new SelectList(_context.User.Where(sp => sp.Status == true), "Id", "AccountName", adminForumModel.Id);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", adminForumModel) });
        }

        // GET: Admin/AdminForum/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminForumModel = await _context.AdminForum
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminForumModel == null)
            {
                return NotFound();
            }

            return View(adminForumModel);
        }

        // POST: Admin/AdminForum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminForumModel = await _context.AdminForum.FindAsync(id);
            _context.AdminForum.Remove(adminForumModel);
            await _context.SaveChangesAsync();
            var dPContext = _context.AdminForum.Where(sp => sp.Status == true).Include(a => a.User);
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", dPContext.ToList()) });
        }

        //Get:Admin/AdminForum/Profile/id
        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var adminForumModel = await _context.AdminForum.Where(sp => sp.IdUser == id).FirstOrDefaultAsync();
            if (adminForumModel == null)
            {
                return NotFound();
            }
            ViewData["IdUser"] = new SelectList(_context.User.Where(sp => sp.Status == true), "Id", "AccountName", id);
            ViewData["Img"] = _context.User.Find(id).Img;
            return View(adminForumModel);
        }

        //Post:Admin/AdminForum/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(int id, [Bind("Id,FullName,Email,Address,Phone,IdUser")] AdminForumModel adminForumModel, IFormFile ful)
        {
            if (id != adminForumModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminForumModel);
                    var admin = await _context.Admin.FindAsync(id);
                    if (ful != null)
                    {
                        var userModel = await _context.User.FindAsync(admin.IdUser);
                        string path = null;
                        if (userModel.Img != "NoImg.jpg")
                        {
                            path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/User", userModel.Img);
                            System.IO.File.Delete(path);
                        }
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/User",
                        userModel.Id + "." + ful.FileName.Split('.')[ful.FileName.Split('.').Length - 1]);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await ful.CopyToAsync(stream);
                        }
                        userModel.Img = userModel.Id + "." + ful.FileName.Split('.')[ful.FileName.Split('.').Length - 1];
                        _context.Update(userModel);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (AdminForumModelExists(adminForumModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Profile", id);
            }
            ViewData["IdUser"] = new SelectList(_context.User.Where(sp => sp.Status == true), "Id", "AccountName", adminForumModel.Id);
            ViewData["Img"] = _context.User.Find(adminForumModel.IdUser).Img;
            return View(adminForumModel);
        }

        private bool AdminForumModelExists(int id)
        {
            return _context.AdminForum.Any(e => e.Id == id);
        }
    }
}
