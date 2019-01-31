using MinorityDashboard.Data;
using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using MinorityDashboardWeb;
using MinorityDashboardWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        IDistrictAdmin objDistrictAdmin = new DistrictAdmin();

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

        [HttpPost]
        public ActionResult ParentChange(int PSchemeid)
        {
            System.Threading.Thread.Sleep(1000);
            return Json(BindChildScheme1(1, PSchemeid));
        }

        [HttpPost]
        public ActionResult ChildChange1(int ChildSchemeid1)
        {
            System.Threading.Thread.Sleep(1000);
            return Json(BindChildScheme2(1, ChildSchemeid1));
        }

        [HttpPost]
        public ActionResult ChildChange2(int ChildSchemeid2)
        {
            System.Threading.Thread.Sleep(1000);
            return Json(BindChildScheme3(1, ChildSchemeid2));
        }


        public List<string> SaveFileinFolder(HttpPostedFileBase[] inputstr, string folderName, int id = 0)
        {
            string InputFileName = "";
            List<string> lstDoc = new List<string>();
            if (inputstr != null)
            {
                foreach (var HPF in inputstr)
                {
                    string str1 = DateTime.Today.ToString("yyyyMMdd");
                    string str2 = DateTime.Now.ToString("HH:mm:ss").Replace(":", "");
                    string str = str1 + str2;
                    if(id>0)
                    {
                        InputFileName = id + "_" + str + "_" + Path.GetFileName(HPF.FileName);
                    }
                    else
                    {
                        InputFileName = str + "_" + Path.GetFileName(HPF.FileName);
                    }               
                    var ServerSavePath = Path.Combine(Server.MapPath(folderName) + InputFileName);
                    HPF.SaveAs(ServerSavePath);
                    lstDoc.Add(InputFileName);
                }
            }
            return lstDoc;
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
        public List<DistrictList> DistrictwithAmt()
        {
            List<district_master> lst = objDashboard.GetDistrict();
            List<DistrictList> lstdistrict = new List<DistrictList>();

            foreach (district_master X in lst)
            {
                lstdistrict.Add(new DistrictList() { district_id = X.des_id, districtname = X.des_name,allotedmt=0 });
            }
            return lstdistrict;
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

        public List<SelectListItem> BindImplementationAgency()
        {
            List<implementation_agency_master> lst = objDistrictAdmin.GetImplementationAgency();
            List<SelectListItem> lstddl = new List<SelectListItem>();
            lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });
            foreach (implementation_agency_master X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.imp_agency_id.ToString(), Text = X.agency_name });
            }
            return lstddl;
        }

        public List<SelectListItem> BindParentScheme(int lang)
        {
            List<parentscheme> lst = objDashboard.GetParentScheme();
            List<SelectListItem> lstddl = new List<SelectListItem>();
            lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });
            if (lang == 0)
            {

                foreach (parentscheme X in lst)
                {
                    lstddl.Add(new SelectListItem() { Value = X.parent_scheme_id.ToString(), Text = X.parent_schemename_e });
                }
            }
            else
            {
                foreach (parentscheme X in lst)
                {
                    lstddl.Add(new SelectListItem() { Value = X.parent_scheme_id.ToString(), Text = X.parent_schemename_m });
                }
            }
            return lstddl;
        }
        public List<SelectListItem> BindChildScheme1(int lang, int PSchemId = 0)
        {
            List<scheme_child1> lst = PSchemId == 0 ? objDashboard.GetSchemeChild1() : objDashboard.GetSchemeChild1().Where(s => s.parent_scheme_id == PSchemId).ToList();
            List<SelectListItem> lstddl = new List<SelectListItem>();
            //lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });


            foreach (scheme_child1 X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.scheme_id_child1.ToString(), Text = lang == 0 ? X.child1_schemename_e : X.child1_schemename_m });
            }

            return lstddl;
        }
        public List<SelectListItem> BindChildScheme2(int lang, int ChildSchemId1 = 0)
        {

            List<scheme_child2> lst = ChildSchemId1 == 0 ? objDashboard.GetSchemeChild2() : objDashboard.GetSchemeChild2().Where(s => s.scheme_id_child1 == ChildSchemId1).ToList();
            List<SelectListItem> lstddl = new List<SelectListItem>();
           // lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });
            foreach (scheme_child2 X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.scheme_id_child2.ToString(), Text = lang == 0 ? X.child2_schemename_e : X.child2_schemename_m });
            }

            return lstddl;
        }
        public List<SelectListItem> BindChildScheme3(int lang, int ChildSchemId2 = 0)
        {
            List<scheme_child3> lst = ChildSchemId2 == 0 ? objDashboard.GetSchemeChild3() : objDashboard.GetSchemeChild3().Where(s => s.scheme_id_child2 == ChildSchemId2).ToList();
            List<SelectListItem> lstddl = new List<SelectListItem>();
           // lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });

            foreach (scheme_child3 X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.scheme_id_child3.ToString(), Text = lang == 0 ? X.child3_schemename_e : X.child3_schemename_m });
            }
            return lstddl;
        }

        public List<SelectListItem> BlankSelectItem()
        {
            SelectListItem sli = new SelectListItem();
            sli.Text = "Select";
            sli.Value = "0";
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(sli);
            return lst;
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


        public string PickColor(int id)
        {
            List<string> lstcolr = new List<string>();
            lstcolr.Add("#99CCFF");
            lstcolr.Add("#0000FF");
            lstcolr.Add("#FFFF00");
            lstcolr.Add("#FF00FF");
            lstcolr.Add("#00FFFF");
            lstcolr.Add("#800000");
            lstcolr.Add("#008000");
            lstcolr.Add("#000080");
            lstcolr.Add("#808000");
            lstcolr.Add("#800080");
            lstcolr.Add("#008080");
            lstcolr.Add("#C0C0C0");
            lstcolr.Add("#808080");
            lstcolr.Add("#9999FF");
            lstcolr.Add("#993366");
            lstcolr.Add("#FFFFCC");
            lstcolr.Add("#CCFFFF");
            lstcolr.Add("#660066");
            lstcolr.Add("#FF8080");
            lstcolr.Add("#0066CC");
            lstcolr.Add("#CCCCFF");
            lstcolr.Add("#000080");
            lstcolr.Add("#FF00FF");
            lstcolr.Add("#FFFF00");
            lstcolr.Add("#00FFFF");
            lstcolr.Add("#800080");
            lstcolr.Add("#800000");
            lstcolr.Add("#008080");
            lstcolr.Add("#0000FF");
            lstcolr.Add("#00CCFF");
            lstcolr.Add("#CCFFFF");
            lstcolr.Add("#CCFFCC");
            lstcolr.Add("#FFFF99");
            lstcolr.Add("#99CCFF");
            lstcolr.Add("#FF99CC");
            lstcolr.Add("#CC99FF");
            lstcolr.Add("#FFCC99");
            lstcolr.Add("#3366FF");
            lstcolr.Add("#33CCCC");
            lstcolr.Add("#99CC00");
            lstcolr.Add("#FFCC00");
            lstcolr.Add("#FF9900");

            return lstcolr[id].ToString();


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

            string cultureOnCookie = GetCultureOnCookie(filterContext.HttpContext.Request);
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