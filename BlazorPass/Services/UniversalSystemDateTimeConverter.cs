using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorPass.Services
{
    public class UniversalSystemDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Вариант 1: Пришло число (Unix Timestamp в миллисекундах)
            if (reader.TokenType == JsonTokenType.Number)
            {
                long milliseconds = reader.GetInt64();
                // .UtcDateTime гарантирует, что C# пометит дату как UTC
                return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).UtcDateTime;
            }

            // Вариант 2: Пришла строка (например, "2026-07-07 08:17:21" или "2026-07-07T08:17:21")
            if (reader.TokenType == JsonTokenType.String)
            {
                string dateStr = reader.GetString()!;
                if (DateTime.TryParse(dateStr, out DateTime parsedDate))
                {
                    // КРИТИЧЕСКИ ВАЖНО ДЛЯ POSTGRESQL:
                    // Если в строке не было указано смещение (Z или +00:00), 
                    // принудительно говорим системе, что эта дата в UTC.
                    return DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
                }
            }

            throw new JsonException($"Не удалось сконвертировать значение в тип DateTime.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Переводим в UTC (на случай, если дата локальная) и записываем в стандартном ISO формате
            // На конце будет добавлена 'Z' (например, 2026-07-07T08:17:21Z)
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }
}