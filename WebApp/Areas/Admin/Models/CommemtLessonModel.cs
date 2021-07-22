using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class CommemtLessonModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 300, ErrorMessage = "Độ Dài khong phù hợp", MinimumLength = 16)]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }
        public int IdLesson { get; set; }

        [ForeignKey("IdLesson")]
        public virtual LessonModel Lesson { get; set; }
        public int IdUser { get; set; }

        [ForeignKey("IdUser")]

        public virtual UserModel User { get; set; }
        public bool Status { get; set; }

        public CommemtLessonModel()
        {
            Status = true;
        }

    }
}

