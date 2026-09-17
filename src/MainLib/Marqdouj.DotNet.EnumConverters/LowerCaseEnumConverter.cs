using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.EnumConverters
{
    /// <summary>
    /// Converts the Enum to lower case during Json Serialize operations. Deserialize is case-insensitive.
    /// Serialize: TopLeft becomes topleft, Bottom_Right becomes bottom_right.
    /// Deserialize topleft becomes TopLeft, bottom_right becomes Bottom_Right (case-insensitive).
    /// <code>
    /// <![CDATA[[JsonConverter(typeof(LowerCaseEnumConverter<Position>))]]]>
    /// public enum Position
    /// {
    ///   TopLeft,
    ///   Bottom_Right,
    ///   //etc...
    /// }
    /// </code>
    /// </summary>
    /// <remarks>
    /// I find this useful with some of third-party Typescript NPM packages I use with JSInterop.
    /// </remarks>
    /// <typeparam name="T"><see cref="Enum"/></typeparam>
    public class LowerCaseEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
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

            var enumText = reader.GetString();
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
            string enumName = value.ToString();
            if (string.IsNullOrEmpty(enumName))
            {
                writer.WriteNullValue();
                return;
            }

            var name = enumName.ToLower();
            writer.WriteStringValue(name);
        }
    }
}
