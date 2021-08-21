using System;
using System.IO;
using Newtonsoft.Json;

public class Configs
{
    private Configs()
    {
    }

    public string Token { get; set; }
    public uint Id { get; set; }

    public static Configs GetConfig(string fileName = "Config")
    {
        string path = Path.Combine(Environment.CurrentDirectory, $"Local/{fileName}.json");
        string json = File.ReadAllText(path);
        var configs = JsonConvert.DeserializeObject<Configs>(json);

        return configs;
    }
}