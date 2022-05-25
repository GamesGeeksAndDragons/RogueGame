#nullable enable
using Utils.Coordinates;

namespace Assets.Personas;

public record class PositionObservation(string UniqueId, (Coordinate Before, Coordinate After) Change);
