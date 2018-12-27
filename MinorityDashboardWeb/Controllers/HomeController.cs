using MinorityDashboard.Data;
using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using MinorityDashboardWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboard.Web.Controllers
{
    public class HomeController : BaseController
    {
        IDashboard objDashboard = new Dashboard();
        [AllowAnonymous]
        public ActionResult Index()
        {
            //Success(string.Format("<b>{0}</b> was successfully added to the database.", "Niraj"), true);

            //InsertMenu();

            return View(BindDropdowns());
        }

        private DashboardModel BindDropdowns()
        {

            DashboardModel objDashboardM = new DashboardModel();
            objDashboardM.lstDesk = BindDesk();
            objDashboardM.lstDistrict = BindDistrict();
            objDashboardM.lstFinancialYear = BindFinancialYear();
            objDashboardM.lstScheme = BindScheme();
            objDashboardM.lstSubject = BindSubject();
            objDashboardM.lstEstimatipn = BindEstimation();
            objDashboardM.lstDeskTransData = objDashboard.GetTransDeskData();
            FinancialYearCount ObjFYC = new FinancialYearCount();

            ObjFYC.D1FY2017_18 = CountFYAmount(2, 1)[0];
            objDashboardM.FinancialYearwiseCounr = ObjFYC;
            return objDashboardM;

        }

        private List<decimal> CountFYAmount(int fyid, int deskid)
        {
            decimal commitedamount = 0;
            decimal schemeamount = 0;

            List<decimal> lstfamount = new List<decimal>();

            DashboardModel objDashboardM = new DashboardModel();
            objDashboardM.lstDeskTransData = objDashboard.GetTransDeskData();
            var lstamt = objDashboardM.lstDeskTransData.Where(s => s.fin_y_id == fyid && s.desk_id == deskid);

            foreach (var x in lstamt)
            {
                commitedamount = commitedamount + Convert.ToDecimal(x.budgetary_provision_amt);
                schemeamount = schemeamount + Convert.ToDecimal(x.actual_allocation_amt);

            }


            lstfamount.Add(commitedamount);
            lstfamount.Add(schemeamount);

            return lstfamount;

        }


        [AllowAnonymous]
        //  [Authorize(Roles = "1")]
        public ActionResult DashboardData()
        {
            int deskid = Convert.ToInt32(Request["desk_id"]);
            int subid = Convert.ToInt32(Request["sub_id"]);
            int schemeid = Convert.ToInt32(Request["scheme_id"]);
            int districtid = Convert.ToInt32(Request["des_id"]);
            // DashboardModel objDashboardM = new DashboardModel();
            // objDashboardM.lstDeskTransData = objDashboard.GetTransDeskData();
            // var lstamt = objDashboardM.lstDeskTransData.Where(s => s.fin_y_id == fyid && s.desk_id == deskid);
            return View(BindDropdownsFilterData(deskid, subid, schemeid, districtid));
        }

        [AllowAnonymous]
        public ActionResult DeskDocuments()
        {
            DashboardModel objDashboardM = BindDropdowns();
            objDashboardM.lstDeskDocData = objDashboard.GetDeskDocumentsData();
            return View(objDashboardM);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetDocuments(int deskdocid)
        {
            DashboardModel objDashboardM = BindDropdowns();
            objDashboardM.lstDeskDocFile = objDashboard.GetTransactionFile(deskdocid);
            return PartialView("../Admin/FileDetails", objDashboardM);
        }

        private DashboardModel BindDropdownsFilterData(int deskid, int subid, int schemeid, int districtid)
        {

            DashboardModel objDashboardM = new DashboardModel();

            var lstDeskData = objDashboard.GetTransDeskData();

            if (deskid > 0)
            {

                lstDeskData = lstDeskData.Where(s => s.desk_id == deskid).ToList();
            }
            if (subid > 0)
            {

                lstDeskData = lstDeskData.Where(s => s.sub_id == subid).ToList();
            }
            if (schemeid > 0)
            {

                lstDeskData = lstDeskData.Where(s => s.scheme_id == schemeid).ToList();
            }
            if (districtid > 0)
            {

                lstDeskData = lstDeskData.Where(s => s.des_id == districtid).ToList();
            }

            objDashboardM.lstDeskTransData = lstDeskData;
            return objDashboardM;

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TestDemo()
        {
         
            double P = 100000;
            int n = 12;
            double Rate = 10;
            // Intrest Permonth
           
            double r = Rate / 1200;
           
        
            double E = Math.Round(P * r * Math.Pow(1 + r, n) / (Math.Pow(1 + r, n) - 1),2);

            double monnthlyPAmount = P;


            List<string> lst = new List<string>();

            lst.Add("Total Amount --> " + (E*n));

            for(int i = 1; i<= 12; i++)
            {
                double IPM = (monnthlyPAmount * Rate * 1) / 100;

                double emintrestpmonth = Math.Round(IPM / n,2);

                double OneMonthPAmount = E - emintrestpmonth;

                monnthlyPAmount = Convert.ToInt32(monnthlyPAmount - OneMonthPAmount);

                lst.Add(E + "," + emintrestpmonth + "," + OneMonthPAmount + "," + monnthlyPAmount);


            }
            

            return View();
        }

    }
    public class ControllerActions
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Attributes { get; set; }
        public string ReturnType { get; set; }
    }

}