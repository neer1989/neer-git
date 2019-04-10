using System.ComponentModel.DataAnnotations;

namespace MinorityDashboard.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string CaptchaOrg { get; set; }
        [Required]
        [Display(Name = "Captcha Input")]
        public string CaptchIn { get; set; }
    }

    public class RoleModel
    {


    }

    }