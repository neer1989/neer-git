using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorityDashboard.Data.Repository
{
   public interface ILoginRegister
    {
        List<LoginByUsernamePassword_Result> GetLoginDetails(string uname, string pwd);
        List<login> GetLogin();
        int InsertLogin(login obj);
        int UpdateLogin(login obj);
        int InsertError(error_log obj);
        int InsertLoginTrail(login_trail obj);

    }
}
