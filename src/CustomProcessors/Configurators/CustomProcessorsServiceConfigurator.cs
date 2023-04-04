using CustomProcessors.Behaviours.Response;
using CustomProcessors.Behaviours.Triggers;
using DrugBot.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CustomProcessors;

public static class CustomProcessorsServiceConfigurator
{
    private const string CustomProcessorsPath = "CustomProcessorsPath";

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<CustomProcessor>();
        BehaviourConfigurator.ConfigureBehaviours(services);
        IEnumerable<string> files = GetFiles(configuration);
        ConfigureFiles(services, files);
    }

    private static void ConfigureFiles(IServiceCollection services, IEnumerable<string> files)
    {
        foreach (string file in files)
        {
            string json = File.ReadAllText(file);
            services.AddScoped<IProcessor>(serviceProvider => CreateProcessor(json, serviceProvider));
        }
    }

    private static IProcessor CreateProcessor(string json, IServiceProvider serviceProvider)
    {
        JsonConverter behaviourConverter = new BehaviourConverter(serviceProvider);
        JsonConverter processorConverter = new ProcessorConverter(serviceProvider);
        return JsonConvert.DeserializeObject<CustomProcessor>(json, behaviourConverter, processorConverter) ??
               throw new InvalidOperationException($"Json can't deserialize to CustomProcessor\n{json}");
    }

    private static IEnumerable<string> GetFiles(IConfiguration configuration)
    {
        string? path = configuration[CustomProcessorsPath];
        if (path != default && !Path.IsPathRooted(path))
        {
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path));
        }

        return path == default ? Array.Empty<string>() : Directory.GetFiles(path);
    }
}