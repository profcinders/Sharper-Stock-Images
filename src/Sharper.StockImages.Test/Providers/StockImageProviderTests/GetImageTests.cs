using System.Threading.Tasks;
using Moq;
using Sharper.StockImages.Exceptions;
using Sharper.StockImages.Models;
using Sharper.StockImages.Providers;
using Sharper.StockImages.Services;
using Xunit;

namespace Sharper.StockImages.Test.Providers.StockImageProviderTests
{
    public class GetImageTests
    {
        private readonly string defaultServiceId;
        private readonly Mock<IStockImageService> mockedService;
        private readonly StockImageProvider provider;

        public GetImageTests()
        {
            defaultServiceId = "default";
            mockedService = new Mock<IStockImageService>();
            mockedService.Setup(s => s.Id).Returns(defaultServiceId);
            provider = new StockImageProvider(mockedService.Object);
        }

        [Fact]
        public async Task ReturnsStockImageModel()
        {
            // Arrange
            mockedService.Setup(s => s.GetImage(It.IsAny<string>())).ReturnsAsync(new StockImageModel());

            // Act
            var stockImage = await provider.GetImage("", defaultServiceId);

            // Assert
            Assert.IsAssignableFrom<StockImageModel>(stockImage);
        }

        [Fact]
        public async Task CallsServiceMethod()
        {
            // Arrange
            const string id = "XXXXXXXX";
            mockedService.Setup(s => s.GetImage(It.IsAny<string>())).ReturnsAsync((StockImageModel) null);

            // Act
            await provider.GetImage(id, defaultServiceId);

            // Assert
            mockedService.Verify(s => s.GetImage(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CallsCorrectService()
        {
            // Arrange
            const string imageId = "XXXXXXXX";
            mockedService.Setup(s => s.GetImage(It.IsAny<string>())).ReturnsAsync((StockImageModel) null);
            var otherService = new Mock<IStockImageService>();
            otherService.Setup(s => s.Id).Returns("other");
            otherService.Setup(s => s.GetImage(It.IsAny<string>())).ReturnsAsync((StockImageModel) null);
            var imageProvider = new StockImageProvider(otherService.Object, mockedService.Object);

            // Act
            await imageProvider.GetImage(imageId, defaultServiceId);

            // Assert
            mockedService.Verify(s => s.GetImage(It.IsAny<string>()), Times.Once);
            otherService.Verify(s => s.GetImage(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ReturnsModelFromServiceMethod()
        {
            // Arrange
            const string imageId = "XXXXXXXX";
            var model = new StockImageModel { Id = imageId };
            mockedService.Setup(s => s.GetImage(It.IsAny<string>())).ReturnsAsync(model);

            // Act
            var stockImage = await provider.GetImage(imageId, defaultServiceId);

            // Assert
            Assert.Equal(imageId, stockImage.Id);
            Assert.Same(model, stockImage);
        }

        [Fact]
        public async Task RequestsCorrectImage()
        {
            // Arrange
            const string imageId = "XXXXXXXX";
            mockedService.Setup(s => s.GetImage(imageId)).ReturnsAsync((StockImageModel) null);
            mockedService.Setup(s => s.GetImage(It.Is<string>(v => v != imageId))).ReturnsAsync((StockImageModel) null);

            // Act
            await provider.GetImage(imageId, defaultServiceId);

            // Assert
            mockedService.Verify(s => s.GetImage(imageId), Times.Once);
            mockedService.Verify(s => s.GetImage(It.Is<string>(v => v != imageId)), Times.Never);
        }

        [Fact]
        public void ThrowsWhenInvalidService()
        {
            // Arrange
            const string invalidServiceId = "invalid";

            // Act
            var exception = Record.ExceptionAsync(async () => await provider.GetImage("", invalidServiceId));

            // Assert
            Assert.NotNull(exception);
            Assert.NotNull(exception.Result);
            Assert.IsAssignableFrom<NoValidServiceException>(exception.Result);
            Assert.Equal($@"The ""{invalidServiceId}"" service was not available.", exception.Result.Message);
        }

        [Fact]
        public void ThrowsWhenInvalidImageId()
        {
            // Arrange
            const string invalidImageId = "invalid";
            mockedService.Setup(s => s.GetImage(invalidImageId))
                .ThrowsAsync(new ImageNotFoundException { ImageId = invalidImageId });
            mockedService.Setup(s => s.GetImage(It.Is<string>(v => v != invalidImageId)))
                .ReturnsAsync((StockImageModel) null);

            // Act
            var exception =
                Record.ExceptionAsync(async () => await provider.GetImage(invalidImageId, defaultServiceId));

            // Assert
            Assert.NotNull(exception);
            Assert.NotNull(exception.Result);
            Assert.IsAssignableFrom<ImageNotFoundException>(exception.Result);
            Assert.Equal(invalidImageId, (exception.Result as ImageNotFoundException)?.ImageId);
        }
    }
}