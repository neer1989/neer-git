using MinorityDashboard.Data.Repository;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace MinorityDashboardWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IDashboard, Dashboard>();
            container.RegisterType<IDistrictAdmin, DistrictAdmin>();
            container.RegisterType<IRoleManager, RoleManager>();
            container.RegisterType<IMenuManager, MenuManager>();
            container.RegisterType<ILoginRegister, LoginRegister>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}