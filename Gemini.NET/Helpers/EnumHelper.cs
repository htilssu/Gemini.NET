using System.ComponentModel;
using System.Reflection;

namespace GeminiDotNET.Helpers
{
    /// <summary>
    /// Helper class for enum operations
    /// </summary>
    internal static class EnumHelper
    {
        /// <summary>
        /// Get the description of an enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }
            return value.ToString();
        }

        public static IEnumerable<T> GetAllValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
