namespace NLayer.Core.DTOs.ProductFeature
{
    public class ProductFeatureDto
    {
        public long Id { get; set; }
        public string Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public long ProductId { get; set; }
    }
}
