namespace DrugBot.Vk.Bot;

internal class VkUser : IVkUser
{
    public long? PeerId { get; }
    public long? UserId { get; }
    public string? Status { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public long Id => UserId ?? 0;

    public VkUser(long? userId, long? peerId, string? status, string? firstName, string? lastName)
    {
        UserId = userId;
        PeerId = peerId;
        Status = status;
        FirstName = firstName;
        LastName = lastName;
    }

    public override int GetHashCode() => UserId.GetHashCode();

    public override string ToString() => UserId?.ToString() ?? "";
}