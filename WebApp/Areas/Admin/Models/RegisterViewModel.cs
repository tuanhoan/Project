using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Validation;

namespace WebApp.Areas.Admin.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Không được bỏ trống")]
        [UserNameUserUniqueAttribute]
        [StringLength(maximumLength: 200, ErrorMessage = "Độ dài không phù hợp", MinimumLength = 8)]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Không được bỏ trống")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhập không giống nhau")]
        public string ConfirmPassword { get; set; }
    }
}
