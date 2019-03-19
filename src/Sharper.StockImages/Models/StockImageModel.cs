namespace Sharper.StockImages.Models
{
    public class StockImageModel
    {
        public string Id { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageThumbUrl { get; set; }

        public string ImageEmbedUrl { get; set; }

        public string ImageServiceUrl { get; set; }

        public string CreatorUserName { get; set; }

        public string CreatorName { get; set; }

        public string CreatorServiceUrl { get; set; }

        public string ServiceId { get; set; }
    }
}