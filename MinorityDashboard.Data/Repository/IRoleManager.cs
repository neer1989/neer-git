using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorityDashboard.Data.Repository
{
   public interface IRoleManager
    {
        int InsertRole(role_master obj);
        List<role_master> GetRole();
        int Update_DeleteRole(role_master obj);

        int DeleteRole(role_master obj);
    }
}
