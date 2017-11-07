namespace CalendarBackend
{
    using Newtonsoft.Json;
    using NodaTime;
    using NodaTime.Serialization.JsonNet;
    using NodaTime.Text;
    using NodaTime.Utility;
    using System;
    using System.Collections.Generic;

    public static class NodaExtensions
    {
        /// <summary>
        /// Configures the given serializer settings to use <see cref="NodaConverters.IsoDateIntervalConverter"/>.
        /// Any other converters which can convert <see cref="DateInterval"/> are removed from the serializer.
        /// </summary>
        /// <param name="settings">The existing serializer settings to add Noda Time converters to.</param>
        /// <returns>The original <paramref name="settings"/> value, for further chaining.</returns>
        public static JsonSerializerSettings WithIsoDateIntervalConverter(this JsonSerializerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            ReplaceExistingConverters<DateInterval>(settings.Converters, new NodaIsoDateIntervalConverter());
            return settings;
        }

        private static void ReplaceExistingConverters<T>(IList<JsonConverter> converters, JsonConverter newConverter)
        {
            for (int i = converters.Count - 1; i >= 0; i--)
            {
                if (converters[i].CanConvert(typeof(T)))
                {
                    converters.RemoveAt(i);
                }
            }
            converters.Add(newConverter);
        }

        public sealed class NodaIsoDateIntervalConverter : NodaConverterBase<DateInterval>
        {
            protected override DateInterval ReadJsonImpl(JsonReader reader, JsonSerializer serializer)
            {
                if (reader.TokenType != JsonToken.String)
                {
                    throw new InvalidNodaDataException(
                        $"Unexpected token parsing DateInterval. Expected String, got {reader.TokenType}.");
                }
                string text = reader.Value.ToString();
                int slash = text.IndexOf('/');
                if (slash == -1)
                {
                    throw new InvalidNodaDataException("Expected ISO-8601-formatted date interval; slash was missing.");
                }

                string startText = text.Substring(0, slash);
                if (startText == "")
                {
                    throw new InvalidNodaDataException("Expected ISO-8601-formatted date interval; start date was missing.");
                }

                string endText = text.Substring(slash + 1);
                if (endText == "")
                {
                    throw new InvalidNodaDataException("Expected ISO-8601-formatted date interval; end date was missing.");
                }

                var pattern = LocalDatePattern.Iso;
                var start = pattern.Parse(startText).Value;
                var end = pattern.Parse(endText).Value;

                return new DateInterval(start, end);
            }

            /// <summary>
            /// Serializes the date interval as start/end instants.
            /// </summary>
            /// <param name="writer">The writer to write JSON to</param>
            /// <param name="value">The date interval to serialize</param>
            /// <param name="serializer">The serializer for embedded serialization.</param>
            protected override void WriteJsonImpl(JsonWriter writer, DateInterval value, JsonSerializer serializer)
            {
                var pattern = LocalDatePattern.Iso;
                string text = pattern.Format(value.Start) + "/" + pattern.Format(value.End);
                writer.WriteValue(text);
            }
        }
    }
}
