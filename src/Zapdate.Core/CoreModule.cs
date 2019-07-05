using Autofac;
using Zapdate.Core.Domain.Actions;
using Zapdate.Core.Interfaces;

namespace Zapdate.Core
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
