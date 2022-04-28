#nullable enable
using System.IO;
using Utils.Enums;

namespace Utils.Random
{
    public class DieBuilder : IDieBuilder
    {
        private readonly string _loadFolder;
        private readonly Die.RandomiserReset _reset;

        public const string BetweenPrefix = "between";
        public const string NumberPrefix = "number";
        public const string CompassPrefix = "compass";
        public const string RandomExtension = "random";
        public const string IndexExtension = "index";
        public const string FolderSuffix = "RandomNumbers";

        public IDice D2 { get; }
        public IDice D3 { get; }
        public IDice D4 { get; }
        public IDice D5 { get; }
        public IDice D6 { get; }
        public IDice D7 { get; }
        public IDice D8 { get; }
        public IDice D9 { get; }
        public IDice D10 { get; }
        public IDice D12 { get; }
        public IDice D20 { get; }

        public ICompassDice<Compass4Points> Compass4Die { get; }
        public ICompassDice<Compass8Points> Compass8Die { get; }
        private readonly Dictionary<string, IDice> _dice;

        public DieBuilder(string loadFolder = FileAndDirectoryHelpers.LoadFolder, Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            _loadFolder = loadFolder + FolderSuffix;
            _reset = reset;

            if (reset != Die.RandomiserReset.None)
            {
                FileAndDirectoryHelpers.CreateLoadFolder(_loadFolder);
            }

            D2 = new Die(GetNormalDiceName(2), 1, 2, _loadFolder, _reset);
            D3 = new Die(GetNormalDiceName(3), 1, 3, _loadFolder, _reset);
            D4 = new Die(GetNormalDiceName(4), 1, 4, _loadFolder, _reset);
            D5 = new Die(GetNormalDiceName(5), 1, 5, _loadFolder, _reset);
            D6 = new Die(GetNormalDiceName(6), 1, 6, _loadFolder, _reset);
            D7 = new Die(GetNormalDiceName(7), 1, 7, _loadFolder, _reset);
            D8 = new Die(GetNormalDiceName(8), 1, 8, _loadFolder, _reset);
            D9 = new Die(GetNormalDiceName(9), 1, 9, _loadFolder, _reset);
            D10 = new Die(GetNormalDiceName(10), 1, 10, _loadFolder, _reset);
            D12 = new Die(GetNormalDiceName(12), 1, 12, _loadFolder, _reset);
            D20 = new Die(GetNormalDiceName(20), 1, 20, _loadFolder, _reset);

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

            Compass4Die = new CompassDice<Compass4Points>(GetName(CompassPrefix, 1, 4), 4, _loadFolder, _reset);
            Compass8Die = new CompassDice<Compass8Points>(GetName(CompassPrefix, 1, 8), 8, _loadFolder, _reset);

            //PopulateAllQueues();
            string GetNormalDiceName(int max)
            {
                return GetName(NumberPrefix, 1, max);
            }
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

        private void PopulateAllQueues()
        {
            PopulateQueues(NumberPrefix, PopulateNumberDice);
            PopulateQueues(BetweenPrefix, PopulateBetweenDice);
            PopulateQueues(CompassPrefix, PopulateCompassDice);
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
            GetDice(NumberPrefix, 1, max);
        }

        private void PopulateBetweenDice(string filename)
        {
            var split = filename.Split('_');
            var min = int.Parse(split[1]);
            var max = int.Parse(split[2]);
            GetDice(BetweenPrefix, min, max);
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

        string GetName(string prefix, int min, int max)
        {
            return $"{prefix}_{min}_{max}";
        }

        public IDice Between(int min, int max)
        {
            var name = GetName(BetweenPrefix, min, max);
            return GetDice(name, min, max);
        }

        private IDice GetDice(string name, int min, int max)
        {
            if (_dice.TryGetValue(name, out var dice)) return dice;

            var newDice = new Die(name, min, max, _loadFolder, _reset);
            newDice.Populate();
            _dice[name] = newDice;

            return newDice;
        }

        public IDice Dice(int max)
        {
            var name = GetName(NumberPrefix, 1, max);

            var die = GetDice(name, 1, max);

            return die;
        }
    }
}
