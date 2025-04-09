using System.ComponentModel;
using System.Reflection;

namespace GeminiDotNET.Helpers
{
    /// <summary>
    /// Helper class for enum operations
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Get the description of an enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(Enum value)
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
    }
}
