using Autofac;
using Zapdate.Server.Core.Interfaces.Gateways.Repositories;
using Zapdate.Server.Core.Interfaces.Services;
using Zapdate.Server.Infrastructure.Auth;
using Zapdate.Server.Infrastructure.Cryptography;
using Zapdate.Server.Infrastructure.Data.Repositories;
using Zapdate.Server.Infrastructure.Files;
using Zapdate.Server.Infrastructure.Identity.Repositories;
using Zapdate.Server.Infrastructure.Interfaces;

namespace Zapdate.Server.Infrastructure
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

            builder.RegisterType<AsymmetricKeyFactory>().As<IAsymmetricKeyParametersFactory>().SingleInstance();
            builder.RegisterType<SymmetricEncryption>().As<ISymmetricEncryption>().SingleInstance();
            builder.RegisterType<AsymmetricCryptoHandler>().As<IAsymmetricCryptoHandler>().SingleInstance();

            builder.RegisterType<ServerFilesManager>().As<IServerFilesManager>().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IRepository<>)).AsImplementedInterfaces();
            builder.RegisterType<StoredFileRepository>().As<IStoredFileRepository>();
        }
    }
}
