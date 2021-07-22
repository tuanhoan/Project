using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Data;

namespace WebApp.Areas.Admin.Validation
{
    public class RolesNameUserUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            var _context = (DPContext)validationContext.GetService(typeof(DPContext));
            var entity = _context.Roles.Where(s => s.Name == value.ToString() && s.Status == true).FirstOrDefault();
            if(entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string RolesName)
        {
            return $"Tên quyền {RolesName} đã tồn tại";
        }
    }
}
