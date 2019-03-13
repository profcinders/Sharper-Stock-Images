namespace Sharper.StockImages.Models
{
    internal class UnsplashImageModel
    {
        public string Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Description { get; set; }

        public UnsplashImageUrlModel Urls { get; set; }

        public UnsplashImageLinkModel Links { get; set; }

        public UnsplashUserModel User { get; set; }
    }

    internal class UnsplashImageUrlModel
    {
        public string Raw { get; set; }

        public string Full { get; set; }

        public string Regular { get; set; }

        public string Small { get; set; }

        public string Thumb { get; set; }
    }

    internal class UnsplashImageLinkModel
    {
        public string Self { get; set; }

        public string Html { get; set; }

        public string Download { get; set; }

        public string DownloadLocation { get; set; }
    }
}