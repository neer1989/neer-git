using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinorityDashboardWeb.Models
{
    public class RoleManagementModel
    {
        public List<RoleMaster> lstRoleMaster { get; set; }

    }
    public class RoleMaster
    {
        public int role_id { get; set; }
        public string rolename { get; set; }
        public bool is_active { get; set; }
        public List<role_master > lstRole { get; set; }

    }


    public class MainMenuMaster
    {
        public int menu_id { get; set; }
        public string menu_name { get; set; }
        public string page_url { get; set; }
        public int menu_no { get; set; }
        public bool is_active { get; set; }
        public int createdby { get; set; }
        public DateTime createddate { get; set; }
        public int updatedby { get; set; }
        public DateTime updateddate { get; set; }

        public List<main_menu_master> lstMainMaster { get; set; }


    }

    public class SubMenuMaster
    {
        public int sub_menu_id { get; set; }
        public int menu_id { get; set; }
        public string sub_menu_name { get; set; }
        public string page_url { get; set; }
        public int sub_menu_no { get; set; }
        public bool is_active { get; set; }
        public int createdby { get; set; }
        public DateTime createddate { get; set; }
        public int updatedby { get; set; }
        public DateTime updateddate { get; set; }
        public List<sub_menu_master> lstSubMenuMaster { get; set; }

    }

   public class SubMenu_Role_Mapping
    {
        public int map_id { get; set; }
        public int role_id { get; set; }
        public int menu_id { get; set; }
        public int submenu_id { get; set; }
        public bool isactive { get; set; }
        public int createdby { get; set; }
        public DateTime createddate { get; set; }
        public int updatedby { get; set; }
        public DateTime updateddate { get; set; }

        public List<CheckAuthorizeMenu_Result> lstAuthorizeMenu { get; set; }
        public List<SelectListItem>  lstRoleMaster { get; set; }
        public List<sub_menu_master> lstSubMenuMaster { get; set; }
        public List<menu_role_mapping> lstMenuRoleMapping { get; set; }
    }

}