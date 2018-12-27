using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using MinorityDashboard.Web.Controllers;
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
            List<scheme_master> lstScheme = new List<scheme_master>();
            lstScheme = objDashboard.GetScheme();
            return View(lstScheme);
        }


    }
}