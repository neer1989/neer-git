using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorityDashboard.Data.Repository
{
    public class RoleManager : IRoleManager
    {
        CommonRepository ObjCR = new CommonRepository();
        public int InsertRole(role_master obj)
        {
            role_master returnobj = ObjCR.SaveData<role_master>(obj);
            return returnobj.role_id;
        }

        public int Update_DeleteRole(role_master obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var roleobj = db.role_master.FirstOrDefault(x => x.role_id == obj.role_id);
                    roleobj.role_name = obj.role_name;
                    roleobj.isactive = obj.isactive;
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                successflg = 0;
            }

            return successflg;
            //role_master returnobj = ObjCR.UpdateData<role_master>(obj, s => s.role_id == obj.role_id);
            //return returnobj.role_id;
        }


        public List<role_master> GetRole()
        {
            return ObjCR.GetData<role_master>(s => s.isactive == true);
        }

        public int DeleteRole(role_master obj)
        {
            return ObjCR.DeleteData<role_master>(obj);
        }


    }
}
