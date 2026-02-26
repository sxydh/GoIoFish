using System.Threading.Tasks;
using GoIoFish.Models;

namespace GoIoFish.Services.Interfaces
{
    public interface IGooFishService
    {
        Task<PageResult<GooFishProduct>> FetchProductPageAsync(GooFishProductPageRequest request);
    }
}