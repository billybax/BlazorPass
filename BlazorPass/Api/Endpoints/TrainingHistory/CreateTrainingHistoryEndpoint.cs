using BlazorPass.Api.DTOs;

namespace BlazorPass.Api.Endpoints;

public static class CreateTrainingHistoryEndpoint
{
    public static void MapCreateTrainingHistory(this RouteGroupBuilder group)
    {
        group.MapPost("/", CreateTrainingHistory)
            .WithName("CreateTrainingHistory")
            .WithDescription("Создать новую запись тренировки")
            .Accepts<CreateTrainingHistoryRequest>("application/json")
            .Produces(201)
            .Produces(400);
    }

    private static async Task<IResult> CreateTrainingHistory(CreateTrainingHistoryRequest request, ApplicationDbContext db)
    {
        if (request == null)
        {
            return Results.BadRequest("Request не может быть пустым");
        }

        var trainingHistory = new TrainingHistory
        {
            TrainDate = request.TrainDate,
            TrainTime = request.TrainTime,
            Minutes = request.Minutes,
            Train = request.Train,
            KpBefore = request.KpBefore,
            KpAfter = request.KpAfter,
            RFaktor = request.RFaktor,
            Practise = request.Practise,
            Sleep = request.Sleep,
            Health = request.Health,
            ClientId = request.ClientId,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        db.TrainingHistories.Add(trainingHistory);
        await db.SaveChangesAsync();

        return Results.Created($"/api/training-history/{trainingHistory.Id}", new
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
