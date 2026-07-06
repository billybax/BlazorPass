namespace BlazorPass.Api.DTOs;

public class CreateTrainingHistoryRequest
{
    public long UserId { get; set; }
    public int? LocalId { get; set; }
    public DateOnly? TrainDate { get; set; }
    public TimeOnly? TrainTime { get; set; }
    public int? Minutes { get; set; }
    public string? Train { get; set; }
    public int? KpBefore { get; set; }
    public int? KpAfter { get; set; }
    public string? RFaktor { get; set; }
    public string? Practise { get; set; }
    public string? Sleep { get; set; }
    public string? Health { get; set; }
    public string? ClientId { get; set; }
    public DateTime ClientUpdatedAt { get; set; }
}

public class UpdateTrainingHistoryRequest
{
    public long? UserId { get; set; }
    public int? LocalId { get; set; }
    public DateOnly? TrainDate { get; set; }
    public TimeOnly? TrainTime { get; set; }
    public int? Minutes { get; set; }
    public string? Train { get; set; }
    public int? KpBefore { get; set; }
    public int? KpAfter { get; set; }
    public string? RFaktor { get; set; }
    public string? Practise { get; set; }
    public string? Sleep { get; set; }
    public string? Health { get; set; }
    public string? ClientId { get; set; }
    public DateTime? ClientUpdatedAt { get; set; }
}

public class SyncTrainingHistoryRequest
{
    public List<CreateTrainingHistoryRequest> Records { get; set; } = new();
}

public class SyncTrainingHistoryResponse
{
    public List<int?> LocalIds { get; set; } = new();
    public int TotalRecords { get; set; }
    public int SuccessfulRecords { get; set; }
}
