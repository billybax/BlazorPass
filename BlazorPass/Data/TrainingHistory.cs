using BlazorPass.Services;
using System.Text.Json.Serialization;

public class TrainingHistory
{
    public int Id { get; set; }
    public long? UserId { get; set; }
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
    
    [JsonConverter(typeof(UniversalSystemDateTimeConverter))]
    public DateTime ClientUpdatedAt { get; set; }
    public DateTime ServerUpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
