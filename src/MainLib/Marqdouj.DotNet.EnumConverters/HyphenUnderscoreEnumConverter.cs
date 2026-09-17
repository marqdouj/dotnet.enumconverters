using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.EnumConverters
{
    /// <summary>
    /// Manages the transition between underscores/hyphens during Json Serialize/Deserialize operations. Deserialize is case-insensitive.
    /// Serialize: Top_Left becomes Top-Left.
    /// Deserialize Top-Left becomes Top_Left (case-insensitive).
    /// <code>
    /// <![CDATA[[JsonConverter(typeof(HyphenUnderscoreEnumConverter<Position>))]]]>
    /// public enum Position
    /// {
    ///   Top_Left,
    ///   //etc...
    /// }
    /// </code>
    /// </summary>
    /// <remarks>
    /// I find this useful with some of third-party Typescript NPM packages I use with JSInterop.
    /// </remarks>
    /// <typeparam name="T"><see cref="Enum"/></typeparam>
    public class HyphenUnderscoreEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        private enum ConvertDirection { Read, Write }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException($"Expected string for enum {typeof(T).Name}");

            var enumText = ToEnumString(reader.GetString(), ConvertDirection.Read);
            if (string.IsNullOrWhiteSpace(enumText))
                throw new JsonException("Enum string value cannot be null or empty");

            if (Enum.TryParse(enumText, ignoreCase: true, out T value))
                return value;

            throw new JsonException($"Unable to convert \"{enumText}\" to {typeof(T).Name}");
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var enumName = ToEnumString(value.ToString(), ConvertDirection.Write);

            if (string.IsNullOrEmpty(enumName))
            {
                writer.WriteNullValue();
                return;
            }

            string lowerCaseName = enumName.ToLower();
            writer.WriteStringValue(lowerCaseName);
        }

        private static string? ToEnumString(string? enumString, ConvertDirection direction)
        {
            enumString = direction == ConvertDirection.Read ? enumString?.Replace("-", "_") : enumString?.Replace("_", "-");

            return enumString?.Trim();
        }
    }
}
