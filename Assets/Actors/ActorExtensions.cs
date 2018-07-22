using Utils;

namespace Assets.Actors
{
    public static class ActorExtensions
    {
        public static T[] CloneActors<T>(this T[] array) where T : Actor
        {
            var clone = new T[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                T t = (T)array[i].Clone();
                clone[i] = t;
            }

            return clone;
        }

        public static T[,] CloneActors<T>(this T[,] array) where T : Actor
        {
            var maxRow = array.RowUpperBound();
            var maxCol = array.ColumnUpperBound();
            var clone = new T[maxRow+1, maxCol+1];

            for (var row = 0; row <= maxRow; row++)
            {
                for (int col = 0; col <= maxCol; col++)
                {
                    T t = (T)array[row, col].Clone();
                    clone[row, col] = t;
                }
            }

            return clone;
        }
    }
}
