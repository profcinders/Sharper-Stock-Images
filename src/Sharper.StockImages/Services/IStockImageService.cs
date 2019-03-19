using System.Threading.Tasks;
using Sharper.StockImages.Models;

namespace Sharper.StockImages.Services
{
    public interface IStockImageService
    {
        string Id { get; }

        Task<StockImageModel> GetRandomImage();
    }
}