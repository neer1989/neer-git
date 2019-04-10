using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using MinorityDashboard.Web;
using MinorityDashboard.Web.Controllers;
using MinorityDashboard.Web.Models;
using MinorityDashboardWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboardWeb.Controllers
{
    [CustomExceptionFilter]
    public class RoleManagementController : BaseController
    {
        // GET: RoleManagement
     
        private readonly IDashboard objDashboard;
        private readonly IRoleManager objRoleManager;
        private readonly IMenuManager objMenuManager;
        private readonly ILoginRegister objLoginRegister;
        

        public RoleManagementController(IDashboard repository1, IRoleManager repository2, IMenuManager repository3 , ILoginRegister repository4)
        {
            objDashboard = repository1;
            objRoleManager = repository2;
            objMenuManager = repository3;
            objLoginRegister = repository4;
        }

        public ActionResult Index()
        {
            return View();
        }
      

        private RoleMaster GetTempRole(int id)
        {
            RoleMaster obj = new RoleMaster();
            if (TempData.ContainsKey("RoleTemp"))
            {
                obj = (RoleMaster)TempData.Peek("RoleTemp");
                List<role_master> newrole = obj.lstRole.Where(s => s.role_id == id).ToList();
                obj.rolename = newrole[0].role_name;
                obj.role_id = newrole[0].role_id;
                obj.is_active = Convert.ToBoolean(newrole[0].isactive);
            }
            return obj;
        }
        
        [HttpGet]
        public ActionResult AddUser()
        {
            LoginModel LM = new LoginModel();
            LM.lstLoginUser = objLoginRegister.GetLogin();
            LM.ddlRoleMaster = BindRole();
            TempData["LoginUserList"] = LM;

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

            return View(LM);

        }
        [HttpPost]
        public ActionResult AddUser(LoginModel LM)
        {

            string originalpwd = string.Empty;
            string strUserName = string.Empty;
            string strPassword = string.Empty;
            string strIpAddress = string.Empty;
            string StrLast = string.Empty;
            string strFirst = string.Empty;

            strPassword = LM.Password;

            if (strPassword != "" || strPassword != string.Empty)
            {
                StrLast = strPassword.Remove(strPassword.Length - 32).Trim();
                strFirst = StrLast.Substring(32).Trim();
                LM.Password = strFirst.ToString().Trim();
                originalpwd = Session["RandomNo"] + "" + LM.Password + "" + Session["AuthToken"];
            }



            int result = 0;
            login obj = new login();
            obj.email_id = LM.EmailID;
            obj.employee_name = LM.EmployeeName;
            obj.id = LM.Id;
            obj.password = LM.Password;
            obj.role_id = LM.Role_Id;
            obj.username = LM.UserName;

            if (obj.id > 0)
            {
                   
                result = objLoginRegister.UpdateLogin(obj);
            }
            else
            {
                obj.created_by = GetUidbyClaim();
                obj.created_date = DateTime.Now;
                result = objLoginRegister.InsertLogin(obj);
            }
            if (result > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            return RedirectToAction("AddUser");
        }

        public ActionResult EditUser(int id)
        {
            LoginModel obj = new LoginModel();
            if (TempData["LoginUserList"] != null)
            {
                obj = (LoginModel)TempData["LoginUserList"];
            }
            List<login> lst = obj.lstLoginUser.Where(s => s.id == id).ToList();  //objDashboard.GetLatestNewsById(id);
            obj.isactive = Convert.ToBoolean(lst[0].isactive);
            obj.EmailID = lst[0].email_id;
            obj.EmployeeName = lst[0].employee_name;
            obj.isactive = Convert.ToBoolean(lst[0].isactive);
            obj.Role_Id = lst[0].role_id;
            obj.UserName = lst[0].username;
            obj.Id = lst[0].id;
            TempData["LoginUserList"] = obj;
            return View("AddUser", obj);          
        }


            private SubjectModel BindSubjectMaster()
        {
            SubjectModel obj = new SubjectModel();
            obj.lstSubject = objDashboard.GetSubjectData();
            obj.lstDesk = BindDesk();
            return obj;
        }


        #region Manage Role

        [HttpGet]
        [NonAction]
        public ActionResult ManageRole()
        {
            SubMenu_Role_Mapping objSRM = new SubMenu_Role_Mapping();

            //  ViewBag.lstRoleMaster = BindRole();

            objSRM.lstRoleMaster = BindRole();
            //  objSRM.lstSubMenuMaster = objMenuManager.GetSubMenuList();


            return View(objSRM);
        }

        [HttpPost]
        [NonAction]
        public ActionResult ManageRole(SubMenu_Role_Mapping obj)
        {

            int flag = objMenuManager.UpdateSubMenu(obj.lstSubMenuMaster, obj.role_id);

            if (flag > 0)
            {
                Success(CommonUtility.SucessMessage);
            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            SubMenu_Role_Mapping objSRM = new SubMenu_Role_Mapping();
            objSRM.lstRoleMaster = BindRole();
            objSRM.lstSubMenuMaster = objMenuManager.GetSubMenuList();
            return View(objSRM);
        }


        [HttpPost]
        [NonAction]
        public ActionResult RoleChange(int id)
        {
            SubMenu_Role_Mapping objSRM = new SubMenu_Role_Mapping();
            objSRM.lstRoleMaster = BindRole();
            System.Threading.Thread.Sleep(1000);
            if (id > 0)
            {
                List<sub_menu_master> lstsmm = objMenuManager.GetSubMenuList();
                List<menu_role_mapping> lstmrm = objMenuManager.GetMenuRoleMapings(id);

                foreach (var X in lstsmm)
                {
                    var lstfinal = lstmrm.Where(s => s.submenu_id == X.sub_menu_id).ToList();

                    if (lstfinal.Count > 0)
                    {
                        X.isactive = Convert.ToBoolean(lstfinal[0].isactive);
                    }
                    else
                    {
                        X.isactive = false;
                    }
                }
                objSRM.lstSubMenuMaster = lstsmm;
            }

            return PartialView("ManageRolePartial", objSRM);
        }

        #endregion


        #region Menu Code
        [HttpGet]
        [NonAction]
        public ActionResult AddMenu()
        {

            return View(new CommonUtility().GetMainMenuList());
        }

        [HttpPost]
        [NonAction]
        public ActionResult AddMenu(MainMenuMaster obj)
        {
            int sucessflg = 0;
            main_menu_master mObj = new main_menu_master();

            mObj.createdby = Convert.ToInt32(Session["user_id"]);
            mObj.createddate = DateTime.Now;
            mObj.isactive = obj.is_active;
            mObj.menu_id = 0;
            mObj.menu_name = obj.menu_name;
            mObj.menu_no = obj.menu_no;
            mObj.page_url = obj.page_url;
            mObj.updatedby = Convert.ToInt32(Session["user_id"]);
            mObj.updateddate = DateTime.Now;

            sucessflg = objMenuManager.InsertMainMenu(mObj);


            if (sucessflg > 0)
            {
                Success(CommonUtility.SucessMessage);

            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }


            return View();
        }

        #endregion


        #region Role Page Code

        [HttpGet]
        public ActionResult AddRole()
        {

            return View(GetRoleList());

        }

        public ActionResult EditRole(int id)
        {
            // Edit Delete flag for preserve the Temp Data
            return View("AddRole", GetTempRole(id));
        }

        private RoleMaster GetRoleList()
        {
            RoleMaster obj = new RoleMaster();
            obj.lstRole = objRoleManager.GetRole();
            TempData["RoleTemp"] = obj;
            return obj;

        }
        public ActionResult DeleteRole(int id)
        {

            int sucessflg = 0;
            RoleMaster obj = new RoleMaster();
            if (TempData.ContainsKey("RoleTemp"))
            {
                obj = (RoleMaster)TempData.Peek("RoleTemp");
                List<role_master> newrole = obj.lstRole.Where(s => s.role_id == id).ToList();

                newrole[0].isactive = false;

                sucessflg = objRoleManager.Update_DeleteRole(newrole[0]);
            }
            if (sucessflg > 0)
            {
                Success(CommonUtility.SucessMessage);

            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }


            return RedirectToAction("AddRole");

            //return View("AddRole", GetRoleList());
        }

        [HttpPost]
        public ActionResult AddRole(RoleMaster obj)
        {

            int sucessflg = 0;
            role_master objrm = new role_master();
            objrm.role_name = obj.rolename;
            objrm.isactive = obj.is_active;

            if (obj.role_id > 0)
            {
                objrm.role_id = obj.role_id;
                sucessflg = objRoleManager.Update_DeleteRole(objrm);
            }
            else
            {
                sucessflg = objRoleManager.InsertRole(objrm);
            }
            if (sucessflg > 0)
            {
                Success(CommonUtility.SucessMessage);

            }
            else
            {
                Danger(CommonUtility.ErrorMessage);
            }
            return RedirectToAction("AddRole");
            // return View(GetRoleList());

        }


        #endregion


    }
}