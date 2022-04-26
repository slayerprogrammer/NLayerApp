namespace NLayer.Core.DTOs.Product
{
    public class ProductUpdateDto
    {
        public long Id{ get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
