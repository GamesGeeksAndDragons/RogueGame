#nullable enable
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Assets.Game
{
    internal class GameConfig
    {
        public GameConfig()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("game.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
