namespace Assets.Tiles
{
    public interface ITile
    {
        (int x, int y) Corrdinates { get; }
        bool IsEmpty { get; }
        bool HasFloor { get; }
    }
}