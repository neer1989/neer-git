using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboardWeb.Models
{
    public class DashboardModel
    {
        public int tran_id { get; set; }
        public int desk_id { get; set; }
        public int scheme_id { get; set; }
        public int des_id { get; set; }
        public int fin_y_id { get; set; }
        public int sub_id { get; set; }
        public int est_id { get; set; }
        public decimal budgetary_provision_amt { get; set; }
        public decimal actual_allocation_amt { get; set; }

        public decimal actual_expenditure_amt { get; set; }
        public decimal actual_remaining_amt { get; set; }

        public int desk_officer_id { get; set; }

        public List<GetDeskTransactionData_Result> lstDeskTransData { set; get; }
        public List<GetDeskDocuments_Result> lstDeskDocData { set; get; }

        public List<trans_docfile> lstDeskDocFile { set; get; }
        public List<SelectListItem> lstSubject { get; set; }
        public List<SelectListItem> lstDesk { get; set; }
        public List<SelectListItem> lstScheme { get; set; }
        public List<SelectListItem> lstFinancialYear { get; set; }
        public List<SelectListItem> lstDistrict { get; set; }
        public List<SelectListItem> lstEstimatipn { get; set; }

        public FinancialYearCount FinancialYearwiseCounr { get; set; }
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] UploadDoc { get; set; }

        public string DocTitle { get; set; }
    }

    public class SubjectModel
    {
        public int sub_id { get; set; }
        public int desk_id { get; set; }
        public string sub_name { get; set; }
        public List<SelectListItem> lstDesk { get; set; }
        public List<GetSubjectData_Result> lstSubject { get; set; }
    }
    public class DeskModel
    {
        public List<desk_master> lstDeskMaster { get; set; }
        public int desk_id { get; set; }
        public string desk_name { get; set; }
    }

    public class Desk
    {
       
    }

    public class GRModel
    {
        public int gr_id { get; set; }
        public string keywords_e { get; set; }
        public string keywords_m { get; set; }
        public DateTime gr_date { get; set; }


        public DateTime grfrom_date { get; set; }
        public DateTime grto_date { get; set; }


        public string unique_code_e { get; set; }
        public string unique_code_m { get; set; }
        public bool isactive { get; set; }
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] GrFile { get; set; }

        public List<grdetail> lstGRList { get; set; }

    }

    public class LatestNews
    {
        public int latest_news_id { get; set; }
        public string latest_news_e { get; set; }
        public string latest_news_m { get; set; }

        public string news_description_e { get; set; }
        public string news_description_m { get; set; }
        public DateTime news_date { get; set; }
        public bool isactive { get; set; }

        public List<latest_news> lstLatestNews { get; set; }

    }


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

        public int parent_scheme_id { get; set; }
        public int scheme_id_child1 { get; set; }
        public int scheme_id_child2 { get; set; }
        public int scheme_id_child3 { get; set; }
        public string scheme_description_e { get; set; }
        public string scheme_description_m { get; set; }
        public int scheme_des_id { get; set; }


        public List<SelectListItem> ddlParentScheme { get; set; }
        public List<SelectListItem> ddlChildScheme1 { get; set; }
        public List<SelectListItem> ddlChildScheme2 { get; set; }
        public List<SelectListItem> ddlChildScheme3 { get; set; }


    }

   public class SchemeAmtAllotment
    {

        public int parent_scheme_id { get; set; }
        public int scheme_id_child1 { get; set; }
        public int scheme_id_child2 { get; set; }
        public int scheme_id_child3 { get; set; }

        public decimal alloted_amount { get; set; }
        public int fin_y_id { get; set; }

        public List<SelectListItem> ddlParentScheme { get; set; }
        public List<SelectListItem> ddlChildScheme1 { get; set; }
        public List<SelectListItem> ddlChildScheme2 { get; set; }
        public List<SelectListItem> ddlChildScheme3 { get; set; }
        public List<SelectListItem> ddlFinancialYear { get; set; }


        public List<parent_scheme_amt> lstParentSchemeAmt { get; set; }
        public List<scheme_child1_amt> lstChildScheme1Amt { get; set; }
        public List<scheme_child2_amt> lstChildScheme2Amt { get; set; }
        public List<scheme_child3_amt> lstChildScheme3Amt { get; set; }

        public List<DistrictList> lstDistrict { get; set; }

        public string DistrictAllotedAmt { get; set; }

    }

    public class FinancialYearModel
    {
        public int fin_y_id { get; set; }
        public string fin_y_name { get; set; }
    }

    public class DistrictModel
    {
        public int des_id { get; set; }
        public string des_name { get; set; }
    }

   public class FinancialYearCount
    {
        public decimal D1FY2017_18 { get; set; }
        public decimal D1FY2018_19 { get; set; }

        public decimal D2FY2017_18 { get; set; }
        public decimal D2FY2018_19 { get; set; }

        public decimal D3FY2017_18 { get; set; }
        public decimal D3FY2018_19 { get; set; }

        public decimal D4FY2017_18 { get; set; }
        public decimal D4FY2018_19 { get; set; }
    
        public decimal D5FY2017_18 { get; set; }
        public decimal D5FY2018_19 { get; set; }

        public decimal D6FY2017_18 { get; set; }
        public decimal D6FY2018_19 { get; set; }

        public decimal D7FY2017_18 { get; set; }
        public decimal D7FY2018_19 { get; set; }

        public decimal D8FY2017_18 { get; set; }
        public decimal D8FY2018_19 { get; set; }

        public decimal D9FY2017_18 { get; set; }
        public decimal D9FY2018_19 { get; set; }

    }

   public class ImgGallery
    {
        public int gallery_id { get; set; }
        public string title { get; set; }
        public string file_name { get; set; }
        public string file_extension { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }


    public class DistrictList
    {
        public int district_id { get; set; }
        public string districtname { get; set; }
        public decimal allotedmt { get; set; }
    }

}