using Autofac;
using SampleApp.Comm.Contracts;
using SampleApp.Comm.Implementations;
using SampleApp.Comm.Models;
using SampleApp.Implementations.Logic;
using SampleApp.Logic.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Logic
{
    public class LogicDIRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterModule<CommunicationDIRegisterModule>();
            builder.RegisterType<AccountLogic>().As<IAccountLogic>().InstancePerLifetimeScope();

            builder.RegisterType<AdminLogic>().As<IAdminLogic>().InstancePerLifetimeScope();

            //DbContext
            builder.RegisterType<SampleAppModels>().As<ISampleAppModels>().InstancePerLifetimeScope();

            //Generic SqlReporsitories and unitof work register.
            builder.RegisterGeneric(typeof(SampleAppRepo<>)).As(typeof(ISampleAppRepo<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            //builder.RegisterType<DataBase>().As<IDataBase>();
            //builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>().SingleInstance();

        }
    }
}
