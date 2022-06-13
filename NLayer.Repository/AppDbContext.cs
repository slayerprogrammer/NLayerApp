using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using System.Reflection;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReferance)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReferance.CreatedDate = DateTime.UtcNow;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReferance).Property(x=>x.CreatedDate).IsModified = false;
                                entityReferance.UpdatedDate = DateTime.UtcNow;
                                break;
                            }
                    }
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            //ChangeTracker ismindeki property Bu Prop ile Entries lerimizi alabiliriz.

            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReferance)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReferance.CreatedDate = DateTime.UtcNow;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReferance).Property(x => x.CreatedDate).IsModified = false;
                                entityReferance.UpdatedDate = DateTime.UtcNow;
                                break;
                            }
                    }
                }
            }


            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent API olarak tanımlanır.
            //Best Practies bakımından tek bir yerden tanımlanması gerekir. 
            //İlgili Modellerde tanımlamamak gerekir.

            //Fakat bu sefer de DbContext için kalabalık olmuş olacak bunun için
            //ilgili model için classlar oluşturup oralarda tanımlamak gerekir.
            //modelBuilder.Entity<Category>().HasKey(c => c.Id);

            //tüm configurationları buluyor yani IEntityTypeConfigurationları reflection yaparak bu interface sahip tüm classları buluyor.

            //burada çalışmış olduğumuz Assembly tara diyoruz.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //yukarıdaki örnek yerine aşağıda ki gibi de yazabiliriz Fakat birden fazla olacağı için kod kalabalığı oluşturur.
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());


            modelBuilder.Entity<ProductFeature>().HasData(
                new ProductFeature()
                {
                    Id = 1,
                    Color = "Kırmızı",
                    Height = 100,
                    Width = 200,
                    ProductId = 1
                },

                 new ProductFeature()
                 {
                     Id = 2,
                     Color = "Mavi",
                     Height = 200,
                     Width = 300,
                     ProductId = 2
                 });


            base.OnModelCreating(modelBuilder);
        }

    }
}
