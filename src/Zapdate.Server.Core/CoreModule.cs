using Autofac;
using Zapdate.Server.Core.Domain.Actions;
using Zapdate.Server.Core.Interfaces;

namespace Zapdate.Server.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IUseCaseRequestHandler<,>)).AsImplementedInterfaces();
            builder.RegisterType<AddUpdatePackageFilesAction>().As<IAddUpdatePackageFilesAction>();
        }
    }
}
