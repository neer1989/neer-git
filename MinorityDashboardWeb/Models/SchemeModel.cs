using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboardWeb.Models
{
    public class SchemeModel
    {
        public int scheme_id { get; set; }
        public string scheme_name { get; set; }
        public string scheme_description { get; set; }
        public int sub_id { get; set; }
        public int desk_id { get; set; }
        public List<SelectListItem> lstSubject { get; set; }
        public List<SelectListItem> lstDesk { get; set; }
        public List<GetSchemeData_Result> lstSchems { get; set; }

        public List<parentscheme> lstParentScheme { get; set; }
        public List<scheme_child1> lstChildScheme1 { get; set; }
        public List<scheme_child2> lstChildScheme2 { get; set; }
        public List<scheme_child3> lstChildScheme3 { get; set; }
        public List<GetSchemeDesc_Result> lstSchemeDesc { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select Parent Scheme")]
        public int parent_scheme_id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select Child Scheme")]
        public int scheme_id_child1 { get; set; }
        public int scheme_id_child2 { get; set; }
        public int scheme_id_child3 { get; set; }
        public string scheme_description_e { get; set; }
        public string scheme_description_m { get; set; }
        public int scheme_des_id { get; set; }

        public bool isactive { get; set; }
        public List<SelectListItem> ddlParentScheme { get; set; }
        public List<SelectListItem> ddlChildScheme1 { get; set; }
        public List<SelectListItem> ddlChildScheme2 { get; set; }
        public List<SelectListItem> ddlChildScheme3 { get; set; }


    }

}