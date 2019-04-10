using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboard.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorizeAttribute());
            filters.Add(new CustomExceptionFilter());
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        IMenuManager objMenuManager = new MenuManager();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.TempData["ErrorDetails"] = "You must be logged in to access this page";
                // filterContext.Result = new RedirectResult("~/Account/Login");
                // return;
            }

            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                string username = filterContext.HttpContext.User.Identity.Name;
                string pageurl = filterContext.HttpContext.Request.CurrentExecutionFilePath.Substring(1);

                List<Claim> lst = filterContext.HttpContext.GetOwinContext().Authentication.User.Claims.ToList();

                if (lst[1].Value != "1")
                {
                    string flg = ConfigurationManager.AppSettings["AuthorizationFlag"].ToString();
                    if (flg == "Y")
                    {
                        if (username != null && pageurl != null)
                        {
                            if (pageurl != "Base/AccessDenied" && pageurl != "")
                            {
                                string pageurlnew = "";
                                string[] arrypageurl = pageurl.Split('/');

                                if(arrypageurl.Length >2)
                                {
                                    pageurlnew = arrypageurl[0] + "/" + arrypageurl[1];
                                }
                                else
                                {
                                    pageurlnew = pageurl;
                                }

                                List<CheckAuthorizeMenu_Result> latmenulst = objMenuManager.CheckMenuAuthorization(username, pageurlnew);

                                if (latmenulst.Count == 0)
                                {
                                    filterContext.Result = new RedirectResult("~/Base/AccessDenied");
                                }
                            }
                        }
                    }
                }
            }

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Controller.TempData["ErrorDetails"] = "You do nat have necessary rights to access this page";
                filterContext.Result = new RedirectResult("~/Account/Login");
                //  return;
            }

        }
        public CustomAuthorizeAttribute(params object[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("roles");

            this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
        }
    }

    public class CustomExceptionFilter : FilterAttribute,
    IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                error_log log = new error_log();
                log.ExceptionMessage = filterContext.Exception.Message;
                log.ExceptionStackTrace = filterContext.Exception.StackTrace;
                log.ControllerName = filterContext.RouteData.Values["controller"].ToString();
                log.LogTime = System.DateTime.Now;
                ILoginRegister mgrSr = new LoginRegister();
                mgrSr.InsertError(log);
                filterContext.Result =  new RedirectResult("~/Base/AdminError");
                filterContext.ExceptionHandled = true;
            }
        }
    }

}
