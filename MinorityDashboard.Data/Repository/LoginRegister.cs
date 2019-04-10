using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorityDashboard.Data.Repository
{
  public class LoginRegister : ILoginRegister
    {
        CommonRepository ObjCR = new CommonRepository();
        public List<LoginByUsernamePassword_Result> GetLoginDetails(string uname, string pwd)
        {
            List<LoginByUsernamePassword_Result> lst = new List<LoginByUsernamePassword_Result>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                lst = entities.LoginByUsernamePassword(uname, pwd).ToList();
            }
            return lst;
        }

        public List<login> GetLogin()
        {
            return ObjCR.GetData<login>();
        }

        public int InsertLogin(login obj)
        {
            login returnobj = ObjCR.SaveData<login>(obj);
            return returnobj.id;
        }

        public int UpdateLogin(login obj)
        {
            int successflg = 1;
            try
            {
                using (var db = new MinorityDasboard_DBEntities())
                {
                    var upobj = db.logins.FirstOrDefault(x => x.id == obj.id);
                    upobj.email_id = obj.email_id;
                    upobj.employee_name = obj.employee_name;
                    upobj.password = obj.password;
                    upobj.role_id = obj.role_id;
                    upobj.username = obj.username;
                    upobj.updated_by = obj.updated_by;
                    upobj.updated_date = obj.updated_date;
                    upobj.isactive = obj.isactive;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                successflg = 0;
            }
            return successflg;
        }

        public int InsertError(error_log obj)
        {
            error_log returnobj = ObjCR.SaveData<error_log>(obj);
            return returnobj.Id;
        }
        public int InsertLoginTrail(login_trail obj)
        {
            login_trail returnobj = ObjCR.SaveData<login_trail>(obj);
            return returnobj.logintrailid;
        }



    }
}
