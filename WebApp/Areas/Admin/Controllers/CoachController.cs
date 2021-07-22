using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.Data;
using WebApp.Areas.Admin.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoachController : Controller
    {
        private readonly DPContext _context;

        public CoachController(DPContext context)
        {
            _context = context;
        }

        // GET: Admin/Coach
        public async Task<IActionResult> Index()
        {
            var dPContext = _context.Coach.Include(c => c.User);
            return View(await dPContext.ToListAsync());
        }

        // GET: Admin/Coach/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coachModel = await _context.Coach
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coachModel == null)
            {
                return NotFound();
            }

            return View(coachModel);
        }

        // GET: Admin/Coach/Create
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if(id == 0)
            {
                ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName");
                return View(new CoachModel());
            }
            else
            {
                var coachModel = await _context.Coach.FindAsync(id);
                if (coachModel == null)
                {
                    return NotFound();
                }
                ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName", coachModel.Id);
                return View(coachModel);
            }
           
        }

        // POST: Admin/Coach/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id,FullName,Email,Address,Phone,IdUser")] CoachModel coachModel)
        {
            if (ModelState.IsValid)
            {
                if(id == 0)
                {
                    _context.Add(coachModel);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    try
                    {
                        _context.Update(coachModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CoachModelExists(coachModel.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    } 
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Coach.ToList()) });
            }
            ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName", coachModel.IdUser);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", coachModel) });
        }

        // GET: Admin/Coach/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coachModel = await _context.Coach.FindAsync(id);
            if (coachModel == null)
            {
                return NotFound();
            }
            ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName", coachModel.IdUser);
            return View(coachModel);
        }

        // POST: Admin/Coach/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Address,Phone,IdUser")] CoachModel coachModel)
        {
            if (id != coachModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coachModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoachModelExists(coachModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName", coachModel.IdUser);
            return View(coachModel);
        }

        // GET: Admin/Coach/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coachModel = await _context.Coach
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coachModel == null)
            {
                return NotFound();
            }

            return View(coachModel);
        }

        // POST: Admin/Coach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coachModel = await _context.Coach.FindAsync(id);
            _context.Coach.Remove(coachModel);
            await _context.SaveChangesAsync();
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Coach.ToList()) });
        }

        //Get:Admin/Coach/Profile/id
        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coachModel = await _context.Coach.Where(sp => sp.IdUser == id).FirstOrDefaultAsync();
            if (coachModel == null)
            {
                return NotFound();
            }
            ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName", id);
            ViewData["Img"] = _context.User.Find(id).Img;
            return View(coachModel);
        }

        //Post:Admin/Coach/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(int id, [Bind("Id,FullName,Email,Address,Phone,IdUser")] CoachModel coachModel,IFormFile ful)
        {
            if (id != coachModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coachModel);
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
                catch (DbUpdateConcurrencyException)
                {
                    if (CoachModelExists(coachModel.Id))
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
            ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName", coachModel.Id);
            ViewData["Img"] = _context.User.Find(coachModel.IdUser).Img;
            return View(coachModel);
        }

        private bool CoachModelExists(int id)
        {
            return _context.Coach.Any(e => e.Id == id);
        }
    }
}
