using System;
using System.Threading.Tasks;
using GoIoFish.Models;

namespace GoIoFish.Services.Interfaces
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(IProgress<LoginProgressMsg> progress);
    }
}