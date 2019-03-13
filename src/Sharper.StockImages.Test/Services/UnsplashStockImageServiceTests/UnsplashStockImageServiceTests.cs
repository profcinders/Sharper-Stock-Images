using System;
using System.Net.Http;
using Moq;
using Moq.Protected;
using Sharper.StockImages.Services;
using Xunit;

namespace Sharper.StockImages.Test.Services.UnsplashStockImageServiceTests
{
    public class UnsplashStockImageServiceTests
    {
        [Fact]
        public void ItExists()
        {
            // Act
            var service = new UnsplashStockImageService();

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void InheritsFromIStockImageService()
        {
            // Act
            var service = new UnsplashStockImageService();

            // Assert
            Assert.IsAssignableFrom<IStockImageService>(service);
        }

        [Fact]
        public void IsDisposable()
        {
            // Act
            var service = new UnsplashStockImageService();

            // Assert
            Assert.IsAssignableFrom<IDisposable>(service);
        }

        [Fact]
        public void TakesHttpClient()
        {
            // Arrange
            var client = new HttpClient();

            // Act
            var service = new UnsplashStockImageService(client);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void ThrowsOnNullHttpClient()
        {
            // Act
            var exception = Record.Exception(() => new UnsplashStockImageService(null));

            // Assert
            Assert.IsAssignableFrom<ArgumentNullException>(exception);
        }

        [Fact]
        public void DisposesHttpClient()
        {
            // Arrange
            var client = new Mock<HttpMessageHandler>();
            client.Protected().Setup("Dispose", ItExpr.IsAny<bool>());

            // Act
            new UnsplashStockImageService(new HttpClient(client.Object)).Dispose();

            // Assert
            client.Protected().Verify("Dispose", Times.Once(), ItExpr.IsAny<bool>());
        }

        [Fact]
        public void HasAnId()
        {
            // Arrange
            IStockImageService service = new UnsplashStockImageService();

            // Act
            var id = service.Id;

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(id));
        }
    }
}