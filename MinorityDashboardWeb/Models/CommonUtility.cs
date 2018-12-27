using MinorityDashboard.Data.Repository;
using MinorityDashboardWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MinorityDashboardWeb
{
    public class CommonUtility
    {
        public static string SucessMessage = "Record has been saved sucessfully.";

        public static string ErrorMessage = "Something went wrong.";

        public static string DeleteMessage = "Record has been deleted sucessfully.";

        public static string EditMessage = "Record has been edited sucessfully.";
        IMenuManager objMenuManager = new MenuManager();

        public MainMenuMaster GetMainMenuList()
        {
            MainMenuMaster objMMM = new MainMenuMaster();
            objMMM.lstMainMaster = objMenuManager.GetMainMenuList();
            return objMMM;
        }

        public SubMenuMaster GetSubMenuList()
        {
            SubMenuMaster objSMM = new SubMenuMaster();
            objSMM.lstSubMenuMaster = objMenuManager.GetSubMenuList();
            return objSMM;
        }
    }
}