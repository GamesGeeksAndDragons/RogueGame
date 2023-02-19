#nullable enable
using Utils;
using Utils.Coordinates;
using Utils.Display;
using Utils.Dispatching;

namespace Assets.Personas;

public interface ICharacterPosition
{
    ICharacter? this[Coordinate position] { get; }
    ICharacter Me { get; }
}

public interface IGameCharacters : ICharacterPosition, IDisposable
{
    IEnumerable<ICharacter> Characters { get; }
    ICharacter this[string uniqueId] { get; }

    IEnumerable<ICharacter> Load(params string[] charactersState);

    void Add(ICharacter character);
    void Position(ICharacter character, Coordinate before);
    void Remove(ICharacter character);
    IEnumerable<ICharacter> GenerateRandomCharacters(int numMonsters, int level, bool includeMe);
}

internal class GameCharacters : IGameCharacters
{
    private readonly Dictionary<Coordinate, ICharacter> _positions = new Dictionary<Coordinate, ICharacter>();
    private readonly Dictionary<string, ICharacter> _characters = new Dictionary<string, ICharacter>();

    public GameCharacters(IDispatchRegistry dispatchRegistry, ICharacterFactory characterFactory)
    {
        dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
        characterFactory.ThrowIfNull(nameof(characterFactory));

        DispatchRegistry = dispatchRegistry;
        CharacterFactory = characterFactory;

        Me = CharacterFactory.LoadCharacter("@", "");
    }

    public IDispatchRegistry DispatchRegistry { get; }
    private ICharacterFactory CharacterFactory { get; }
    public ICharacter? this[Coordinate position] => !_positions.ContainsKey(position) ? null : _positions[position];
    public IEnumerable<ICharacter> Characters => _characters.Values;
    public ICharacter this[string uniqueId] => _characters[uniqueId];

    public ICharacter Me { get; private set; }

    public void Add(ICharacter character)
    {
        if (_characters.ContainsKey(character.UniqueId)) return;

        _characters.Add(character.UniqueId, character);
    }

    public void Position(ICharacter character, Coordinate before)
    {
        if (before != Coordinate.NotSet)
        {
            _positions.Remove(before);
        }

        if (before != character.Coordinates)
        {
            _positions.Add(character.Coordinates, character);
        }
    }

    public void Remove(ICharacter character)
    {
        if (_positions.ContainsKey(character.Coordinates))
        {
            _positions.Remove(character.Coordinates);
        }

        _characters.Remove(character.UniqueId);
        character.Dispose();
    }

    public IEnumerable<ICharacter> GenerateRandomCharacters(int numMonsters, int level, bool includeMe)
    {
        var characters = new List<ICharacter>(numMonsters + (includeMe ? 1 : 0));

        if (includeMe)
        {
            Me = CharacterFactory.RandomCharacter(CharacterDisplay.Me, level);
            characters.Add(Me);
        }

        for (int i = 0; i < numMonsters; i++)
        {
            var monster = CharacterFactory.RandomCharacter(CharacterDisplay.DebugMonster, level);
            characters.Add(monster);
        }

        return characters;
    }

    public IEnumerable<ICharacter> Load(params string[] charactersState)
    {
        foreach (var state in charactersState)
        {
            var actor = state[0].ToString();
            const int parametersStartAt = 2;
            var parameters = state.Right(state.Length - parametersStartAt);

            var character = CharacterFactory.LoadCharacter(actor, parameters);
            Add(character);

            if (character.Actor.IsSame(CharacterDisplay.Me))
            {
                Me = character;
            }
        }

        return _characters.Values.ToList();
    }

    public void Dispose()
    {
        Me.Dispose();

        if (_characters.Count == 0) return;

        foreach (var character in _characters.Values)
        {
            character.Dispose();
        }

        _characters.Clear();
        _positions.Clear();
    }
}
