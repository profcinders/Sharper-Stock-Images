using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Sharper.StockImages.Attributes;
using Sharper.StockImages.Extensions;
using Sharper.StockImages.Models;
using Sharper.StockImages.Services;

namespace Sharper.StockImages.Providers
{
    public class StockImageProvider
    {
        private List<IStockImageService> services;
        public ReadOnlyCollection<IStockImageService> Services => services.AsReadOnly();

        public StockImageProvider()
        {
            services = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .Where(t => typeof(IStockImageService).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && t.Namespace != "Castle.Proxies")
                .Select(InstantiateService)
                .DistinctBy(s => s.Id).ToList();

            RemoveDisabledServices();
        }

        public StockImageProvider(params IStockImageService[] services)
        {
            this.services = services.DistinctBy(s => s.Id).ToList();
        }

        public async Task<StockImageModel> GetRandomImage(Random random = null)
        {
            random = random ?? new Random();
            var index = random.Next(services.Count);
            var randomService = services[index];
            return await randomService.GetRandomImage();
        }

        protected void RemoveDisabledServices()
        {
            var disablingAttributes =
                services.SelectMany(s => s.GetType().GetCustomAttributes<DisableServiceAttribute>()).ToList();

            var disablingIds = disablingAttributes.Select(a => a.DisabledId)
                .Where(id => !string.IsNullOrWhiteSpace(id)).ToList();

            var disablingTypes = disablingAttributes.Select(a => a.DisabledType)
                .Where(type => type != null).ToList();

            services = services.Where(s => !disablingIds.Contains(s.Id) && !disablingTypes.Contains(s.GetType()))
                .ToList();
        }

        private static IStockImageService InstantiateService(Type type)
        {
            var constructor = type.GetConstructor(Array.Empty<Type>());
            if (constructor == null)
            {
                throw new InvalidOperationException($"Stock Image Service {type.FullName} does not have a parameter-less constructor.");
            }

            return constructor.Invoke(Array.Empty<object>()) as IStockImageService;
        }
    }
}