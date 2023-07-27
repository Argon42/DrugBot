#nullable enable
using System;
using System.Linq;

namespace DrugBot.Bot.Vk;

[Serializable]
public class VkConfigs
{
    private const ushort SecretKey = 0x42;
    private string? _token;
    public uint Id { get; set; }

    public string? SecretToken { get; set; }

    public string Token
    {
        get => _token ??= EncodeDecrypt(SecretToken, SecretKey);
        set => _token = value;
    }

    private static string EncodeDecrypt(string? str, ushort secretKey)
    {
        return str?.ToArray().Aggregate("", (current, c) => current + TopSecret(c, secretKey)) ?? "";
    }

    private static char TopSecret(char character, ushort secretKey) => (char)(character ^ secretKey);
}