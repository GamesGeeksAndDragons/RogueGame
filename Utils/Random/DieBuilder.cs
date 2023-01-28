#nullable enable
using Utils.Enums;

namespace Utils.Random
{
    public class DieBuilder : IDieBuilder
    {
        private readonly string _loadFolder;
        private readonly Die.RandomiserReset _reset;

        public const string RandomExtension = "random";
        public const string IndexExtension = "index";
        public const string FolderSuffix = "RandomNumbers";

        public virtual ICompassDice<Compass4Points> Compass4Die { get; }
        public virtual ICompassDice<Compass8Points> Compass8Die { get; }
        protected readonly Dictionary<string, IDice> Dice;

        public DieBuilder(string loadFolder = FileAndDirectoryHelpers.LoadFolder, Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            _loadFolder = loadFolder + FolderSuffix;
            _reset = reset;

            FileAndDirectoryHelpers.CreateLoadFolder(_loadFolder);


            Dice = new Dictionary<string, IDice>();

            Compass4Die = new CompassDice<Compass4Points>(4, _loadFolder, _reset);
            Compass8Die = new CompassDice<Compass8Points>(8, _loadFolder, _reset);
        }

        public void NextTurn()
        {
            var dice = Dice.Values.Cast<Die>().ToList();

            foreach (var die in dice)
            {
                die.NextTurn();
            }

            ((CompassDice<Compass4Points>)Compass4Die).NextTurn();
            ((CompassDice<Compass8Points>)Compass8Die).NextTurn();
        }

        private void PopulateQueues(string prefix, Action<string> populator)
        {
            var filenames = GetFilenamesToLoad();
            filenames = FilterByExtensionAndPrefix(RandomExtension);

            foreach (var filename in filenames)
            {
                populator(filename);
            }

            IEnumerable<string> GetFilenamesToLoad()
            {
                var loadFolder = FileAndDirectoryHelpers.GetLoadDirectory(_loadFolder);
                var files = Directory.EnumerateFiles(loadFolder);

                return files.Select(filename => (Path.GetFileName(filename)));
            }

            IEnumerable<string> FilterByExtensionAndPrefix(string extension)
            {
                var dottedExtension = "." + extension;

                var filesWithExtension = filenames.Where(filename => filename.HasExtension(dottedExtension))
                    .ToList();

                return filesWithExtension
                    .Select(filename => (Path.GetFileNameWithoutExtension(filename)))
                    .Where(filename => filename.StartsWith(prefix));
            }
        }

        private void PopulateNumberDice(string filename)
        {
            var file = Path.ChangeExtension(filename, "");
            var max = int.Parse(file);
            Between(1, max);
        }

        private void PopulateCompassDice(string filename)
        {
            if (filename.IsSame(Compass4Die.Name))
            {
                ((CompassDice<Compass4Points>)Compass4Die).Populate();
            }
            else if (filename.IsSame(Compass8Die.Name))
            {
                ((CompassDice<Compass8Points>)Compass8Die).Populate();
            }
            else
            {
                throw new ArgumentException($"Attempting to populate unknown compass dice {filename}");
            }
        }

        public virtual IDice Between(string between)
        {
            var numbers = between.Split('B');
            var min = int.Parse(numbers[0]);
            var max = int.Parse(numbers[1]);

            return Between(min, max);
        }

        public virtual IDice Between(int min, int max)
        {
            var name = Die.NameFormat(min, max);
            if (Dice.TryGetValue(name, out var dice)) return dice;

            var newDice = new Die(min, max, _loadFolder, _reset);
            newDice.Populate();
            Dice[name] = newDice;

            return newDice;
        }
    }
}
