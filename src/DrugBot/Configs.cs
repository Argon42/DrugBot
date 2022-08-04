#nullable enable
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DrugBot
{
    [Serializable]
    public class Configs
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

        public Configs()
        {
        }

        public static Configs GetConfig(string file = "Local/Config.json")
        {
            var path = Path.Combine(Environment.CurrentDirectory, file);
            var json = File.ReadAllText(path);
            var configs = JsonConvert.DeserializeObject<Configs>(json);

            return configs;
        }

        private static string EncodeDecrypt(string? str, ushort secretKey)
        {
            return str?.ToArray().Aggregate("", (current, c) => current + TopSecret(c, secretKey)) ?? "";
        }

        private static char TopSecret(char character, ushort secretKey)
        {
            return (char)(character ^ secretKey);
        }
    }
}