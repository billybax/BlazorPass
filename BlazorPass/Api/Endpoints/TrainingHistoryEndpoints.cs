namespace BlazorPass.Api.Endpoints;

public static class TrainingHistoryEndpoints
{
    public static void MapTrainingHistoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/training-history")
            .WithName("TrainingHistory")
            .WithDescription("API для управления историей тренировок");

        group.MapGetTrainingHistory();
        group.MapCreateTrainingHistory();
        group.MapUpdateTrainingHistory();
        group.MapDeleteTrainingHistory();
        group.MapSyncTrainingHistory();
    }
}

