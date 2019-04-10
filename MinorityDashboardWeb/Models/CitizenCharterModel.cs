using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinorityDashboardWeb.Models
{
    public class CitizenCharterModel
    {

        public int cc_id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string name_e { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string name_m { get; set; }
        public string file_path { get; set; }
        public bool isactive { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_date { get; set; }

        [Display(Name = "Browse File")]
        [Required(ErrorMessage = "Please select file.")]   
        public HttpPostedFileBase[] File { get; set; }


        public List<citizen_charter> lstCitizenCharterList { get; set; }


    }
}