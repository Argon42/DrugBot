using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomProcessors.Converters;

internal class ProcessorConverter : JsonConverter
{
    private readonly IServiceProvider _serviceProvider;

    public ProcessorConverter(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public override bool CanConvert(Type objectType) => typeof(CustomProcessor).IsAssignableFrom(objectType);

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        object obj = _serviceProvider.GetRequiredService<CustomProcessor>();
        serializer.Populate(jo.CreateReader(), obj);
        return obj;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        JObject jo = new();
        serializer.Serialize(jo.CreateWriter(), value);
        jo.WriteTo(writer);
    }
}