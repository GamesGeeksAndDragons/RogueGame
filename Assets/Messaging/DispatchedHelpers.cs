#nullable enable
using Assets.Actors;
using Utils;
using Utils.Dispatching;

namespace Assets.Messaging
{
    public static class DispatchedHelpers
    {
        internal static bool IsTypeof<T>(this IDispatched? dispatched) 
            where T : Dispatched<T>
        {
            if (dispatched == null) return false;
            return dispatched.Name == Dispatched<T>.DispatchedName;
        }

        internal static bool IsWall(this IDispatched dispatched)
        {
            return dispatched.IsTypeof<Wall>();
        }

        internal static bool IsRock(this IDispatched dispatched)
        {
            return dispatched.IsTypeof<Rock>();
        }

        internal static bool IsFloor(this IDispatched dispatched)
        {
            return dispatched.IsTypeof<Floor>();
        }

        internal static bool IsDoor(this IDispatched dispatched)
        {
            return dispatched.IsTypeof<Door>();
        }

        internal static bool IsNull(this IDispatched dispatched)
        {
            return dispatched.IsTypeof<Null>();
        }

        internal static bool IsTile(this IDispatched dispatched)
        {
            return dispatched.IsRock() || dispatched.IsWall() || dispatched.IsFloor() || dispatched.IsDoor();
        }
    }
}