using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.Data;
using WebApp.Areas.Admin.Validation;

namespace WebApp.Areas.Admin.Controllers
{
    [AuthorizeRoles("Admin","Coach", "AdminForum")]
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        private readonly DPContext _context;

        public HomeAdminController(DPContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Statistical(int id = 0)
        {
            var lesson = _context.Lesson.Include(s => s.CommemtLessons);
            var listLesson = from s in lesson
                             where s.IdCourse ==
                             (from sp in _context.Course
                              where sp.Id == id
                              select sp.Id).FirstOrDefault()
                             orderby s.CommemtLessons.Count descending
                             select s;
            ViewData["IdCourse"] = new SelectList(_context.Course, "Id", "Title");

            return View(listLesson);
        }
    }
  
}
