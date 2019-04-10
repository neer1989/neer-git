using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboard.Web.Models
{
    public class LoginModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string CaptchaOrg { get; set; }
        [Required]
        [Display(Name = "Captcha Input")]
        public string CaptchIn { get; set; }


        [Required(ErrorMessage = "Employee Name is Required")]
        public string EmployeeName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email ID is Required")]
        public string EmailID { get; set; }

        public int Role_Id { get; set; }

        public bool isactive { get; set; }

        public List<SelectListItem> ddlRoleMaster { get; set; }

        public List<login> lstLoginUser { get; set; }

        //public List<rol> lstRoleMaster { get; set; }

    }
}