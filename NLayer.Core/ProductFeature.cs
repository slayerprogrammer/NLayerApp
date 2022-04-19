namespace NLayer.Core
{
    public class ProductFeature
    {
        public long Id { get; set; }
        public string Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public long ProductId { get; set; }
    
        public Product Product { get; set; }
    }
}
