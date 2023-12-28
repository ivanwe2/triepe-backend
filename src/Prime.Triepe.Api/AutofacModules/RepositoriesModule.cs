using Autofac;
using Triepe.Data.Entities.Base;

namespace Triepe.Api.AutofacModules
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(BaseEntity).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }

}
