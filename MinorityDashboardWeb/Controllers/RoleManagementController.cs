using MinorityDashboard.Data.Repository;
using MinorityDashboard.DataModel;
using MinorityDashboard.Web.Controllers;
using MinorityDashboardWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboardWeb.Controllers
{
    public class RoleManagementController : BaseController
    {
        // GET: RoleManagement
        IDashboard objDashboard = new Dashboard();
        IRoleManager objRoleManager = new RoleManager();
        IMenuManager objMenuManager = new MenuManager();
        public ActionResult Index()
        {
            return View();
        }
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


            return View("AddRole", GetRoleList());
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

            return View(GetRoleList());

        }

        private RoleMaster GetRoleList()
        {
            RoleMaster obj = new RoleMaster();
            obj.lstRole = objRoleManager.GetRole();
            TempData["RoleTemp"] = obj;
            return obj;

        }


        public ActionResult AddUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddMenu()
        {

            return View(new CommonUtility().GetMainMenuList());
        }

        [HttpPost]
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



        [HttpGet]
        public ActionResult ManageRole()
        {
            SubMenu_Role_Mapping objSRM = new SubMenu_Role_Mapping();

            //  ViewBag.lstRoleMaster = BindRole();

            objSRM.lstRoleMaster = BindRole();
            //  objSRM.lstSubMenuMaster = objMenuManager.GetSubMenuList();


            return View(objSRM);
        }

        [HttpPost]
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

        public List<SelectListItem> BindRole()
        {
            List<role_master> lst = objRoleManager.GetRole();
            List<SelectListItem> lstddl = new List<SelectListItem>();
            lstddl.Add(new SelectListItem() { Value = "0", Text = "Select" });
            foreach (role_master X in lst)
            {
                lstddl.Add(new SelectListItem() { Value = X.role_id.ToString(), Text = X.role_name });
            }
            return lstddl;
        }

        private SubjectModel BindSubjectMaster()
        {
            SubjectModel obj = new SubjectModel();
            obj.lstSubject = objDashboard.GetSubjectData();
            obj.lstDesk = BindDesk();
            return obj;
        }

    }
}