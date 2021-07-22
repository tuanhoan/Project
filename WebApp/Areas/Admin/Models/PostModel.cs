using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class PostModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập Đầy Đủ Thông Tin")]
        [StringLength(200, MinimumLength = 16, ErrorMessage = "Độ Dài Phải Từ {2} Đến {1} Kí Tự" )]
        [Display(Name = "Tựa Đề")]
        public string Title { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "Độ Dài Phải Không Quá {1} Ký Tự")]
        [Display(Name = "Mô Tả")]
        public string Descripsion { get; set; }

        [Display(Name = "Nội Dung")]
        public string Content { get; set; }

        [Display(Name = "Tài khoản")]
        public int IdUser { get; set; }
        [ForeignKey("IdUser")]

        public virtual UserModel User { get; set; }
        public ICollection<CommemtPostModel> CommemtPosts { get; set; }

        public bool Status { get; set; }

        public PostModel()
        {
            Status = true;
        }
    }
}
