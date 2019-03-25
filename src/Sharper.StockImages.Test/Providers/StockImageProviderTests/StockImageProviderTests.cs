using System.Collections.Generic;
using System.Linq;
using Sharper.StockImages.Providers;
using Sharper.StockImages.Services;
using Sharper.StockImages.Test.Mocks;
using Xunit;

namespace Sharper.StockImages.Test.Providers.StockImageProviderTests
{
    public class StockImageProviderTests
    {
        [Fact]
        public void ItExists()
        {
            // Act
            var provider = new StockImageProvider();

            // Assert
            Assert.NotNull(provider);
        }

        [Fact]
        public void HasServices()
        {
            // Act
            var provider = new StockImageProvider();

            // Assert
            Assert.NotNull(provider.Services);
            Assert.IsAssignableFrom<IEnumerable<IStockImageService>>(provider.Services);
            Assert.NotEmpty(provider.Services);
        }

        [Fact]
        public void AcceptsSpecificServices()
        {
            // Arrange
            // Using two of our fakes, doesn't matter which ones
            var firstService = new DisabledByIdService();
            var secondService = new DisabledByTypeService();

            // Act
            var provider = new StockImageProvider(firstService, secondService);

            // Assert
            Assert.Equal(2, provider.Services.Count());
            Assert.Contains(provider.Services, s => s.Id == firstService.Id);
            Assert.Contains(provider.Services, s => s.Id == secondService.Id);
        }

        [Fact]
        public void DoesNotHaveServicesDisabledById()
        {
            // DisablingByIdService has an attribute:
            //[DisableService(DisabledByIdService.UniqueId)]

            // Act
            var provider = new StockImageProvider();

            // Assert
            Assert.Contains(provider.Services, s => s.Id == DisablingByIdService.UniqueId);
            Assert.DoesNotContain(provider.Services, s => s.Id == DisabledByIdService.UniqueId);
        }

        [Fact]
        public void DoesNotHaveServicesDisabledByType()
        {
            // DisablingByTypeService has an attribute:
            //[DisableService(typeof(DisabledByTypeService))]

            // Act
            var provider = new StockImageProvider();

            // Assert
            Assert.Contains(provider.Services, s => s.Id == DisablingByTypeService.UniqueId);
            Assert.DoesNotContain(provider.Services, s => s.Id == DisabledByTypeService.UniqueId);
        }

        [Fact]
        public void RemovesDuplicateServices()
        {
            // Arrange
            // Using one of our fakes, doesn't matter which one, as long as the IDs are the same
            var service = new DisabledByIdService();
            var duplicateService = new DisabledByIdService();

            // Act
            var provider = new StockImageProvider(service, duplicateService);

            // Assert
            Assert.Single(provider.Services);
            Assert.Contains(provider.Services, s => s.Id == service.Id);
        }
    }
}