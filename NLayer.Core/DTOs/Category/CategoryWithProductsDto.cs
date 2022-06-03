using NLayer.Core.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs.Category
{
    public class CategoryWithProductsDto:CategoryDto
    {
        public List<ProductDto> Products { get; set; }
    }
}
