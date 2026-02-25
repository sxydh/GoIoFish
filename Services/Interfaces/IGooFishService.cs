using System.Threading.Tasks;

namespace GoIoFish.Services.Interfaces
{
    public interface IGooFishService
    {
        Task<bool> LoginAsync();
    }
}