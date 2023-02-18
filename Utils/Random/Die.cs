#nullable enable
namespace Utils.Random;

public class Die : IDice
{
    private static readonly System.Random Generator = new System.Random();
    private const int NumberToAddToQueue = 1000;

    private readonly List<int> _randomNumbers = new List<int>();
    private int _diceIndex;
    private readonly string _fileWithRandomNumbers = null!;
    private readonly string _fileWithIndex = null!;

    private readonly int _min;
    private readonly int _max;
    public string Name { get; }

    public enum RandomiserReset
    {
        None, Index, Full
    }

    public Die(int min, int max, string loadFolder, RandomiserReset reset)
    {
        Name = NameFormat(min, max);

        if (min == max)
        {
            _min = min;
            _max = max;

            return;
        }

        _min = min;
        _max = max + 1; // Generator.Next(min, max); max is exclusive, so adding 1

        string GetFullyQualifiedName(string extension) => FileAndDirectoryHelpers.GetFullQualifiedName(loadFolder, Name.ChangeExtension(extension));

        _fileWithRandomNumbers = GetFullyQualifiedName(DieBuilder.RandomExtension);
        _fileWithIndex = GetFullyQualifiedName(DieBuilder.IndexExtension);

        if (reset == RandomiserReset.Full)
        {
            if (File.Exists(_fileWithRandomNumbers)) File.Delete(_fileWithRandomNumbers);
            if (File.Exists(_fileWithIndex)) File.Delete(_fileWithIndex);
        }
        else if (reset == RandomiserReset.Index && File.Exists(_fileWithIndex))
        {
            File.WriteAllText(_fileWithIndex, "0");
        }
    }


    public const string Prefix = "number";
    public static string NameFormat(int min, int max) => $"{Prefix}_{min}_{max}";


    internal void Populate()
    {
        if (_max == _min) return;

        LoadIndexFile();
        LoadRandomNumbersFile();

        void LoadIndexFile()
        {
            if (!File.Exists(_fileWithIndex))
            {
                _diceIndex = 0;
                File.WriteAllText(_fileWithIndex, _diceIndex.ToString());
                return;
            }

            var lines = File.ReadLines(_fileWithIndex).ToArray();
            if (lines.Length == 0)
            {
                _diceIndex = 0;
            }

            var index = lines[0];
            _diceIndex = int.Parse(index);
        }

        void LoadRandomNumbersFile()
        {
            if (!File.Exists(_fileWithRandomNumbers)) File.OpenWrite(_fileWithRandomNumbers).Dispose();

            var lines = File.ReadLines(_fileWithRandomNumbers).ToArray();
            foreach (var line in lines)
            {
                var number = int.Parse(line);
                _randomNumbers.Add(number);
            }
        }
    }

    internal void NextTurn()
    {
        if (_fileWithRandomNumbers.Length == 0) return;

        File.WriteAllLines(_fileWithIndex, new[] { _diceIndex.ToString() });
        File.WriteAllLines(_fileWithRandomNumbers, _randomNumbers.Select(number => number.ToString()));
    }

    private void AddRandomNumbers()
    {
        var random = new int[NumberToAddToQueue];
        for (int i = 0; i < NumberToAddToQueue; i++)
        {
            random[i] = Generator.Next(_min, _max); // max is exclusive
        }

        Save(random);

        _randomNumbers.AddRange(random);
    }

    private void Save(int[] random)
    {
        File.AppendAllLines(_fileWithRandomNumbers, random.Select(number => number.ToString()));
    }

    public int Random
    {
        get
        {
            if (_min == _max) return _max;

            bool HasBeenPopulated() => _randomNumbers.Count != 0;
            if (!HasBeenPopulated())
            {
                Populate();
            }

            var hasUsedAllNumbers = _randomNumbers.Count - 1 == _diceIndex;
            if (HasBeenPopulated() && !hasUsedAllNumbers)
            {
                return _randomNumbers[_diceIndex++];
            }

            AddRandomNumbers();

            return Random;
        }
    }
}
