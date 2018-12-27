using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorityDashboard.Data.Repository
{
    public interface IMenuManager
    {
        int InsertMainMenu(main_menu_master obj);
        List<main_menu_master> GetMainMenuList();
        List<sub_menu_master> GetSubMenuList();
        List<CheckAuthorizeMenu_Result> CheckMenuAuthorization(string username, string pageurl);
        List<menu_role_mapping> GetMenuRoleMapings(int role_id);
        int UpdateSubMenu(List<sub_menu_master> lstsmm, int roleid);

    }
}
