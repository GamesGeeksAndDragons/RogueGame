#nullable enable
namespace Utils
{
    public static class EnumHelpers
    {
        public static IEnumerable<T> Values<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T ToEnum<T>(this int value) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            var values = Values<T>();

            var enumValue = values.Single(e => e.ToString() == value);

            return enumValue;
        }
    }
}
