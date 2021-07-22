using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;

namespace WebApp.Areas.Admin.Models
{
    public class StudentModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui Lòng Điền Đủ Thông Tin")]
        [StringLength(maximumLength: 50, ErrorMessage = "Họ Tên Dài Hơn 8 Ký Tự", MinimumLength = 8)]
        [Display(Name = "Họ Và Tên")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Vui Lòng Nhập Vào 1 E-Mail Hợp Lệ")]
        [StringLength(maximumLength: 300, MinimumLength = 8)]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui Lòng Điền Đủ Thông Tin")]
        [StringLength(maximumLength: 300, ErrorMessage = "Địa Chỉ Dài Hơn 8 Ký Tự", MinimumLength = 8)]
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui Lòng Điền Đủ Thông Tin")]
        [Display(Name = "Số Điện Thoại")]
        [StringLength(maximumLength: 10, ErrorMessage = "Số Điện Thoại Phải Đủ 10 Chữ Số", MinimumLength = 10)]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Vui Lòng Nhập Số")]
        public string Phone { get; set; }

        public int IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual UserModel User { get; set; }

        public bool Status { get; set; }

        public StudentModel()
        {
            Status = true;
        }

    }
}