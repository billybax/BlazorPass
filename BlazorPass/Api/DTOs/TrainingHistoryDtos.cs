namespace BlazorPass.Api.DTOs;

public class CreateTrainingHistoryRequest
{
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
}

public class UpdateTrainingHistoryRequest
{
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
}
