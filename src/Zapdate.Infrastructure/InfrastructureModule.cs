using Autofac;
using Zapdate.Core.Interfaces.Gateways.Repositories;
using Zapdate.Core.Interfaces.Services;
using Zapdate.Infrastructure.Auth;
using Zapdate.Infrastructure.Identity.Repositories;
using Zapdate.Infrastructure.Interfaces;

namespace Zapdate.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance();
            builder.RegisterType<JwtHandler>().As<IJwtHandler>().SingleInstance();
            builder.RegisterType<TokenFactory>().As<ITokenFactory>().SingleInstance();
            builder.RegisterType<JwtValidator>().As<IJwtValidator>().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IRepository<>)).AsImplementedInterfaces();
        }
    }
}
