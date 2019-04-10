using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinorityDashboardWeb.Models
{
    public class GRModel
    {
        public int gr_id { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public string keywords_e { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public string keywords_m { get; set; }
        [Required(ErrorMessage = "GR Date is Required")]
        public DateTime gr_date { get; set; }

        [Required(ErrorMessage = "GR from date is Required")]
        public DateTime grfrom_date { get; set; }
        [Required(ErrorMessage = "GR to date is Required")]
        public DateTime grto_date { get; set; }

        [Required(ErrorMessage = "Unicode is Required")]
        public string unique_code_e { get; set; }
        [Required(ErrorMessage = "Unicode is Required")]
        public string unique_code_m { get; set; }
        public bool isactive { get; set; }
        [Display(Name = "Browse File")]
        [Required(ErrorMessage = "Please select file.")]
        public HttpPostedFileBase[] GrFile { get; set; }

        public List<grdetail> lstGRList { get; set; }

    }
}