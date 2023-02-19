#nullable enable
namespace Assets.Game;

internal class GameConfig
{
    public GameConfig()
    {
        //IConfiguration Configuration = new ConfigurationBuilder()
        //    .AddJsonFile("game.json", optional: true, reloadOnChange: true)
        //    .BuildNewLevel();

        Id = Guid.NewGuid();
        Name = string.Empty;
        Folder = Path.Join(".", FileAndDirectoryHelpers.LoadFolder, "Id");
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Folder { get; }
}
