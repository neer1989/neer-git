using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
//using MinorityDashboard.DataModel;
using MinorityDashboard.Web.Models;
using MinorityDashboardWeb;
using Unity;

namespace MinorityDashboard.Web.Controllers
{
    [CustomExceptionFilter]
    public class AccountController : BaseController
    {
        // GET: Account
     //   IDashboard objDashboard; // = new Dashboard();

      //  ILoginRegister objLR;  //= new LoginRegister();

       // IUnityContainer unitycontainer = new UnityContainer();


        private readonly IDashboard objDashboard;
        private readonly ILoginRegister objLR;


        public AccountController(IDashboard repository1, ILoginRegister repository2)
        {
            objDashboard = repository1;
            objLR = repository2;
            //unitycontainer.RegisterType<IDashboard, Dashboard>();
            //unitycontainer.RegisterType<ILoginRegister, LoginRegister>();
            //objDashboard = unitycontainer.Resolve<Dashboard>();
            //objLR = unitycontainer.Resolve<LoginRegister>();
        }



        public ActionResult Index()
        {

            objDashboard.testmethod();
            return View();
        }


        #region Login methods

        /// <summary>
        /// GET: /Account/Login
        /// </summary>
        /// <param name="returnUrl">Return URL parameter</param>
        /// <returns>Return login view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {

                if (string.IsNullOrEmpty(Convert.ToString(Session["RandomNo"])))
                {
                    Random randomclass = new Random();
                    Session["RandomNo"] = Encrypt(randomclass.Next().ToString().Substring(0, 8));
                }
                if (string.IsNullOrEmpty(Convert.ToString(Session["AuthToken"])))
                {
                    string guid = Guid.NewGuid().ToString().Substring(0, 8);
                    Session["AuthToken"] = Encrypt(guid);
                }

                // Verification.
                if (this.Request.IsAuthenticated)
                {
                    // Info.
                    return this.RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Info.
            return this.View();
        }

        private void LoginTrail(string msgResult, string loginMode, string loginStatus,string UserId)
        {
            try
            {

                login_trail objLoginTrailModel = new login_trail();
                objLoginTrailModel.userid = UserId; //GetUidbyClaim();
                objLoginTrailModel.logintime = System.DateTime.Now;
                objLoginTrailModel.loginmode = loginMode;
                objLoginTrailModel.status = msgResult;
                objLoginTrailModel.loginstatusreason = loginStatus;
                objLoginTrailModel.ipaddress = GetIPAddress();               
                objLR.InsertLoginTrail(objLoginTrailModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetIPAddress()
        {
            string IPAdd = string.Empty;
            try
            {

                IPAdd = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(IPAdd))
                    IPAdd = Request.ServerVariables["REMOTE_ADDR"];

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return IPAdd;
        }


        /// <summary>
        /// POST: /Account/Login
        /// </summary>
        /// <param name="model">Model parameter</param>
        /// <param name="returnUrl">Return URL parameter</param>
        /// <returns>Return login view</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {

                string originalpwd = string.Empty;
                string strUserName = string.Empty;
                string strPassword = string.Empty;
                string strIpAddress = string.Empty;
                string StrLast = string.Empty;
                string strFirst = string.Empty;

                strPassword = model.Password;

                if (strPassword != "" || strPassword != string.Empty)
                {
                    StrLast = strPassword.Remove(strPassword.Length - 32).Trim();
                    strFirst = StrLast.Substring(32).Trim();
                    model.Password = strFirst.ToString().Trim();
                    originalpwd = Session["RandomNo"] + "" + model.Password + "" + Session["AuthToken"];
                }
                ViewBag.ErrMessage = "";
              

                if (model.CaptchaOrg.Replace(" ", "") != model.CaptchIn)
                {
                    ViewBag.ErrMessage = "Captcha is not valid";
                    return View();
                }

                // Verification.
                if (ModelState.IsValid)
                {
                    // Initialization.
                    var loginInfo = objLR.GetLoginDetails(model.Username, model.Password);  //this.databaseManager.LoginByUsernamePassword(model.Username, model.Password).ToList();

                    // Verification.
                    if (loginInfo != null && loginInfo.Count() > 0)
                    {
                        var logindetails = loginInfo.First();
                        this.SignInUser(logindetails.username, logindetails.role_id,logindetails.id, false);
                        this.Session["role_id"] = logindetails.role_id;
                        this.Session["user_id"] = logindetails.id;
                         Session["user_name"] = model.Username;

                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, logindetails.username, DateTime.Now, DateTime.Now.AddMinutes(2880), false, logindetails.role_id.ToString(), FormsAuthentication.FormsCookiePath);
                        string hash = FormsAuthentication.Encrypt(ticket);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

                        if (ticket.IsPersistent)
                        {
                            cookie.Expires = ticket.Expiration;
                        }
                        Response.Cookies.Add(cookie);
                        if (!string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
                        {
                            //   CommonUtility

                            return RedirectToAction(Request.Form["ReturnUrl"].Split('/')[2]);
                        }
                        else
                        {
                            //  Session["MenuList"] = new CommonUtility().GetMainMenuList();
                            // Session["SubMenuList"] = new CommonUtility().GetSubMenuList();
                            //return RedirectToAction("Profile");
                            LoginTrail("Login Sucess", "Login", "Login Successful", model.Username);
                            if (logindetails.role_id==3)
                            {
                                 return this.RedirectToAction("Index", "DistrictAdmin");
                            }

                            return this.RedirectToLocal(returnUrl);
                        }

                        // return this.RedirectToLocal(returnUrl);

                        //this.SignInUser("neer", 1, false);
                        //this.Session["role_id"] = 1;
                        //return this.RedirectToLocal(returnUrl);

                    }
                    else
                    {
                        // Setting.
                        LoginTrail("Id Password is wrong.", "Login", "Login Failed", model.Username);
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                LoginTrail("Error in Login.", "Login", "Login Failed", model.Username);
                Console.Write(ex);
            }

            // If we got this far, something failed, redisplay form
            return this.View(new LoginModel());
        }



        #endregion

        #region Log Out method.

        /// <summary>
        /// POST: /Account/LogOff
        /// </summary>
        /// <returns>Return log off action</returns>

        //  [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            try
            {
                // Setting.
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                // Sign Out.
                authenticationManager.SignOut();
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }

            // Info.
            return this.RedirectToAction("Index", "Website");
        }

        #endregion

        #region Helpers

        #region Sign In method.

        /// <summary>
        /// Sign In User method.
        /// </summary>
        /// <param name="username">Username parameter.</param>
        /// <param name="role_id">Role ID parameter</param>
        /// <param name="isPersistent">Is persistent parameter.</param>
        private void SignInUser(string username, int role_id,int userid, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();
            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim(ClaimTypes.Role, role_id.ToString()));
                claims.Add(new Claim(ClaimTypes.Sid, userid.ToString()));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                // Sign In.
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }

        #endregion

        #region Redirect to local method.

        /// <summary>
        /// Redirect to local method.
        /// </summary>
        /// <param name="returnUrl">Return URL parameter.</param>
        /// <returns>Return redirection action</returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                // Verification.
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }

            // Info.
            return this.RedirectToAction("Index", "Admin");
        }

        #endregion

        #endregion

    }
}