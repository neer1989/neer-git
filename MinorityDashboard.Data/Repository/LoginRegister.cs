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

        public List<LoginByUsernamePassword_Result> GetLoginDetails(string uname, string pwd)
        {
            List<LoginByUsernamePassword_Result> lst = new List<LoginByUsernamePassword_Result>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                lst = entities.LoginByUsernamePassword(uname, pwd).ToList();
            }
            return lst;
        }

    }
}
