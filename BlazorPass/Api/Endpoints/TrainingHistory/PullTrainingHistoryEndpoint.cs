using BlazorPass.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazorPass.Api.Endpoints;

/// <summary>
/// Эндпоинт для Pull-фазы синхронизации с дельта-запросом.
/// Реализует стратегию разрешения конфликтов "Local Wins":
/// - Клиент отправляет время последней успешной синхронизации
/// - Сервер возвращает только измененные/новые записи
/// - На клиента возвращается всю информацию для правильного слияния
/// </summary>
public static class PullTrainingHistoryEndpoint
{
    public static void MapPullTrainingHistory(this RouteGroupBuilder group)
    {
        group.MapGet("/pull", PullTrainingHistory)
            .WithName("PullTrainingHistory")
            .WithDescription("Дельта-синхронизация (Pull). Возвращает только измененные записи с момента since_timestamp")
            .Produces<PullTrainingHistoryResponse>(200)
            .Produces(400);
    }

    /// <summary>
    /// Обработчик Pull-запроса.
    /// 
    /// Алгоритм:
    /// 1. Клиент передает user_id и since_timestamp (время последней успешной синхронизации)
    /// 2. Сервер находит все записи, у которых ServerUpdatedAt > since_timestamp
    /// 3. Сервер возвращает эти записи + информацию о текущем timestamp сервера
    /// 4. На клиента это используется для слияния с логикой Local Wins
    /// </summary>
    private static async Task<IResult> PullTrainingHistory(
        string clientId,
        long? sinceTimestamp,
        ApplicationDbContext db)
    {
        try
        {
            // Валидация
            if (clientId == null || clientId.Length == 0)
            {
                return Results.BadRequest("clientId обязателен");
            }

            // Если sinceTimestamp не передан, берем 0 (все записи)
            var since = sinceTimestamp ?? 0;

            // Преобразуем Unix timestamp в DateTime
            // Предполагаем, что sinceTimestamp в миллисекундах (как обычно в мобильных приложениях)
            var sinceDateTime = UnixTimeStampToDateTime(since);

            // Дельта-запрос: ищем записи, которые изменились после sinceTimestamp
            var changedRecords = await db.TrainingHistories
                .Where(x => x.ClientId == clientId && x.ServerUpdatedAt > sinceDateTime)
                .AsNoTracking()
                .ToListAsync();

            // Преобразуем в DTO для ответа
            var syncItems = changedRecords.Select(record => new SyncItemDto
            {
                ServerId = record.Id,
                UserId = record.UserId,               
                TrainDate = record.TrainDate,
                TrainTime = record.TrainTime,
                Minutes = record.Minutes,
                Train = record.Train,
                KpBefore = record.KpBefore,
                KpAfter = record.KpAfter,
                RFaktor = record.RFaktor,
                Practise = record.Practise,
                Sleep = record.Sleep,
                Health = record.Health,
                ClientUpdatedAt = record.ClientUpdatedAt,
                ServerUpdatedAt = record.ServerUpdatedAt,
                IsDeleted = record.IsDeleted
            }).ToList();

            // Получаем текущий timestamp сервера для следующего Pull запроса
            var currentServerTimestamp = DateTimeToUnixTimeStamp(DateTime.UtcNow);

            var response = new PullTrainingHistoryResponse
            {
                Records = syncItems,
                TotalRecords = syncItems.Count,
                CurrentServerTimestamp = currentServerTimestamp
            };

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"Ошибка при Pull синхронизации: {ex.Message}");
        }
    }

    /// <summary>
    /// Преобразует Unix timestamp (в миллисекундах) в DateTime (UTC)
    /// </summary>
    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        if (unixTimeStamp == 0)
        {
            // Если 0, возвращаем минимальную дату (первый Pull)
            return DateTime.MinValue;
        }

        // Предполагаем миллисекунды
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(unixTimeStamp);
        return dateTime;
    }

    /// <summary>
    /// Преобразует DateTime (UTC) в Unix timestamp (в миллисекундах)
    /// </summary>
    private static long DateTimeToUnixTimeStamp(DateTime dateTime)
    {
        var timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return (long)timeSpan.TotalMilliseconds;
    }
}
