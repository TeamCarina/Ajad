using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using SampleApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SampleApp.DI
{
    public class AutofacDIContainer
    {
        public static void ConfigureDIContainer()
        {

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);
            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterModule<LogicDIRegisterModule>();
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            // Set the dependency resolver for MVC.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}