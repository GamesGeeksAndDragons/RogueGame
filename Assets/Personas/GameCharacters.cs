#nullable enable
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Display;

namespace Assets.Personas;

public interface ICharacterPosition
{
    ICharacter? this[Coordinate position] { get; }
    ICharacter Me { get; }
}

public interface IGameCharacters : ICharacterPosition, IDisposable
{
    ICharacter this[string uniqueId] { get; }

    IEnumerable<ICharacter> Load(params string[] charactersState);

    void Move(Coordinate from, Coordinate to);
    void Add(ICharacter character);
    void Remove(ICharacter character);
    IEnumerable<ICharacter> GenerateRandomCharacters(int count);
}

internal class GameCharacters : IGameCharacters
{
    public IDispatchRegistry DispatchRegistry { get; }
    public ICharacterFactory CharacterFactory { get; }
    internal readonly Dictionary<Coordinate, ICharacter> Positions = new Dictionary<Coordinate, ICharacter>();
    internal readonly Dictionary<string, ICharacter> Characters = new Dictionary<string, ICharacter>();

    public GameCharacters(IDispatchRegistry dispatchRegistry, ICharacterFactory characterFactory)
    {
        dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
        characterFactory.ThrowIfNull(nameof(characterFactory));

        DispatchRegistry = dispatchRegistry;
        CharacterFactory = characterFactory;

        Me = CharacterFactory.LoadCharacter("@", "");
    }

    public ICharacter? this[Coordinate position] => !Positions.ContainsKey(position) ? null : Positions[position];
    public ICharacter this[string uniqueId] => Characters[uniqueId];

    public ICharacter Me { get; private set; }

    public void Move(Coordinate from, Coordinate to)
    {
        var fromCharacter = this[from];
        var toCharacter = this[to];

        if (fromCharacter != null && toCharacter == null)
        {
            Positions.Remove(from);
            Positions.Add(to, fromCharacter);
        }
    }

    public void Add(ICharacter character)
    {
        Positions.Add(character.Coordinates, character);
        Characters.Add(character.UniqueId, character);
    }

    public void Remove(ICharacter character)
    {
        Positions.Remove(character.Coordinates);
        Characters.Remove(character.UniqueId);
        character.Dispose();
    }

    public IEnumerable<ICharacter> GenerateRandomCharacters(int count)
    {
        throw new NotImplementedException();
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

        return Positions.Values.ToList();
    }

    public void Dispose()
    {
        Me.Dispose();

        if (Characters.Count == 0) return;

        foreach (var character in Characters.Values)
        {
            character.Dispose();
        }

        Characters.Clear();
        Positions.Clear();
    }
}
