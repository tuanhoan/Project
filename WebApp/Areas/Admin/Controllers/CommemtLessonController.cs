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
    public class CommemtLessonController : Controller
    {
        private readonly DPContext _context;

        public CommemtLessonController(DPContext context)
        {
            _context = context;
        }

        // GET: Admin/CommemtLesson
        public async Task<IActionResult> Index()
        {
            var dPContext = _context.CommemtLesson.Where(sp => sp.Status == true).Include(c => c.Lesson).Include(c => c.User);
            return View(await dPContext.ToListAsync());
        }

        // GET: Admin/CommemtLesson/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commemtLessonModel = await _context.CommemtLesson
                .Include(c => c.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commemtLessonModel == null)
            {
                return NotFound();
            }

            return View(commemtLessonModel);
        }



        // GET: Admin/CommemtLesson/AddOrEdit/id?
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewData["IdLesson"] = new SelectList(_context.Lesson, "Id", "Content");
                ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName");
                return View(new CommemtLessonModel());
            }
            else
            {
                var commemtLessonModel = await _context.CommemtLesson.FindAsync(id);
                if (commemtLessonModel == null)
                {
                    return NotFound();
                }
                ViewData["IdLesson"] = new SelectList(_context.Lesson, "Id", "Content");
                ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName");
                return View(commemtLessonModel);
            }

            
        }

        // POST: Admin/CommemtLesson/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id,Title,Content,IdLesson,IdUser")] CommemtLessonModel commemtLessonModel)
        {

            if (ModelState.IsValid)
            {
                if(id == 0)
                {
                    _context.Add(commemtLessonModel);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        _context.Update(commemtLessonModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CommemtLessonModelExists(commemtLessonModel.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                var dPContext = _context.CommemtLesson.Where(sp => sp.Status == true).Include(c => c.Lesson).Include(c => c.User);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", dPContext.ToList()) });
            }
            ViewData["IdLesson"] = new SelectList(_context.Lesson, "Id", "Content", commemtLessonModel.IdLesson);
            ViewData["IdUser"] = new SelectList(_context.User, "Id", "AccountName", commemtLessonModel.IdUser);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit",commemtLessonModel) });
        }

        // GET: Admin/CommemtLesson/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commemtLessonModel = await _context.CommemtLesson
                .Include(c => c.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commemtLessonModel == null)
            {
                return NotFound();
            }

            return View(commemtLessonModel);
        }

        // POST: Admin/CommemtLesson/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commemtLessonModel = await _context.CommemtLesson.FindAsync(id);
            _context.CommemtLesson.Remove(commemtLessonModel);
            await _context.SaveChangesAsync();
            var dPContext = _context.CommemtLesson.Where(sp => sp.Status == true).Include(c => c.Lesson).Include(c => c.User);
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", dPContext.ToList()) });
        }

        private bool CommemtLessonModelExists(int id)
        {
            return _context.CommemtLesson.Any(e => e.Id == id);
        }

        //Thong ke cmt cua moi bai viet
        public async Task<IActionResult> ThongKeCommentLesson(int id)
        {
            ViewData["LessonList"] = new SelectList(_context.Lesson, "Id", "LessonName");
            ViewData["trangthai"] = null;
            if (id == null)
            {
                ViewData["trangthai"] = false;
                return View();
            }
            ViewData["trangthai"] = true;
            var dPContext = (from cmt in _context.CommemtLesson
                             from lesson in _context.Lesson
                             from user in _context.User
                             join c in _context.Student on user.Id equals c.IdUser into studentGroup
                             from student in studentGroup.DefaultIfEmpty()
                             where 
                             cmt.IdLesson == lesson.Id && lesson.Id == id &&
                             user.Id == cmt.IdUser && 
                             user.Status == true && lesson.Status == true && cmt.Status == true
                             select new ThongKe
                             {
                                 LessonName = lesson.LessonName,
                                 Content = cmt.Content,
                                 FullName = student.FullName,
                                 AccountName = user.AccountName
                             }
                             );
            ViewBag.Data = dPContext;
            return View();

        }
    }

    public class ThongKe
    {
        public string LessonName;
        public string Content;
        public string FullName;
        public string AccountName;
    }
}
