#nullable enable
using Utils.Coordinates;

namespace Assets.Characters;

public record class PositionObservation(ICharacter Character, (Coordinate Before, Coordinate After) Change);
