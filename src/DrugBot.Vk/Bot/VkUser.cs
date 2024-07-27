namespace DrugBot.Vk.Bot;

internal class VkUser : IVkUser
{
    public long? PeerId { get; }
    public long? UserId { get; }
    public string? Status { get; }
    public long Id => UserId ?? 0;

    public VkUser(long? userId, long? peerId, string? status)
    {
        UserId = userId;
        PeerId = peerId;
        Status = status;
    }

    public override int GetHashCode() => UserId.GetHashCode();

    public override string ToString() => UserId?.ToString() ?? "";
}