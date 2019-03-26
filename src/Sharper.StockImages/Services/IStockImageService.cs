using System.Threading.Tasks;
using Sharper.StockImages.Models;

namespace Sharper.StockImages.Services
{
    public interface IStockImageService
    {
        string Id { get; }

        bool RandomImageEnabled { get; }

        Task<StockImageModel> GetRandomImage();

        Task<StockImageModel> GetImage(string id);
    }
}