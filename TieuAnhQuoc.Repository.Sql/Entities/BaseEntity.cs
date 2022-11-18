namespace TieuAnhQuoc.Repository.Sql.Entities;

public class BaseEntity<TIdType>
{
    public TIdType Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public TIdType CreatedUserId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public TIdType UpdatedUserId { get; set; }

    public DateTime? DeletedAt { get; set; }

    public TIdType DeletedUserId { get; set; }

    public void SetCreated(TIdType currentUserId, DateTime? dateTime = null)
    {
        var utcNow = dateTime ?? DateTime.UtcNow;
        CreatedAt = utcNow;
        CreatedUserId = currentUserId;
        UpdatedAt = utcNow;
        UpdatedUserId = currentUserId;
    }

    public void SetUpdated(TIdType currentUserId, DateTime? dateTime = null)
    {
        var utcNow = dateTime ?? DateTime.UtcNow;
        UpdatedAt = utcNow;
        UpdatedUserId = currentUserId;
    }

    public void SetDeleted(TIdType currentUserId, DateTime? dateTime = null)
    {
        var utcNow = dateTime ?? DateTime.UtcNow;
        UpdatedAt = utcNow;
        DeletedAt = utcNow;
        DeletedUserId = currentUserId;
    }
}