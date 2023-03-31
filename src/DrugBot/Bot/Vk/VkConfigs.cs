#nullable enable
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

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

    public static VkConfigs GetConfig(string file = "Local/Config.json")
    {
        string path = Path.Combine(Environment.CurrentDirectory, file);
        string json = File.ReadAllText(path);
        VkConfigs? configs = JsonConvert.DeserializeObject<VkConfigs>(json);

        return configs;
    }

    private static string EncodeDecrypt(string? str, ushort secretKey)
    {
        return str?.ToArray().Aggregate("", (current, c) => current + TopSecret(c, secretKey)) ?? "";
    }

    private static char TopSecret(char character, ushort secretKey) => (char)(character ^ secretKey);
}