using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.Data;
using WebApp.Areas.Admin.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly DPContext _context;

        public UserController(DPContext context)
        {
            _context = context;
        }

        // GET: Admin/User
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var dPContext = _context.User.Where(sp => sp.Status == true).Include(u => u.Roles);
            return View(await dPContext.ToListAsync());
        }

        // GET: Admin/User/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.User
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // GET: Admin/User/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["IdRoles"] = new SelectList(_context.Roles.Where(sp => sp.Status == true), "Id", "Name");
            return View();
        }

        // POST: Admin/User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,AccountName,AccountPassword,Img,IdRoles")] UserModel userModel,IFormFile ful)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/User",
                    userModel.Id + "." + ful.FileName.Split('.')[ful.FileName.Split('.').Length - 1]);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ful.CopyToAsync(stream);
                }
                userModel.Img = userModel.Id + "." + ful.FileName.Split('.')[ful.FileName.Split('.').Length - 1];
                await _context.SaveChangesAsync();
                var user = _context.User.Where(sp => sp.Id == userModel.Id).Include(s => s.Roles).FirstOrDefault();
                switch (userModel.Roles.Name)
                {
                    case "Admin": 
                        {
                            AdminModel adminModel = new AdminModel();
                            adminModel.FullName = user.AccountName;
                            adminModel.Phone = "0000000000";
                            adminModel.IdUser = user.Id;
                            adminModel.Address = user.AccountName;
                            adminModel.Email = user.AccountName;
                            _context.Add(adminModel);
                            await _context.SaveChangesAsync();
                        }
                        break;
                    case "Coach":
                        {
                            CoachModel coachModel = new CoachModel();
                            coachModel.FullName = user.AccountName;
                            coachModel.Phone = "0000000000";
                            coachModel.IdUser = user.Id;
                            coachModel.Address = user.AccountName;
                            coachModel.Email = user.AccountName;
                            _context.Add(coachModel);
                            await _context.SaveChangesAsync();
                        }
                        break;
                    case "AdminForum":
                        {
                            AdminForumModel adminForumhModel = new AdminForumModel();
                            adminForumhModel.FullName = user.AccountName;
                            adminForumhModel.Phone = "0000000000";
                            adminForumhModel.IdUser = user.Id;
                            adminForumhModel.Address = user.AccountName;
                            adminForumhModel.Email = user.AccountName;
                            _context.Add(adminForumhModel);
                            await _context.SaveChangesAsync();
                        }
                        break;
                    case "Student":
                        {
                            StudentModel studentModel = new StudentModel();
                            studentModel.FullName = user.AccountName;
                            studentModel.Phone = "0000000000";
                            studentModel.IdUser = user.Id;
                            studentModel.Address = user.AccountName;
                            studentModel.Email = user.AccountName;
                            _context.Add(studentModel);
                            await _context.SaveChangesAsync();
                        }
                        break;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRoles"] = new SelectList(_context.Roles.Where(sp => sp.Status == true), "Id", "Name", userModel.IdRoles);
            return View(userModel);
        }

        // GET: Admin/User/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.User.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            ViewData["IdRoles"] = new SelectList(_context.Roles.Where(sp => sp.Status == true), "Id", "Name", userModel.IdRoles);
            return View(userModel);
        }

        // POST: Admin/User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id,[Bind("Id,AccountName,AccountPassword,Img,IdRoles")] UserModel userModel, IFormFile ful)
        {
            if (id != userModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ful != null)
                    {

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
                    _context.Update(userModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(userModel.Id))
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
            ViewData["IdRoles"] = new SelectList(_context.Roles.Where(sp => sp.Status == true), "Id", "Name", userModel.IdRoles);
            return View(userModel);
        }

        // GET: Admin/User/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.User
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userModel = await _context.User.FindAsync(id);
            _context.User.Remove(userModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool UserModelExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
        public IActionResult Login(string requestPath)
        {
            ViewBag.RequestPath = requestPath ?? "/";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("UserName,Password,Remember,RequestPath")] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _context.User.Where(
                    s => s.AccountName == model.UserName && s.AccountPassword == model.Password && s.Status == true
                ).FirstOrDefault();
                if (result == null)
                {
                    ViewData["LoginError"] = "Tài khoản hoặc mật khẩu không đúng";
                    return View();
                }
                var Roles = _context.Roles.Find(result.IdRoles);
                if (Roles == null)
                {
                    return NotFound();
                }
                // create claims
                List<Claim> claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, result.AccountName),
                    new Claim(ClaimTypes.Role, Roles.Name),
                    new Claim("id", result.Id.ToString()),
                    new Claim("img",result.Img),
            };

                // create identity
                ClaimsIdentity identity = new ClaimsIdentity(claims, model.UserName);

                // create principal
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                // sign-in
                if (model.Remember)
                {
                    await HttpContext.SignInAsync(
                            scheme: "SecurityScheme",
                            principal: principal,
                            properties: new AuthenticationProperties
                            {
                                IsPersistent = true, // for 'remember me' feature
                            });
                }
                else
                {
                    await HttpContext.SignInAsync(
                            scheme: "SecurityScheme",
                            principal: principal,
                            properties: new AuthenticationProperties
                            {
                                IsPersistent = true, // for 'remember me' feature
                                ExpiresUtc = DateTime.UtcNow.AddDays(10)
                            });
                }
                return Redirect(model.RequestPath ?? "/");
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = new UserModel();
                user.AccountName = model.UserName;
                user.AccountPassword = model.Password;
                user.Img = "NoImg.jpg";
                user.IdRoles = 1;
                _context.Add(user);
                await _context.SaveChangesAsync();
                var result = _context.User.Where(
                    s => s.AccountName == model.UserName && s.AccountPassword == model.Password
                ).FirstOrDefault();
                var Roles = _context.Roles.Find(result.IdRoles);
                if (result == null)
                {
                    return View();
                }
                // create claims
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.AccountName),
                    new Claim(ClaimTypes.Role, Roles.Name),
                    new Claim("id", result.Id.ToString()),
                    new Claim("img",result.Img),
                };

                // create identity
                ClaimsIdentity identity = new ClaimsIdentity(claims, model.UserName);

                // create principal
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                // sign-in
                await HttpContext.SignInAsync(
                        scheme: "SecurityScheme",
                        principal: principal,
                        properties: new AuthenticationProperties
                        {
                            IsPersistent = true, // for 'remember me' feature
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                        });
                return RedirectToRoute("Home");
            }
            return View();
        }
        public async Task<IActionResult> Logout(string requestPath)
        {
            await HttpContext.SignOutAsync(
                    scheme: "SecurityScheme");

            return RedirectToAction("Login"); 
        }

        public IActionResult Access()
        {
            return View();
        }
    }
}
