using Autofac;
using Triepe.Domain.Dtos;

namespace Triepe.Api.AutofacModules
{
    public class FactoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(BaseDto).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Factory"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }

}
