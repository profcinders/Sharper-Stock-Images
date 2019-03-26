using System.Threading.Tasks;
using Sharper.StockImages.Attributes;
using Sharper.StockImages.Models;
using Sharper.StockImages.Services;

namespace Sharper.StockImages.Test.Mocks
{
    [DisableService(DisabledByIdService.UniqueId)]
    public class DisablingByIdService : IStockImageService
    {
        public const string UniqueId = "Disabling Service By Id";

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