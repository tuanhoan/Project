using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Admin.Models
{
    public class CommemtPostModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 300, ErrorMessage = "Độ Dài Vượt Qúa 300 Ký Tự", MinimumLength = 16)]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }
        public int IdPost { get; set; }
        [ForeignKey("IdPost")]
        public virtual PostModel Post { get; set; }
        public int IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual UserModel User { get; set; }
        public bool Status { get; set; }

        public CommemtPostModel()
        {
            Status = true;
        }

    }
}
