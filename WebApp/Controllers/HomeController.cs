using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApp.Areas.Admin.Data;
using WebApp.Areas.Admin.Models;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DPContext _context;

        public HomeController(DPContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> IndexCourse(string search)
        {
            List<CourseModel> listCourse = null;
            if (search != null)
            {
                listCourse = new List<CourseModel>();
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync("https://localhost:44379/api/CourseAPI/name/" + search))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        listCourse = JsonConvert.DeserializeObject<List<CourseModel>>(apiResponse);
                    }
                }
            }
            else
            {
                listCourse = new List<CourseModel>();
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync("https://localhost:44379/api/CourseAPI"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        listCourse = JsonConvert.DeserializeObject<List<CourseModel>>(apiResponse);
                    }
                }
            }
            if (listCourse == null)
            {
                return NotFound();
            }
            return View(listCourse);
        }

        public async Task<IActionResult> DetailCourse(int id)
        {
            List<LessonModel> listLessonModel = new List<LessonModel>();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44379/api/LessonAPI/" + id))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listLessonModel = JsonConvert.DeserializeObject<List<LessonModel>>(apiResponse);
                }
            }
            int idLesson = listLessonModel.First().Id;
            List<CommemtLessonModel> listCommentLesson = new List<CommemtLessonModel>();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44379/api/CommemtLessonAPI/" + idLesson))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listCommentLesson = JsonConvert.DeserializeObject<List<CommemtLessonModel>>(apiResponse);
                }
            }
            if (listLessonModel == null)
            {
                return NotFound();
            }
            ViewBag.ListCom = listCommentLesson;
            return View(listLessonModel);
        }


        public async Task<IActionResult> DetailLesson(int? id)
        {
            LessonModel lessonModel = new LessonModel();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44379/api/LessonAPIModels/" + id))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    lessonModel = JsonConvert.DeserializeObject<LessonModel>(apiResponse);
                }
            }
            List<LessonModel> listLessonModel = new List<LessonModel>();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44379/api/LessonAPI/" + lessonModel.IdCourse))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listLessonModel = JsonConvert.DeserializeObject<List<LessonModel>>(apiResponse);
                }
            }

            List<CommemtLessonModel> listCommentLesson = new List<CommemtLessonModel>();
            using (var client = new HttpClient())
            {
                //Sua cho nay thanh id
                using (var response = await client.GetAsync("https://localhost:44379/api/CommemtLessonAPI/" + id))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listCommentLesson = JsonConvert.DeserializeObject<List<CommemtLessonModel>>(apiResponse);
                }
            }
            if (lessonModel == null)
            {
                return NotFound();
            }
            ViewBag.ListLesson = listLessonModel;
            ViewBag.ListComment = listCommentLesson;
            ViewData["IdLesson"] = id;
            return View(lessonModel);
        }

        public IActionResult IndexPost()
        {
            var listPost = from s in _context.Post
                           join p in _context.User on
                           s.IdUser equals p.Id into muserGroup
                           from m in muserGroup.DefaultIfEmpty()
                           join roles in _context.Roles on
                           m.IdRoles equals roles.Id into mgr
                           from k in mgr.DefaultIfEmpty()
                           where s.Status == true && m.Status == true &&
                           k.Status == true
                           select new PostIndex
                           {
                               idPost = s.Id,
                               Title = s.Title,
                               Description = s.Descripsion,
                               Img = m.Img,
                               roles = k.Name,
                           };
            ViewBag.ListCom = _context.CommemtPost.ToList();

            return View(listPost.ToList());
        }
        public IActionResult DetailPost(int id)
        {
            var postdetail = from s in _context.Post
                           join p in _context.User on
                           s.IdUser equals p.Id into muserGroup
                           from m in muserGroup.DefaultIfEmpty()
                           join roles in _context.Roles on
                           m.IdRoles equals roles.Id into mgr
                           from k in mgr.DefaultIfEmpty()
                           where s.Status == true && m.Status == true &&
                           k.Status == true && s.Id == id
                           select new PostIndex
                           {
                               idPost = s.Id,
                               name = m.AccountName,
                               Title = s.Title,
                               Description = s.Descripsion,
                               Img = m.Img,
                               roles = k.Name,
                               Content = s.Content
                           };
            ViewBag.ListComment = _context.CommemtPost.Where(s => s.IdPost == id).Include(sp => sp.User).ToList();
            return View(postdetail.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult chat()
        {
            return View();
        }
        public class PostIndex
        {
            public string Img;
            public string Title;
            public string Description;
            public string roles;
            public int countComment;
            public int idPost;
            public string Content;
            public string name;
        }
    }
}
