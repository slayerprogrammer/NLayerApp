using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;

namespace NLayer.Repository.Seeds
{
    internal class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //eski kullanım
            //Category c = new Category();
            //Yeni kullanım
            //Category a = new();

            builder.HasData(
                new Category { Id = 1, Name = "Kalemler", CreatedDate = DateTime.UtcNow },
                new Category { Id = 2, Name = "Defterler", CreatedDate = DateTime.UtcNow },
                new Category { Id = 3, Name = "Silgiler", CreatedDate = DateTime.UtcNow });
        }
    }
}
