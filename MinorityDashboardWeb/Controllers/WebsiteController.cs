using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using MinorityDashboard.Web.Controllers;
using MinorityDashboardWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;

namespace MinorityDashboardWeb.Controllers
{
    [AllowAnonymous]
    public class WebsiteController : BaseController
    {
        IDashboard objDashboard;
        IUnityContainer unitycontainer = new UnityContainer();
        // GET: Website

        public WebsiteController()
        {
            unitycontainer.RegisterType<IDashboard, Dashboard>();
            objDashboard = unitycontainer.Resolve<Dashboard>();
        }

        public ActionResult Index()
        {
            var url = Request.RawUrl;
            if (url == @"/")
            {
                Response.Redirect("/Website/Home");
            }

            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult CitizenCharter()
        {
            return View();
        }


        public ActionResult Division()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GRandAct()
        {
            GRModel GRM = new GRModel();

            GRM.grfrom_date = DateTime.Now;
            GRM.grto_date = DateTime.Now;

            GRM.lstGRList = objDashboard.GetGRList();
            return View(GRM);
        }
        [HttpPost]
        public ActionResult GRandAct(GRModel obj)
        {
            GRModel GRM = new GRModel();

            GRM.grfrom_date = DateTime.Now;
            GRM.grto_date = DateTime.Now;

            GRM.lstGRList = objDashboard.GetGRList();

            if (obj.keywords_e != "" && obj.keywords_e != null)
            {
                GRM.lstGRList = GRM.lstGRList.Where(s => s.unique_code_e == obj.keywords_e).ToList();
            }
            if (Convert.ToString(obj.grfrom_date) != "")
            {
                GRM.lstGRList = GRM.lstGRList.Where(s => s.gr_date>= obj.grfrom_date).ToList();
            }
            if (Convert.ToString(obj.grto_date) != "")
            {
                GRM.lstGRList = GRM.lstGRList.Where(s => s.gr_date <= obj.grto_date).ToList();
            }

            return View(GRM);
        }

        public ActionResult OrganizationStructure()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }

        public ActionResult NewsRoom()
        {
            return View();
        }
        public ActionResult DepartmentInformation()
        {
            return View();
        }

        public ActionResult Schemes()
        {
            SchemeModel sm = new SchemeModel();


            sm.lstParentScheme = objDashboard.GetParentScheme();
            sm.lstChildScheme1 = objDashboard.GetSchemeChild1();
            sm.lstChildScheme2 = objDashboard.GetSchemeChild2();
            sm.lstChildScheme3 = objDashboard.GetSchemeChild3();


            //List<scheme_master> lstScheme = new List<scheme_master>();
            //lstScheme = objDashboard.GetScheme();
            return View(sm);
        }


        [HttpGet]
        public ActionResult SchemeDescription(string schids)
        {
            string[] SchemIdsArr = schids.Split('/');
            SchemeModel sm = new SchemeModel();

            int ParentID = Convert.ToInt32(SchemIdsArr[0]);

            int Childschm1 = Convert.ToInt32(SchemIdsArr[1]);
            int Childschm2 = Convert.ToInt32(SchemIdsArr[2]);
            int Childschm3 = Convert.ToInt32(SchemIdsArr[3]);

            sm.lstSchemeDesc = objDashboard.GetFilteredSchemeDesc(ParentID, Childschm1, Childschm2, Childschm3);


            return View(sm);
        }


    }
}