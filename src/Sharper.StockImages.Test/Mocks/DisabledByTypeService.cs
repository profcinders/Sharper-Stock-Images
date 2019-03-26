using System.Threading.Tasks;
using Sharper.StockImages.Models;
using Sharper.StockImages.Services;

namespace Sharper.StockImages.Test.Mocks
{
    public class DisabledByTypeService : IStockImageService
    {
        public const string UniqueId = "Disabled Service By Type";

        public string Id => UniqueId;

        public bool RandomImageEnabled => false;

        public Task<StockImageModel> GetRandomImage()
        {
            throw new System.NotImplementedException();
        }

        public Task<StockImageModel> GetImage(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}