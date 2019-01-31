using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboardWeb.Models
{
    public class DistrictAdminModel
    {
        public int tran_id { get; set; }
        public int id { get; set; }
        public int scheme_id { get; set; }
        public int parent_scheme_id { get; set; }
        public int scheme_id_child1 { get; set; }
        public int scheme_id_child2 { get; set; }
        public int scheme_id_child3 { get; set; }
        public decimal budgetary_provision_amt { get; set; }
        public decimal actual_allocation_amt { get; set; }
        public decimal actual_expenditure_amt { get; set; }
        public decimal actual_remaining_amt { get; set; }
        public string work_progress_desc { get; set; }
        public string file_upload { get; set; }
        public string utilization_certificate { get; set; }
        public int createdby { get; set; }
        public DateTime createddate { get; set; }
        public int updatedby { get; set; }
        public DateTime updateddate { get; set; }


        public List<SelectListItem> ddlParentScheme { get; set; }
        public List<SelectListItem> ddlChildScheme1 { get; set; }
        public List<SelectListItem> ddlChildScheme2 { get; set; }
        public List<SelectListItem> ddlChildScheme3 { get; set; }

        public List<SelectListItem> lstFinancialYear { get; set; }
        public List<SelectListItem> lstDistrict { get; set; }
        public List<SelectListItem> lstImplementationAgency { get; set; }
        public List<GetDeskTransactionData_Result> lstDeskTransData { set; get; }
       

        public int imp_agency_id { get; set; }
        public int des_id { get; set; }
        public int fin_y_id { get; set; }


        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] FileDoc { get; set; }

        [Display(Name = "Browse Utilization Certificate")]
        public HttpPostedFileBase[] Utilizationertificate { get; set; }


    }
}