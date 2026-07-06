using BlazorPass.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazorPass.Api.Endpoints;

public static class SyncTrainingHistoryEndpoint
{
    public static void MapSyncTrainingHistory(this RouteGroupBuilder group)
    {
        group.MapPost("/sync", SyncTrainingHistory)
            .WithName("SyncTrainingHistory")
            .WithDescription("Синхронизировать тренировоки")
            .Accepts<SyncTrainingHistoryRequest>("application/json")
            .Produces<SyncTrainingHistoryResponse>(200)
            .Produces(400);
    }

    private static async Task<IResult> SyncTrainingHistory(SyncTrainingHistoryRequest request, ApplicationDbContext db)
    {
        if (request?.Records == null || request.Records.Count == 0)
        {
            return Results.BadRequest("Records не может быть пустым");
        }

        var response = new SyncTrainingHistoryResponse
        {
            TotalRecords = request.Records.Count,
            LocalIds = new List<int?>()
        };

        try
        {
            // Используем raw SQL для выполнения UPSERT операции
            foreach (var record in request.Records)
            {
                var result = await UpsertTrainingHistoryRecord(db, record);
                if (result.HasValue)
                {
                    response.LocalIds.Add(result);
                    response.SuccessfulRecords++;
                }
            }

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"Ошибка синхронизации: {ex.Message}");
        }
    }

    private static async Task<int?> UpsertTrainingHistoryRecord(ApplicationDbContext db, CreateTrainingHistoryRequest record)
    {
        // Проверяем, существует ли запись с таким (user_id, local_id)
        var existingRecord = await db.TrainingHistories
            .FirstOrDefaultAsync(x => x.UserId == record.UserId && x.LocalId == record.LocalId);

        if (existingRecord != null)
        {
            // Обновляем только если данные с телефона новее тех, что уже в БД
            if (record.ClientUpdatedAt > existingRecord.ClientUpdatedAt)
            {
                existingRecord.TrainDate = record.TrainDate;
                existingRecord.TrainTime = record.TrainTime;
                existingRecord.Minutes = record.Minutes;
                existingRecord.Train = record.Train;
                existingRecord.KpBefore = record.KpBefore;
                existingRecord.KpAfter = record.KpAfter;
                existingRecord.RFaktor = record.RFaktor;
                existingRecord.Practise = record.Practise;
                existingRecord.Sleep = record.Sleep;
                existingRecord.Health = record.Health;
                existingRecord.ClientId = record.ClientId;
                existingRecord.ClientUpdatedAt = record.ClientUpdatedAt;
                existingRecord.UpdatedAt = DateTime.UtcNow;
                existingRecord.IsDeleted = false;

                db.TrainingHistories.Update(existingRecord);
                await db.SaveChangesAsync();
            }

            return existingRecord.LocalId;
        }
        else
        {
            // Вставляем новую запись
            var newRecord = new TrainingHistory
            {
                UserId = record.UserId,
                LocalId = record.LocalId,
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
                ClientId = record.ClientId,
                ClientUpdatedAt = record.ClientUpdatedAt,
                UpdatedAt = DateTime.UtcNow,
                ServerUpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            db.TrainingHistories.Add(newRecord);
            await db.SaveChangesAsync();

            return newRecord.LocalId;
        }
    }
}
