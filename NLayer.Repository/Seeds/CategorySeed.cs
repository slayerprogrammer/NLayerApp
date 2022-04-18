using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
