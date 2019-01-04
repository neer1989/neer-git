using MinorityDashboard.Data;
using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using MinorityDashboardWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboard.Web.Controllers
{
    public class BaseController : Controller
    {
        IDashboard objDashboard = new Dashboard();
        IMenuManager objMenuManager = new MenuManager();

        //private static string _cookieLangName = "LangChange";

        // GET: Base
        public void Success(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }
        public List<SelectListItem> BindEstimation()
        {
            List<estimation_master> lst = objDashboard.GetEstimation();
            List<SelectListItem> lstddl = new List<SelectListItem>();
            lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });
            foreach (estimation_master X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.est_id.ToString(), Text = X.est_type });
            }
            return lstddl;
        }


        public List<SelectListItem> BindSubject()
        {
            List<subject_master> lst = objDashboard.GetSubject();
            List<SelectListItem> lstddl = new List<SelectListItem>();
            lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });
            foreach (subject_master X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.sub_id.ToString(), Text = X.sub_name });
            }
            return lstddl;
        }

        public List<SelectListItem> BindScheme()
        {
            List<scheme_master> lst = objDashboard.GetScheme();
            List<SelectListItem> lstddl = new List<SelectListItem>();
            lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });
            foreach (scheme_master X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.scheme_id.ToString(), Text = X.scheme_name });
            }
            return lstddl;
        }
        public List<SelectListItem> BindDesk()
        {
            List<desk_master> lst = objDashboard.GetDesk();
            List<SelectListItem> lstddl = new List<SelectListItem>();
            lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });
            foreach (desk_master X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.desk_id.ToString(), Text = X.desk_name });
            }
            return lstddl;
        }

        public List<SelectListItem> BindDistrict()
        {
            List<district_master> lst = objDashboard.GetDistrict();
            List<SelectListItem> lstddl = new List<SelectListItem>();
            lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });
            foreach (district_master X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.des_id.ToString(), Text = X.des_name });
            }
            return lstddl;
        }

        public List<SelectListItem> BindFinancialYear()
        {
            List<financialyear_master> lst = objDashboard.GetFinancial();
            List<SelectListItem> lstddl = new List<SelectListItem>();
            lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });
            foreach (financialyear_master X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.fin_y_id.ToString(), Text = X.fin_y_name });
            }
            return lstddl;
        }

        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        public void InsertMenu()
        {
            Assembly asm = Assembly.GetAssembly(typeof(MinorityDashboard.Web.MvcApplication));

            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        ReturnType = x.ReturnType.Name,
                        Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
                    })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            var list = new List<ControllerActions>();
            CommonRepository ObjCR = new CommonRepository();
            foreach (var item in controlleractionlist)
            {
                list.Add(new ControllerActions()
                {
                    Controller = item.Controller,
                    Action = item.Action,
                    Attributes = item.Attributes,
                    ReturnType = item.ReturnType
                });
                //  myString.Substring(myString.Length - 3)
                string purl = item.Controller.Remove(item.Controller.Length - 10) + "/" + item.Action;
                sub_menu_master obj = new sub_menu_master();
                obj.createdby = 1;
                obj.createddate = DateTime.Now;
                obj.isactive = true;
                obj.menu_id = 0;
                obj.page_url = purl;
                obj.sub_menu_id = 0;
                obj.sub_menu_name = "N";
                obj.sub_menu_no = 0;
                obj.updatedby = 1;
                obj.updateddate = DateTime.Now;

                if (objMenuManager.GetSubMenuList().Where(s => s.page_url == purl).ToList().Count < 1)
                {

                    ObjCR.SaveData<sub_menu_master>(obj);
                }
            }

            //public List<main_menu_master> GetMainMenuList()
            //{
            //    return ObjCR.GetData<main_menu_master>();
            //}
            // sub_menu_master returnobj = ObjCR.SaveData<sub_menu_master>(obj);
        }



        public int GetUidbyClaim()
        {
            List<Claim> lst = HttpContext.GetOwinContext().Authentication.User.Claims.ToList();

            return Convert.ToInt32(lst[2].Value);
        }


        //protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        //{
        //    string lang = null;
        //    HttpCookie langCookie = Request.Cookies["culture"];
        //    if (langCookie != null)
        //    {
        //        lang = langCookie.Value;
        //    }
        //    else
        //    {
        //        var userLanguage = Request.UserLanguages;
        //        var userLang = userLanguage != null ? userLanguage[0] : "";
        //        if (userLang != "")
        //        {
        //            lang = userLang;
        //        }
        //        else
        //        {
        //            lang = "mr-IN"; LanguageManger.GetDefaultLanguage();
        //        }
        //    }
        //    new LanguageManger().SetLanguage(lang);
        //    // new LanguageManger().SetLanguage("mr-IN");
        //    return base.BeginExecuteCore(callback, state);
        //}
        [AllowAnonymous]
        public ActionResult ChangeLanguage(int lang)
        {
            if (lang == 0)
            {
                new LanguageManger().SetLanguage("mr-IN");
            }
            else
            {
                new LanguageManger().SetLanguage("en-US");
            }


            if (!string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
            {
                //   CommonUtility

                return RedirectToAction(Request.Form["ReturnUrl"].Split('/')[2]);
            }
            else
            {

                return RedirectToAction("Index", "Home");
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //string cultureOnCookie = "mr-IN";
            //HttpCookie langCookie = Request.Cookies["culture"];
            //if(langCookie != null)
            //{
            //    cultureOnCookie = langCookie.Value;
            //}

            string cultureOnCookie =  GetCultureOnCookie(filterContext.HttpContext.Request);
            //string cultureOnURL = filterContext.RouteData.Values.ContainsKey("lang")
            //    ? (filterContext.RouteData.Values["lang"] == null ? "en-US" : filterContext.RouteData.Values["lang"].ToString())
            //    : LanguageManger.DefaultCulture;
            //string culture = (cultureOnCookie == string.Empty)
            //    ? (filterContext.RouteData.Values["lang"] == null ? "en-US" : filterContext.RouteData.Values["lang"].ToString())
            //    : cultureOnCookie;

            SetCurrentCultureOnThread(cultureOnCookie);

            //if (culture != MultiLanguageViewEngine.CurrentCulture)
            //{
            //    (ViewEngines.Engines[0] as MultiLanguageViewEngine).SetCurrentCulture(culture);
            //}

            base.OnActionExecuting(filterContext);
        }
        public static String GetCultureOnCookie(HttpRequestBase request)
        {
            var cookie = request.Cookies["culture"];
            string culture = string.Empty;
            if (cookie != null)
            {
                culture = cookie.Value;
            }
            return culture;
        }
        private static void SetCurrentCultureOnThread(string lang)
        {
            if (string.IsNullOrEmpty(lang))
                lang = LanguageManger.DefaultCulture;
            var cultureInfo = new System.Globalization.CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
        }

    }


    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";

        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
    }

    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }

}