using System;
using System.Threading.Tasks;
using Moq;
using Sharper.StockImages.Models;
using Sharper.StockImages.Providers;
using Sharper.StockImages.Services;
using Xunit;

namespace Sharper.StockImages.Test.Providers.StockImageProviderTests
{
    public class GetRandomImageTests
    {
        private readonly Mock<IStockImageService> mockedService;
        private readonly StockImageProvider provider;

        public GetRandomImageTests()
        {
            mockedService = new Mock<IStockImageService>();
            provider = new StockImageProvider(mockedService.Object);
        }

        [Fact]
        public async Task ReturnsStockImageModel()
        {
            // Arrange
            mockedService.Setup(s => s.GetRandomImage()).ReturnsAsync(new StockImageModel());

            // Act
            var stockImage = await provider.GetRandomImage();

            // Assert
            Assert.IsAssignableFrom<StockImageModel>(stockImage);
        }

        [Fact]
        public async Task AcceptsRandom()
        {
            // Arrange
            mockedService.Setup(s => s.GetRandomImage()).ReturnsAsync((StockImageModel) null);
            var random = Mock.Of<Random>(r => r.Next(It.IsAny<int>()) == 0);

            // Act
            await provider.GetRandomImage(random);

            // Assert
            Mock.Get(random).Verify(r => r.Next(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task CallsServiceMethod()
        {
            // Arrange
            mockedService.Setup(s => s.GetRandomImage()).ReturnsAsync((StockImageModel) null);

            // Act
            await provider.GetRandomImage();

            // Assert
            mockedService.Verify(s => s.GetRandomImage(), Times.Once);
        }

        [Fact]
        public async Task ReturnsModelFromServiceMethod()
        {
            // Arrange
            const string id = "XXXXXXXX";
            mockedService.Setup(s => s.GetRandomImage()).ReturnsAsync(new StockImageModel { Id = id });

            // Act
            var stockImage = await provider.GetRandomImage();

            // Assert
            Assert.Equal(id, stockImage.Id);
        }
    }
}