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

/// <summary>
/// Запрос на дельта-синхронизацию (Pull фаза).
/// Клиент передает время последней успешной синхронизации.
/// </summary>
public class PullTrainingHistoryRequest
{
    public string ClientId { get; set; }
    /// <summary>
    /// ID пользователя
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Unix timestamp (в секундах или миллисекундах) времени последней синхронизации.
    /// Если 0 или null, сервер вернет все записи пользователя.
    /// </summary>
    public long? SinceTimestamp { get; set; }
}

/// <summary>
/// Элемент в ответе Pull запроса.
/// Содержит всю информацию для слияния на клиента с учетом Local Wins конфликтов.
/// </summary>
public class SyncItemDto
{
    /// <summary>
    /// Серверный ID (используется для поиска локальной записи)
    /// </summary>
    public int ServerId { get; set; }

    /// <summary>
    /// Локальный ID (если был сгенерирован на клиента)
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// ID клиента (для связи между локальным и серверным ID на первое создание)
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Дата тренировки
    /// </summary>
    public DateOnly? TrainDate { get; set; }

    /// <summary>
    /// Время тренировки
    /// </summary>
    public TimeOnly? TrainTime { get; set; }

    /// <summary>
    /// Продолжительность в минутах
    /// </summary>
    public int? Minutes { get; set; }

    /// <summary>
    /// Тип тренировки
    /// </summary>
    public string? Train { get; set; }

    /// <summary>
    /// КП до тренировки
    /// </summary>
    public int? KpBefore { get; set; }

    /// <summary>
    /// КП после тренировки
    /// </summary>
    public int? KpAfter { get; set; }

    /// <summary>
    /// R-фактор
    /// </summary>
    public string? RFaktor { get; set; }

    /// <summary>
    /// Практика
    /// </summary>
    public string? Practise { get; set; }

    /// <summary>
    /// Сон
    /// </summary>
    public string? Sleep { get; set; }

    /// <summary>
    /// Здоровье
    /// </summary>
    public string? Health { get; set; }

    /// <summary>
    /// Время последнего обновления на клиента (берется из ClientUpdatedAt)
    /// </summary>
    public DateTime ClientUpdatedAt { get; set; }

    /// <summary>
    /// Время последнего обновления на сервере
    /// </summary>
    public DateTime ServerUpdatedAt { get; set; }

    /// <summary>
    /// Флаг удаления.
    /// Если true, то запись была удалена на сервере и должна быть удалена на клиента.
    /// </summary>
    public bool IsDeleted { get; set; }
}

/// <summary>
/// Ответ на Pull запрос (дельта-синхронизация).
/// Содержит только изменившиеся записи с момента SinceTimestamp.
/// </summary>
public class PullTrainingHistoryResponse
{
    /// <summary>
    /// Список изменившихся/новых записей
    /// </summary>
    public List<SyncItemDto> Records { get; set; } = new();

    /// <summary>
    /// Общее количество выданных записей
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// Текущий timestamp сервера (для следующего Pull запроса клиента)
    /// </summary>
    public long CurrentServerTimestamp { get; set; }
}
