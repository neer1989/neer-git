using MinorityDashboard.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace MinorityDashboardWeb.App_Start
{
    public class IOCConfig
    {

        public static void RegisterComponent()
        {
            var container = new UnityContainer();
            container.RegisterType<IDashboard, Dashboard>();
            container.RegisterType<IDistrictAdmin, DistrictAdmin>();
            container.RegisterType<ILoginRegister, LoginRegister>();
            container.RegisterType<IMenuManager, MenuManager>();
            container.RegisterType<IRoleManager, RoleManager>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

    }
}