using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HNS.Areas.Admin.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "현재 비밀번호")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 는 최소 {2} 최대 {1} 입력하실수 있습니다. ", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "새로운 비밀번호")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "비밀번호 확인")]
        [Compare("NewPassword", ErrorMessage = "비밀번호가  일치하지 않습니다.")]
        public string ConfirmPassword { get; set; }
    }
}
