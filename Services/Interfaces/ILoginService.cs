using System.Threading.Tasks;

namespace GoIoFish.Services.Interfaces
{
    public interface ILoginService
    {

        Task InitAsync();
        Task LoginAsync();

    }
}
