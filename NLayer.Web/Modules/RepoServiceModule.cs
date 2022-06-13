using Autofac;
using NLayer.Caching;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;

//bu şekilde tanımlarsak aşağıdaki implemente de çıkan hata gidecektir. !!
using Module = Autofac.Module;

namespace NLayer.Web.Modules
{
    //Module hem Reflection hem Autofac de olduğu için hata veriyor o zaman yukarıda ki using tanımını yapalım !!
    public class RepoServiceModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Tekil oldukları için direkt tanımlıyoruz.
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            //ForExample: IProductRepository, ProductRepository> buradan yakalayacağız ;
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x=>x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();

            //InstancePerLifetimeScope => .Net Core da Scope karşılık gelir.
            //InstancePerDependency => .Net Core da Transient  karşılık gelir. Herhangi bir class ctr ında o interface nerde geçtiyse her seferinde yeni instance.
        }
    }
}
