namespace Sharper.StockImages.Models
{
    internal class UnsplashUserModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public UnsplashUserLinkModel Links { get; set; }
    }

    internal class UnsplashUserLinkModel
    {
        public string Self { get; set; }

        public string Html { get; set; }
    }
}