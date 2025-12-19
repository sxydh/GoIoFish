using GoIoFish.Services.Interfaces;
using System.Threading.Tasks;

namespace GoIoFish.Services.Implementations
{
    public class LoginService : ILoginService
    {

        private readonly IPlaywrightService _playwrightService;

        public LoginService(IPlaywrightService playwrightService)
        {
            _playwrightService = playwrightService;
        }

        public async Task InitAsync() {
            await _playwrightService.InitAsync();
        }

        public async Task LoginAsync()
        {
            var page = _playwrightService.GetPage();
            await page.GotoAsync("https://www.bing.com");
        }

    }
}
