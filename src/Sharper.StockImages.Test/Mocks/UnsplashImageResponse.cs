using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sharper.StockImages.Test.Mocks
{
    public class UnsplashImageResponse
    {
        public UnsplashImageResponse()
        {
            Urls = new Urls();
            Links = new ImageLinks();
            User = new User { Links = new UserLinks() };
        }

        public string Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        public string Description { get; set; }
        public string AltDescription { get; set; }
        public Urls Urls { get; set; }
        public ImageLinks Links { get; set; }
        public User User { get; set; }

        public UnsplashImageResponse WithId(string id)
        {
            Id = id;
            return this;
        }

        public HttpResponseMessage ToResponseMessage()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent(ToString(), Encoding.UTF8, "application/json") };
        }

        public override string ToString()
        {
            var contractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };
            var serializerSettings = new JsonSerializerSettings { ContractResolver = contractResolver };

            return JsonConvert.SerializeObject(this, serializerSettings);
        }
    }

    public class Urls
    {
        public Uri Raw { get; set; }
        public Uri Full { get; set; }
        public Uri Regular { get; set; }
        public Uri Small { get; set; }
        public Uri Thumb { get; set; }
    }

    public class ImageLinks
    {
        public Uri Self { get; set; }
        public Uri Html { get; set; }
        public Uri Download { get; set; }
        public Uri DownloadLocation { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public Uri PortfolioUrl { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public UserLinks Links { get; set; }
    }

    public class UserLinks
    {
        public Uri Self { get; set; }
        public Uri Html { get; set; }
        public Uri Photos { get; set; }
        public Uri Likes { get; set; }
        public Uri Portfolio { get; set; }
        public Uri Following { get; set; }
        public Uri Followers { get; set; }
    }
}