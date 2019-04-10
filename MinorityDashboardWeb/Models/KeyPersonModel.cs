using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinorityDashboardWeb.Models
{
    public class KeyPersonModel
    {
        public int keyperson_id { get; set; }
        [Required(ErrorMessage = "Key Person Name is Required")]
        public string person_name_e { get; set; }
        [Required(ErrorMessage = "Key Person Name is Required")]
        public string person_name_m { get; set; }
        [Required(ErrorMessage = "Key Person Designation is Required")]
        public string designation_e { get; set; }
        [Required(ErrorMessage = "Key Person Designation is Required")]
        public string designation_m { get; set; }
        public string person_image { get; set; }
        [Required(ErrorMessage = "Display Order is Required")]
        public int display_order { get; set; }
        public bool isactive { get; set; }

        [Display(Name = "Browse File")]
        [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase[] File { get; set; }

        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_date { get; set; }

        public List<keyperson_master> lstKeyPersonList { get; set; }
    }
}