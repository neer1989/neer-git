using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorityDashboard.Data.Repository
{
    public class MenuManager : IMenuManager
    {

        CommonRepository ObjCR = new CommonRepository();
        public int InsertMainMenu(main_menu_master obj)
        {
            main_menu_master returnobj = ObjCR.SaveData<main_menu_master>(obj);
            return returnobj.menu_id;
        }

        public List<main_menu_master> GetMainMenuList()
        {
            return ObjCR.GetData<main_menu_master>();
        }

        public List<sub_menu_master> GetSubMenuList()
        {
            return ObjCR.GetData<sub_menu_master>();
        }

        public int UpdateSubMenu(List<sub_menu_master> lstsmm, int roleid)
        {
            int insertedRecords = 0;
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                var lst = entities.menu_role_mapping.Where(s => s.role_id == roleid).ToList();
                if (lst.Count > 0)
                {
                    foreach (var X in lstsmm)
                    {
                        if (lst.Where(s => s.submenu_id == X.sub_menu_id).ToList().Count > 0)
                        {
                            lst.Where(s => s.submenu_id == X.sub_menu_id).ToList()[0].isactive = X.isactive;
                        }
                        else
                        {
                            lst.Add(entities.menu_role_mapping.Add(MenuRoleMapping(X, roleid)));
                        }
                    }
                    insertedRecords = entities.SaveChanges();
                }
                else
                {
                    foreach (var X in lstsmm)
                    {
                       
                        entities.menu_role_mapping.Add(MenuRoleMapping(X,roleid));
                    }
                     insertedRecords = entities.SaveChanges();
                }
            }
            return insertedRecords;

        }

        private menu_role_mapping MenuRoleMapping(sub_menu_master X, int roleid)
        {
            menu_role_mapping mrm = new menu_role_mapping();
            mrm.isactive = X.isactive;
            mrm.menu_id = X.menu_id;
            mrm.submenu_id = X.sub_menu_id;
            mrm.role_id = roleid;
            mrm.createdby = null;
            mrm.createddate = null;
            mrm.updatedby = null;
            mrm.updateddate = null;
            return mrm;

        }


        public List<CheckAuthorizeMenu_Result> CheckMenuAuthorization(string username, string pageurl)
        {
            List<CheckAuthorizeMenu_Result> lst = new List<CheckAuthorizeMenu_Result>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                lst = entities.CheckAuthorizeMenu(username, pageurl).ToList();
            }
            return lst;
        }

        public List<menu_role_mapping> GetMenuRoleMapings(int role_id)
        {
            return ObjCR.GetData<menu_role_mapping>().Where(s => s.role_id == role_id).ToList();

            //  return ObjCR.GetData<trans_docfile>();
        }



    }
}
