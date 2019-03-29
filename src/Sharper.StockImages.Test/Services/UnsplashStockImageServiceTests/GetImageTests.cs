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
    public class GetImageTests : IDisposable
    {
        private readonly Mock<HttpMessageHandler> mockedHttpHandler;
        private readonly UnsplashStockImageService unsplashService;

        public GetImageTests()
        {
            mockedHttpHandler = new Mock<HttpMessageHandler>();
            var client = new HttpClient(mockedHttpHandler.Object);
            var settings = new NameValueCollection();
            unsplashService = new UnsplashStockImageService(client, settings);
        }

        [Fact]
        public async Task InheritsFromInterface()
        {
            // Arrange
            ArrangeHttpHandler(new UnsplashImageResponse());
            var iService = unsplashService as IStockImageService;

            // Act
            await iService.GetImage("");
        }

        [Fact]
        public async Task ReturnsStockImageModel()
        {
            // Arrange
            ArrangeHttpHandler(new UnsplashImageResponse());

            // Act
            var image = await unsplashService.GetImage("");

            // Assert
            Assert.NotNull(image);
            Assert.IsAssignableFrom<StockImageModel>(image);
        }

        [Theory]
        [InlineData("XXXXXXXX")]
        [InlineData("1234567890")]
        public async Task ReturnsCorrectId(string id)
        {
            // Arrange
            ArrangeHttpHandler(new UnsplashImageResponse().WithId(id));

            // Act
            var image = await unsplashService.GetImage(id);

            // Assert
            Assert.NotNull(image);
            Assert.Equal(id, image.Id);
        }

        [Theory]
        [InlineData("XXXXXXXX")]
        [InlineData("1234567890")]
        public async Task CallsUnsplashPhotoEndpoint(string id)
        {
            // Arrange
            ArrangeHttpHandler(new UnsplashImageResponse().WithId(id));

            // Act
            await unsplashService.GetImage(id);

            // Assert
            mockedHttpHandler.Protected().Verify("SendAsync", Times.AtLeastOnce(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get && req.RequestUri.PathAndQuery == $"/photos/{id}"),
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
            const string id = "";

            // Act
            var exception = Record.ExceptionAsync(async () => await unsplashService.GetImage(id));

            // Assert
            Assert.NotNull(exception);
            Assert.NotNull(exception.Result);
            Assert.IsAssignableFrom<ImageNotFoundException>(exception.Result);
            Assert.Equal(id, (exception.Result as ImageNotFoundException)?.ImageId);
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
                .ReturnsAsync(() => response.ToResponseMessage());
        }
    }
}