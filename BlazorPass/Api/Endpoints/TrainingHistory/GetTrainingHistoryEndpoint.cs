using Microsoft.EntityFrameworkCore;
using BlazorPass.Api.DTOs;

namespace BlazorPass.Api.Endpoints;

public static class GetTrainingHistoryEndpoint
{
    public static void MapGetTrainingHistory(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetTrainingHistory)
            .WithName("GetTrainingHistory")
            .WithDescription("Получить список всех записей истории тренировок. Отсортирован по дате и времени в обратном порядке.")
            .Produces(200);
    }

    private static async Task<IResult> GetTrainingHistory(ApplicationDbContext db)
    {
        var trainingHistory = await db.TrainingHistories
            .Where(t => !t.IsDeleted)
            .OrderByDescending(t => t.TrainDate)
            .ThenByDescending(t => t.TrainTime)
            .AsAsyncEnumerable()
            .ToListAsync();

        var result = trainingHistory.Select(t => new 
        { 
            t.Id,
            t.UserId,
            t.LocalId,
            t.TrainDate,
            t.TrainTime,
            t.Minutes,
            t.Train,
            t.KpBefore,
            t.KpAfter,
            t.RFaktor,
            t.Practise,
            t.Sleep,
            t.Health,
            t.ClientId,
            t.ClientUpdatedAt,
            t.UpdatedAt,
            t.ServerUpdatedAt,
            t.IsDeleted
        }).ToList();

        return Results.Ok(result);
    }
}
