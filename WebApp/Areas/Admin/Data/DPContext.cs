using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;
using WebApp.Models;

namespace WebApp.Areas.Admin.Data
{
    public class DPContext:DbContext
    {
        public DPContext(DbContextOptions<DPContext> options) 
        :base(options){ }

        public DbSet<UserModel> User { get; set; }
        public DbSet<RolesModel> Roles { get; set; }
        public DbSet<StudentModel> Student { get; set; }
        public DbSet<PostModel> Post { get; set; }
        public DbSet<LessonModel> Lesson { get; set; }
        public DbSet<CourseModel> Course { get; set; }
        public DbSet<CoachModel> Coach { get; set; }
        public DbSet<CommemtPostModel> CommemtPost { get; set; }
        public DbSet<CommemtLessonModel> CommemtLesson { get; set; }
        public DbSet<AdminModel> Admin { get; set; }
        public DbSet<AdminForumModel> AdminForum { get; set; }
    }
}
