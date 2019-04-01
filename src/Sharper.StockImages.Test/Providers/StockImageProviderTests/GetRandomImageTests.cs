using System;
using System.Threading.Tasks;
using Moq;
using Sharper.StockImages.Exceptions;
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
            mockedService.Setup(s => s.RandomImageEnabled).Returns(true);
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
            var model = new StockImageModel { Id = id };
            mockedService.Setup(s => s.GetRandomImage()).ReturnsAsync(model);

            // Act
            var stockImage = await provider.GetRandomImage();

            // Assert
            Assert.Equal(id, stockImage.Id);
            Assert.Same(model, stockImage);
        }

        [Fact]
        public async Task OnlyCallsServicesWithRandomEnabled()
        {
            // Arrange
            mockedService.Setup(s => s.Id).Returns("1");
            mockedService.Setup(s => s.GetRandomImage()).ReturnsAsync((StockImageModel) null);
            var randomDisabledService = new Mock<IStockImageService>();
            randomDisabledService.Setup(s => s.Id).Returns("2");
            randomDisabledService.Setup(s => s.RandomImageEnabled).Returns(false);
            randomDisabledService.Setup(s => s.GetRandomImage()).ReturnsAsync((StockImageModel) null);
            var random = Mock.Of<Random>(r => r.Next(It.IsAny<int>()) == 0);
            var randomProvider = new StockImageProvider(randomDisabledService.Object, mockedService.Object);

            // Act
            await randomProvider.GetRandomImage(random);

            // Assert
            mockedService.Verify(s => s.GetRandomImage(), Times.Once);
            randomDisabledService.Verify(s => s.GetRandomImage(), Times.Never);
        }

        [Fact]
        public void ThrowsWhenNoValidService()
        {
            // Arrange
            var emptyProvider = new StockImageProvider(new IStockImageService[] { });

            // Act
            var exception = Record.ExceptionAsync(async () => await emptyProvider.GetRandomImage());

            // Assert
            Assert.NotNull(exception);
            Assert.NotNull(exception.Result);
            Assert.IsAssignableFrom<NoValidServiceException>(exception.Result);
            Assert.Equal("No available services support random images.", exception.Result.Message);
        }
    }
}