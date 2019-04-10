using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboardWeb.Models
{
    public class OrganizationStructureModel
    {


        public int employee_id { get; set; }
        [Required(ErrorMessage = "Employee Name is Required")]
        public string name { get; set; }
        [Required(ErrorMessage = "Designation is Required")]
        public string designation { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select reporting manager")]
        public int reporting_manager { get; set; }

        public string reporting_manager_name { get; set; }
        public bool isactive { get; set; }
        public List<org_structure> lstOrgStructure { set; get; }
        public List<SelectListItem> lstReportingManager { get; set; }
    }
}