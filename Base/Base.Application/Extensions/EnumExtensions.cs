using System.ComponentModel;
using System.Reflection;

namespace Base.Utilities.Extensions
{
    public static partial class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? value.ToString();
        }

        public static List<KeyValueDto> ToKeyValueList<TEnum>()
            where TEnum : struct, Enum
        {
            return ToKeyValueList<TEnum>(null);
        }

        public static List<KeyValueDto> ToKeyValueList<TEnum>(string search)
            where TEnum : struct, Enum
        {
            var query = Enum.GetValues<TEnum>()
                .Select(x => new KeyValueDto(
                    Convert.ToInt32(x),
                    x.GetDescription()
                ));

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Value.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.Key.ToString().Contains(search)
                );
            }

            return [.. query];
        }
    }
}
