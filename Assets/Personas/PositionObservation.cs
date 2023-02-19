#nullable enable
using Utils.Coordinates;

namespace Assets.Personas;

public record class PositionObservation(ICharacter Character, (Coordinate Before, Coordinate After) Change);
