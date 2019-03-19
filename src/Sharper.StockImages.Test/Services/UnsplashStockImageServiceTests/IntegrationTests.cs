using System;
using System.Threading.Tasks;
using Sharper.StockImages.Models;
using Sharper.StockImages.Services;
using Xunit;

namespace Sharper.StockImages.Test.Services.UnsplashStockImageServiceTests
{
    [Trait("Category", "Integration")]
    public class IntegrationTests : IDisposable
    {
        private readonly UnsplashStockImageService unsplashService;

        public IntegrationTests()
        {
            unsplashService = new UnsplashStockImageService();
        }

        [Fact(Skip = "Integration test")]
        public async Task GetsImage()
        {
            // Act
            var image = await unsplashService.GetRandomImage();

            // Assert
            Assert.NotNull(image);
            Assert.IsAssignableFrom<StockImageModel>(image);
            Assert.False(string.IsNullOrWhiteSpace(image.Id));
            Assert.False(string.IsNullOrWhiteSpace(image.ImageEmbedUrl));
        }

        public void Dispose()
        {
            unsplashService.Dispose();
        }
    }
}