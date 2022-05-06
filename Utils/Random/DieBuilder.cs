#nullable enable
using System.IO;
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

        public virtual IDice D2 { get; protected set; }
        public virtual IDice D3 { get; protected set; }
        public virtual IDice D4 { get; protected set; }
        public virtual IDice D5 { get; protected set; }
        public virtual IDice D6 { get; protected set; }
        public virtual IDice D7 { get; protected set; }
        public virtual IDice D8 { get; protected set; }
        public virtual IDice D9 { get; protected set; }
        public virtual IDice D10 { get; protected set; }
        public virtual IDice D12 { get; protected set; }
        public virtual IDice D20 { get; protected set; }

        public virtual ICompassDice<Compass4Points> Compass4Die { get; }
        public virtual ICompassDice<Compass8Points> Compass8Die { get; }
        private readonly Dictionary<string, IDice> _dice;

        public DieBuilder(string loadFolder = FileAndDirectoryHelpers.LoadFolder, Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            _loadFolder = loadFolder + FolderSuffix;
            _reset = reset;

            FileAndDirectoryHelpers.CreateLoadFolder(_loadFolder);

            D2 = new Die(1, 2, _loadFolder, _reset);
            D3 = new Die(1, 3, _loadFolder, _reset);
            D4 = new Die(1, 4, _loadFolder, _reset);
            D5 = new Die(1, 5, _loadFolder, _reset);
            D6 = new Die(1, 6, _loadFolder, _reset);
            D7 = new Die(1, 7, _loadFolder, _reset);
            D8 = new Die(1, 8, _loadFolder, _reset);
            D9 = new Die(1, 9, _loadFolder, _reset);
            D10 = new Die(1, 10, _loadFolder, _reset);
            D12 = new Die(1, 12, _loadFolder, _reset);
            D20 = new Die(1, 20, _loadFolder, _reset);

            _dice = new Dictionary<string, IDice>
            {
                {D2.Name, D2},
                {D3.Name, D3},
                {D4.Name, D4},
                {D5.Name, D5},
                {D6.Name, D6},
                {D7.Name, D7},
                {D8.Name, D8},
                {D9.Name, D9},
                {D10.Name, D10},
                {D12.Name, D12},
                {D20.Name, D20},
            };

            Compass4Die = new CompassDice<Compass4Points>(4, _loadFolder, _reset);
            Compass8Die = new CompassDice<Compass8Points>(8, _loadFolder, _reset);
        }

        public void NextTurn()
        {
            var dice = _dice.Values.Cast<Die>().ToList();

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
            GetDice(1, max);
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
            return GetDice(min, max);
        }

        private IDice GetDice(int min, int max)
        {
            var name = Die.NameFormat(min, max);
            if (_dice.TryGetValue(name, out var dice)) return dice;

            var newDice = new Die(min, max, _loadFolder, _reset);
            newDice.Populate();
            _dice[name] = newDice;

            return newDice;
        }

        public virtual IDice Dice(int max)
        {
            var die = GetDice(1, max);

            return die;
        }
    }
}
