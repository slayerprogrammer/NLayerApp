using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;

namespace NLayer.Repository.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //PK için kullanılır
            builder.HasKey(x => x.Id);
            //Id kaçar kaçar artacağını belirler
            builder.Property(x => x.Id).UseIdentityColumn();
            //zorunlu alan ve max karakter i belirler.
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            //tablo adını belirtir.
            builder.ToTable("Categories");

        }
    }
}
