using System.Text;
using DrugBot.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CustomProcessors;

public class CustomProcessorsServiceConfigurator
{
    private const string CustomProcessorsPath = "CustomProcessorsPath";

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, ILogger<CustomProcessorsServiceConfigurator> logger)
    {
        services.AddTransient<CustomProcessor>();
        BehaviourConfigurator.ConfigureBehaviours(services);
        string[] files = GetFiles(configuration, logger);
        ConfigureFiles(services, files, logger);
    }

    private static void ConfigureFiles(IServiceCollection services, string[] files,
        ILogger<CustomProcessorsServiceConfigurator> logger)
    {
        logger.LogInformation("Start loading custom processors, files founded: {Count}", files.Length);
        foreach (string file in files)
        {
            string json = File.ReadAllText(file);
            try
            {
                logger.LogDebug("Loading custom processor from {File}, json:{Json}", file, json);
                services.AddSingleton<IProcessor>(serviceProvider => CreateProcessor(json, serviceProvider));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Loading custom processors from files");
            }
        }
        logger.LogInformation("End loading custom processors");
    }

    private static IProcessor CreateProcessor(string json, IServiceProvider serviceProvider)
    {
        JsonConverter behaviourConverter = new BehaviourConverter(serviceProvider);
        JsonConverter processorConverter = new ProcessorConverter(serviceProvider);
        return JsonConvert.DeserializeObject<CustomProcessor>(json, behaviourConverter, processorConverter) ??
               throw new InvalidOperationException($"Json can't deserialize to CustomProcessor\n{json}");
    }

    private static string[] GetFiles(IConfiguration configuration, ILogger<CustomProcessorsServiceConfigurator> logger)
    {
        string? path = configuration[CustomProcessorsPath];
        logger.LogDebug("Path from configuration: {Path}", path);
        if (path != default && !Path.IsPathRooted(path))
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path));
        logger.LogDebug("Full path from configuration: {Path}", path);

        if (path == default)
            return Array.Empty<string>();
        
        if(Directory.Exists(path))
            return Directory.GetFiles(path);

        logger.LogDebug("Dictionary in path not exist");
        return Array.Empty<string>();
    }
}