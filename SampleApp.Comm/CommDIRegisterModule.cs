using Autofac;
using SampleApp.Comm.Contracts;
using SampleApp.Comm.Implementations;
using SampleApp.Comm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Comm
{
    public class CommDIRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //DbContext
            builder.RegisterType<SampleAppModels>().As<ISampleAppModels>().InstancePerLifetimeScope();
            //builder.Register(c => new TransScanContext("name=TransScanContext")).As<ITransScanContext>().InstancePerRequest();
            //Generic SqlReporsitories and unitof work register.
            builder.RegisterGeneric(typeof(SampleAppRepo<>)).As(typeof(ISampleAppRepo<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
        public static void disposeMethod()
        {

        }
    }
}
