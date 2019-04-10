using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using MinorityDashboard.Web;
using MinorityDashboard.Web.Controllers;
using MinorityDashboardWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboardWeb.Controllers
{
    // [Authorize(Roles = "3")]
    [CustomAuthorize(Roles = "3")]
    [CustomExceptionFilter]
    public class DistrictAdminController : BaseController
    {
        IDistrictAdmin objDistrictAdmin = new DistrictAdmin();
        // GET: DistrictAdmin
        public ActionResult Index()
        {

            return View(BindDistrictScheme());
        }

        public ActionResult ShowUpdate()
        {
            return View();
        }

        public ActionResult SchemeDetails()
        {
            return View();
        }

        private DistrictAdminModel BindDistrictScheme()
        {
            DistrictAdminModel sm = new DistrictAdminModel();
            sm.ddlParentScheme = BindParentScheme(1);
            sm.ddlChildScheme1 = BlankSelectItem();
            sm.ddlChildScheme2 = BlankSelectItem();
            sm.ddlChildScheme3 = BlankSelectItem();
            sm.lstFinancialYear = BindFinancialYear();
            sm.lstDistrict = BindDistrict();
            sm.lstImplementationAgency = BindImplementationAgency();
            sm.lstGetUsedSchemeAmount = objDistrictAdmin.GetUsedSchemeAmount();
            return sm;

        }


        [HttpPost]
        public ActionResult SaveSchemeUpdate(DistrictAdminModel ADM)
        {
            int flg = 0;
            district_scheme_details obj = new district_scheme_details();
            obj.actual_allocation_amt = ADM.actual_allocation_amt;
            obj.actual_expenditure_amt = ADM.actual_expenditure_amt;
            obj.actual_remaining_amt = ADM.actual_remaining_amt;
            obj.budgetary_provision_amt = ADM.budgetary_provision_amt;
            obj.fin_y_id = ADM.fin_y_id;
            obj.imp_agency_id = ADM.imp_agency_id;
            obj.parent_scheme_id = ADM.parent_scheme_id;
            obj.scheme_id_child1 = ADM.scheme_id_child1;
            obj.scheme_id_child2 = ADM.scheme_id_child2;
            obj.scheme_id_child3 = ADM.scheme_id_child3;
            obj.work_progress_desc = ADM.work_progress_desc;
            obj.des_id = ADM.des_id;

            obj.file_upload = SaveFileinFolder(ADM.FileDoc, ConfigurationManager.AppSettings["DistrictAdminDocFolder"].ToString(), ADM.des_id)[0];
            obj.utilization_certificate = SaveFileinFolder(ADM.Utilizationertificate, ConfigurationManager.AppSettings["UCDocFolder"].ToString(), ADM.des_id)[0];
            flg = objDistrictAdmin.InsertDistrictSchemeUpdate(obj);
            if (flg > 0)
            {
                Success(CommonUtility.SucessMessage);

            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            return View("Index", BindDistrictScheme());

        }



        [HttpPost]
        public ActionResult ParentChange1(string PSchemeid)
        {
            System.Threading.Thread.Sleep(1000);

            string[] arr = PSchemeid.Split('$');

            List<gp_GetAmount_Scheme_Result> lst = lstAmounts(null, Convert.ToInt32(arr[1]), Convert.ToInt32(arr[0]), null, null, null);

            List<SelectListItem> sli = BindChildScheme1(1, Convert.ToInt32(arr[0]));

            sli.Add(new SelectListItem() { Text = "BudgetrypAmount", Value = lst[0].BPA.ToString() == "" ? "0" : lst[0].BPA.ToString() });
            sli.Add(new SelectListItem() { Text = "ActualAllAmount", Value = lst[0].AAA.ToString() == "" ? "0" : lst[0].AAA.ToString() });
            return Json(sli);
        }


        [HttpPost]
        public ActionResult DistrictChange(string DistrictId)
        {
            System.Threading.Thread.Sleep(1000);
            List<gp_GetAmount_Scheme_Result> lst = lstAmounts(null,Convert.ToInt32(DistrictId),null,null, null, null);
            List<SelectListItem> sli = new List<SelectListItem>();// BindChildScheme2(1, Convert.ToInt32(arr[1]));
            sli.Add(new SelectListItem() { Text = "BudgetrypAmount", Value = lst[0].BPA.ToString() == "" ? "0" : lst[0].BPA.ToString() });
            sli.Add(new SelectListItem() { Text = "ActualAllAmount", Value = lst[0].AAA.ToString() == "" ? "0" : lst[0].AAA.ToString() });
            return Json(sli);
        }


        [HttpPost]
        public ActionResult ChildChange11(string ChildSchemeid1)
        {
            System.Threading.Thread.Sleep(1000);

            string [] arr = ChildSchemeid1.Split('$');

            List<gp_GetAmount_Scheme_Result> lst = lstAmounts(null, Convert.ToInt32(arr[2]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), null, null);

            List<SelectListItem> sli = BindChildScheme2(1, Convert.ToInt32(arr[1]));

            sli.Add(new SelectListItem() { Text = "BudgetrypAmount", Value = lst[0].BPA.ToString() == "" ? "0" : lst[0].BPA.ToString() });
            sli.Add(new SelectListItem() { Text = "ActualAllAmount", Value = lst[0].AAA.ToString() == "" ? "0" : lst[0].AAA.ToString() });
            return Json(sli);


            //return Json(BindChildScheme2(1, Convert.ToInt32(ChildSchemeid1)));
        }

        [HttpPost]
        public ActionResult ChildChange21(string ChildSchemeid2)
        {
            System.Threading.Thread.Sleep(1000);

            string[] arr = ChildSchemeid2.Split('$');

            List<gp_GetAmount_Scheme_Result> lst = lstAmounts(null, Convert.ToInt32(arr[3]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]),  Convert.ToInt32(arr[2]), null);

            List<SelectListItem> sli = BindChildScheme3(1, Convert.ToInt32(arr[1]));

            sli.Add(new SelectListItem() { Text = "BudgetrypAmount", Value = lst[0].BPA.ToString()==""?"0": lst[0].BPA.ToString() });
            sli.Add(new SelectListItem() { Text = "ActualAllAmount", Value = lst[0].AAA.ToString() == "" ? "0" : lst[0].AAA.ToString() });
            return Json(sli);


            //  return Json(BindChildScheme3(1, Convert.ToInt32(ChildSchemeid2)));
        }


        [HttpPost]
        public ActionResult ChildChange31(string ChildSchemeid3)
        {
            System.Threading.Thread.Sleep(1000);

            string[] arr = ChildSchemeid3.Split('$');

            List<gp_GetAmount_Scheme_Result> lst = lstAmounts(null, Convert.ToInt32(arr[4]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), Convert.ToInt32(arr[2]), Convert.ToInt32(arr[3]));

            List<SelectListItem> sli = new List<SelectListItem>();  //BindChildScheme3(1, Convert.ToInt32(arr[1]));

            sli.Add(new SelectListItem() { Text = "BudgetrypAmount", Value = lst[0].BPA.ToString() == "" ? "0" : lst[0].BPA.ToString() });
            sli.Add(new SelectListItem() { Text = "ActualAllAmount", Value = lst[0].AAA.ToString() == "" ? "0" : lst[0].AAA.ToString() });
            return Json(sli);


            //  return Json(BindChildScheme3(1, Convert.ToInt32(ChildSchemeid2)));
        }


        private List<gp_GetAmount_Scheme_Result> lstAmounts(int? descid, int? fyid, int? pscheme, int? childsch1, int? childsch2, int? childsch3)
        {
             List<gp_GetAmount_Scheme_Result> lst = new List<gp_GetAmount_Scheme_Result>();
            lst =  objDistrictAdmin.SPGetAmounts(2, fyid, pscheme, childsch1, childsch2, childsch3);


            return lst;


        }

    }
}