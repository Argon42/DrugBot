using System;
using System.IO;
using Newtonsoft.Json;

namespace DrugBot
{
    public class Configs
    {
        public uint Id { get; set; }

        public string Token { get; set; }

        private Configs()
        {
        }

        public static Configs GetConfig(string fileName = "Config")
        {
            var path = Path.Combine(Environment.CurrentDirectory, $"Local/{fileName}.json");
            var json = File.ReadAllText(path);
            var configs = JsonConvert.DeserializeObject<Configs>(json);

            return configs;
        }
    }
}