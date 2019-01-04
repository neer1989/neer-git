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
        public ActionResult GRandAct()
        {
            return View();
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


    }
}