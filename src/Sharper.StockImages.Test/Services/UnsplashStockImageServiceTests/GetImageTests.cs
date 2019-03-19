using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Sharper.StockImages.Models;
using Sharper.StockImages.Services;
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
            unsplashService = new UnsplashStockImageService(client);
        }

        [Fact]
        public void InheritsFromInterface()
        {
            // Arrange
            var iService = unsplashService as IStockImageService;

            // Act
            iService.GetImage();
        }

        [Fact]
        public async Task ReturnsStockImageModel()
        {
            // Arrange
            mockedHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(GetBasicResponse);

            // Act
            var stockImage = await unsplashService.GetImage();

            // Assert
            Assert.NotNull(stockImage);
            Assert.IsAssignableFrom<StockImageModel>(stockImage);
        }

        [Fact]
        public async Task ReturnedModelHasServiceReference()
        {
            // Arrange
            mockedHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(GetBasicResponse);

            // Act
            var stockImage = await unsplashService.GetImage();

            // Assert
            Assert.NotNull(stockImage);
            Assert.Equal(unsplashService.Id, stockImage.ServiceId);
        }

        [Fact]
        public async Task CallsHttpClientGet()
        {
            // Arrange
            mockedHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(GetBasicResponse);

            // Act
            await unsplashService.GetImage();

            // Assert
            mockedHttpHandler.Protected().Verify("SendAsync", Times.AtLeastOnce(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get), ItExpr.IsAny<CancellationToken>());
        }

        public void Dispose()
        {
            unsplashService.Dispose();
        }

        private static HttpResponseMessage GetBasicResponse()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    @"{""id"":""testId"",""width"":500,""height"":500," +
                    @"""urls"":{""raw"":""http://example.com/"",""full"":""http://example.com/"",""regular"":""http://example.com/"",""small"":""http://example.com/"",""thumb"":""http://example.com/""}," +
                    @"""links"":{""self"":""http://example.com/"",""html"":""http://example.com/"",""download"":""http://example.com/"",""download_location"":""http://example.com/""}," +
                    @"""user"":{""id"":""testUserId"",""username"":""testUserName"",""name"":""testUser""," +
                    @"""links"":{""self"":""http://example.com/"",""html"":""http://example.com/""}}}",
                    Encoding.UTF8, "application/json")
            };
        }
    }
}