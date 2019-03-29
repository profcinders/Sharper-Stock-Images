using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Sharper.StockImages.Exceptions;
using Sharper.StockImages.Models;
using Sharper.StockImages.Services;
using Sharper.StockImages.Test.Mocks;
using Xunit;

namespace Sharper.StockImages.Test.Services.UnsplashStockImageServiceTests
{
    public class GetRandomImageTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> mockedHttpHandler;
        private readonly UnsplashStockImageService unsplashService;

        public GetRandomImageTests()
        {
            mockedHttpHandler = new Mock<HttpMessageHandler>();
            var client = new HttpClient(mockedHttpHandler.Object);
            var settings = new NameValueCollection();
            unsplashService = new UnsplashStockImageService(client, settings);
        }

        [Fact]
        public void RandomImagesEnabled()
        {
            // Assert
            Assert.True(unsplashService.RandomImageEnabled);
        }

        [Fact]
        public async Task ReturnsStockImageModel()
        {
            // Arrange
            ArrangeHttpHandler(new UnsplashImageResponse());

            // Act
            var stockImage = await unsplashService.GetRandomImage();

            // Assert
            Assert.NotNull(stockImage);
            Assert.IsAssignableFrom<StockImageModel>(stockImage);
        }

        [Fact]
        public async Task ReturnedModelHasServiceReference()
        {
            // Arrange
            ArrangeHttpHandler(new UnsplashImageResponse());

            // Act
            var stockImage = await unsplashService.GetRandomImage();

            // Assert
            Assert.NotNull(stockImage);
            Assert.Equal(unsplashService.Id, stockImage.ServiceId);
        }

        [Fact]
        public async Task CallsUnsplashRandomPhotoEndpoint()
        {
            // Arrange
            ArrangeHttpHandler(new UnsplashImageResponse());

            // Act
            await unsplashService.GetRandomImage();

            // Assert
            mockedHttpHandler.Protected().Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get && req.RequestUri.PathAndQuery == "/photos/random"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void ThrowsWhenImageNotFound()
        {
            // Arrange
            mockedHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.NotFound));

            // Act
            var exception = Record.ExceptionAsync(async () => await unsplashService.GetRandomImage());

            // Assert
            Assert.NotNull(exception);
            Assert.NotNull(exception.Result);
            Assert.IsAssignableFrom<ImageNotFoundException>(exception.Result);
        }

        public void Dispose()
        {
            unsplashService.Dispose();
        }

        private void ArrangeHttpHandler(UnsplashImageResponse response)
        {
            mockedHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response.ToResponseMessage);
        }
    }
}