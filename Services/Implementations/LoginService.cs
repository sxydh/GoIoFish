using GoIoFish.Services.Interfaces;
using GooFish.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace GoIoFish.Services.Implementations
{
    internal class LoginService : ILoginService
    {

        private readonly IPlaywrightService _playwrightService;

        public LoginService(IPlaywrightService playwrightService)
        {
            _playwrightService = playwrightService;
        }
     
        public Task Login()
        {
            throw new NotImplementedException();
        }

    }
}
