using DrugBot.Core.Bot;

namespace DrugBot.Tests;

public class UserStab : IUser
{
    public long Id { get; set; }
    public string Status { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}