using BlazorPass.Api.DTOs;

namespace BlazorPass.Api.Endpoints;

public static class UpdateTrainingHistoryEndpoint
{
    public static void MapUpdateTrainingHistory(this RouteGroupBuilder group)
    {
        group.MapPut("/{id}", UpdateTrainingHistory)
            .WithName("UpdateTrainingHistory")
            .WithDescription("Обновить запись тренировки по ID")
            .Accepts<UpdateTrainingHistoryRequest>("application/json")
            .Produces(200)
            .Produces(404)
            .Produces(400);
    }

    private static async Task<IResult> UpdateTrainingHistory(int id, UpdateTrainingHistoryRequest request, ApplicationDbContext db)
    {
        if (request == null)
        {
            return Results.BadRequest("Request не может быть пустым");
        }

        var trainingHistory = await db.TrainingHistories.FindAsync(id);
        if (trainingHistory == null || trainingHistory.IsDeleted)
        {
            return Results.NotFound($"Запись с ID {id} не найдена");
        }

        if (request.TrainDate.HasValue)
            trainingHistory.TrainDate = request.TrainDate;
        if (request.TrainTime.HasValue)
            trainingHistory.TrainTime = request.TrainTime;
        if (request.Minutes.HasValue)
            trainingHistory.Minutes = request.Minutes;
        if (!string.IsNullOrEmpty(request.Train))
            trainingHistory.Train = request.Train;
        if (request.KpBefore.HasValue)
            trainingHistory.KpBefore = request.KpBefore;
        if (request.KpAfter.HasValue)
            trainingHistory.KpAfter = request.KpAfter;
        if (!string.IsNullOrEmpty(request.RFaktor))
            trainingHistory.RFaktor = request.RFaktor;
        if (!string.IsNullOrEmpty(request.Practise))
            trainingHistory.Practise = request.Practise;
        if (!string.IsNullOrEmpty(request.Sleep))
            trainingHistory.Sleep = request.Sleep;
        if (!string.IsNullOrEmpty(request.Health))
            trainingHistory.Health = request.Health;
        if (!string.IsNullOrEmpty(request.ClientId))
            trainingHistory.ClientId = request.ClientId;

        trainingHistory.UpdatedAt = DateTime.UtcNow;

        db.TrainingHistories.Update(trainingHistory);
        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            trainingHistory.Id,
            trainingHistory.TrainDate,
            trainingHistory.TrainTime,
            trainingHistory.Minutes,
            trainingHistory.Train,
            trainingHistory.KpBefore,
            trainingHistory.KpAfter,
            trainingHistory.RFaktor,
            trainingHistory.Practise,
            trainingHistory.Sleep,
            trainingHistory.Health,
            trainingHistory.ClientId,
            trainingHistory.UpdatedAt,
            trainingHistory.IsDeleted
        });
    }
}
