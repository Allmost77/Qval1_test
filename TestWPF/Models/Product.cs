using System.IO;

namespace TestWPF.Models
{
    public class Product
    {
        static readonly string ImgPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Images");
        static string DefaultImg => Path.Combine(ImgPath, "Icon.JPG");

        public int Id { get; set; }
        public string Article { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string Supplier { get; set; } = "";
        public string Unit { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string ImagePath { get; set; } = "";

        public string ImageOfPlaceholder => string.IsNullOrWhiteSpace(ImagePath) ? DefaultImg : (File.Exists(Path.Combine(ImgPath, ImagePath)) ? Path.Combine(ImgPath, ImagePath) : DefaultImg);
        public decimal FinalPrice => Price * (100m - Discount) / 100m;
        public bool HasDiscount => Discount > 0;
        public bool OutOfStock => Quantity <= 0;
        public bool BigDiscount => Discount > 15;
    }
}
