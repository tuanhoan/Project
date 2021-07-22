using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class LessonModel
    {
        [Key]
        public int Id { set; get; }

        [Required(ErrorMessage = "Nhập tên bài giảng")]
        [StringLength(maximumLength: 300, ErrorMessage = "Độ dài không phù hợp", MinimumLength = 30)]
        [Display(Name = "Lesson Name")]
        public string LessonName { get; set; }

        [Required]
        [Display(Name = "Id Course")]   
        public int IdCourse { get; set; }

        [Required]
        [Display(Name = "Id Coach")]
        public int IdCoach { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(maximumLength: 300, ErrorMessage = "Độ dài không vượt quá 300 ký tự")]    
        public string Title { get; set; }

        [Required]
        [Display(Name = "Video")]
        public string Video { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(maximumLength: 300, ErrorMessage = "Độ dài không phù hợp")]
        public string Description { get; set; }

        [ForeignKey("IdCourse")]
        public virtual CourseModel Course { get; set; }
        [ForeignKey("IdCoach")]
        public virtual CoachModel Coach { get; set; }
        public ICollection<CommemtLessonModel> CommemtLessons { get; set; }
        public bool Status { get; set; }

        public LessonModel()
        {
            Status = true;
        }

    }
}
