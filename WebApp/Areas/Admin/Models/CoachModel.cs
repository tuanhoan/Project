using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class CoachModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Không Được Bỏ Trống")]
        [StringLength(maximumLength: 50)]
        [Display(Name = "Họ Và Tên")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Vui Lòng Nhập Vào 1 E-Mail Hợp Lệ")]
        [StringLength(maximumLength: 300, MinimumLength = 8)]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Không Được Bỏ Trống")]
        [StringLength(maximumLength: 300, ErrorMessage = "Địa chỉ không dài Hơn 300 Ký Tự")]
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Không Được Bỏ Trống")]
        [Display(Name = "Số Điện Thoại")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(maximumLength: 12, ErrorMessage = "Độ dài không phù hợp", MinimumLength = 10)]
        public string Phone { get; set; }

        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public virtual UserModel User { get; set; }

        public ICollection<LessonModel> Lesson { get; set; }

        public bool Status { get; set; }

        public CoachModel()
        {
            Status = true;
        }
    }
}
