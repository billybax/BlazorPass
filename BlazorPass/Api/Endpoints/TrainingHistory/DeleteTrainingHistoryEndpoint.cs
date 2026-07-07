namespace BlazorPass.Api.Endpoints;

public static class DeleteTrainingHistoryEndpoint
{
    public static void MapDeleteTrainingHistory(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", DeleteTrainingHistory)
            .WithName("DeleteTrainingHistory")
            .WithDescription("Удалить запись тренировки (мягкое удаление)")
            .Produces(200)
            .Produces(404);
    }

    private static async Task<IResult> DeleteTrainingHistory(int id, ApplicationDbContext db)
    {
        var trainingHistory = await db.TrainingHistories.FindAsync(id);
        if (trainingHistory == null || trainingHistory.IsDeleted)
        {
            return Results.NotFound($"Запись с ID {id} не найдена");
        }

        trainingHistory.IsDeleted = true;
        trainingHistory.ServerUpdatedAt = DateTime.UtcNow;

        db.TrainingHistories.Update(trainingHistory);
        await db.SaveChangesAsync();

        return Results.Ok(new { message = "Запись успешно удалена", id });
    }
}
