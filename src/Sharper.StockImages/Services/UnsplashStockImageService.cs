using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sharper.StockImages.Models;

namespace Sharper.StockImages.Services
{
    public class UnsplashStockImageService : IStockImageService, IDisposable
    {
        public const string UniqueId = "Sharper Unsplash Service v1";

        public virtual string Id => UniqueId;

        protected readonly HttpClient HttpClient;
        protected readonly NameValueCollection AppSettings;

        protected readonly string BaseUri = "https://api.unsplash.com/";

        public UnsplashStockImageService() : this(new HttpClient(), ConfigurationManager.AppSettings)
        {
        }

        public UnsplashStockImageService(HttpClient httpClient, NameValueCollection settings)
        {
            AppSettings = settings ?? throw new ArgumentNullException(nameof(settings));

            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            HttpClient.BaseAddress = new Uri(BaseUri);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Client-ID", AppSettings[Constants.Settings.UnsplashClientId]);
            HttpClient.DefaultRequestHeaders.Add("Accept-Version", "v1");
        }

        public virtual async Task<StockImageModel> GetRandomImage()
        {
            var response = await HttpClient.GetAsync("/photos/random");
            if (!response.IsSuccessStatusCode)
            {
                // TODO: Error handling
                return null;
            }

            return await DeserializeImageResponse(response);
        }

        public virtual async Task<StockImageModel> GetImage(string id)
        {
            var response = await HttpClient.GetAsync($"/photos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                // TODO: Error handling
                return null;
            }

            return await DeserializeImageResponse(response);
        }

        public virtual void Dispose()
        {
            HttpClient.Dispose();
        }

        protected virtual async Task<StockImageModel> DeserializeImageResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var contractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };
            var serializerSettings = new JsonSerializerSettings { ContractResolver = contractResolver };

            var unsplashModel = await response.Content.ReadAsAsync<UnsplashImageModel>(new[]
                { new JsonMediaTypeFormatter { SerializerSettings = serializerSettings } });

            // TODO: Fire and forget download notifier:
            //await HttpClient.GetAsync(unsplashModel.Links.DownloadLocation);

            return new StockImageModel
            {
                Id = unsplashModel.Id,
                Width = unsplashModel.Width,
                Height = unsplashModel.Height,
                Description = unsplashModel.Description,
                ImageThumbUrl = unsplashModel.Urls.Thumb,
                ImageEmbedUrl = unsplashModel.Urls.Full,
                ImageServiceUrl = unsplashModel.Links.Html,
                CreatorUserName = unsplashModel.User.Username,
                CreatorName = unsplashModel.User.Name,
                CreatorServiceUrl = unsplashModel.User.Links.Html,
                ServiceId = Id
            };
        }
    }
}