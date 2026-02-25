using System.Threading.Tasks;
using GoIoFish.Models;
using GoIoFish.Models.GooFish;

namespace GoIoFish.Services.Interfaces
{
    public interface IGooFishService
    {
        Task<PageResult<Product>> FetchProductPageAsync(ProductPageRequest request);
    }
}