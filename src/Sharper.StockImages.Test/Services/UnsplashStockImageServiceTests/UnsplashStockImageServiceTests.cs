using System;
using System.Collections.Specialized;
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
        public void AcceptsHttpClientAndSettingsCollection()
        {
            // Arrange
            var client = new HttpClient();
            var settings = new NameValueCollection();

            // Act
            var service = new UnsplashStockImageService(client, settings);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void ThrowsOnNullHttpClient()
        {
            // Arrange
            HttpClient client = null;
            var settings = new NameValueCollection();

            // Act
            var exception = Record.Exception(() => new UnsplashStockImageService(client, settings));

            // Assert
            Assert.IsAssignableFrom<ArgumentNullException>(exception);
            Assert.Equal("httpClient", (exception as ArgumentNullException).ParamName);
        }

        [Fact]
        public void ThrowsOnNullSettingsCollection()
        {
            // Arrange
            var client = new HttpClient();
            NameValueCollection settings = null;

            // Act
            var exception = Record.Exception(() => new UnsplashStockImageService(client, settings));

            // Assert
            Assert.IsAssignableFrom<ArgumentNullException>(exception);
            Assert.Equal("settings", (exception as ArgumentNullException).ParamName);
        }

        [Fact]
        public void DisposesHttpClient()
        {
            // Arrange
            var client = new Mock<HttpMessageHandler>();
            client.Protected().Setup("Dispose", ItExpr.IsAny<bool>());
            var settings = new NameValueCollection();

            // Act
            new UnsplashStockImageService(new HttpClient(client.Object), settings).Dispose();

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