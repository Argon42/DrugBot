using System.Reflection;
using CustomProcessors.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomProcessors;

internal class BehaviourConverter : JsonConverter
{
    private readonly IServiceProvider _serviceProvider;

    public BehaviourConverter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private const string TypePropertyName = "Type";

    public override bool CanConvert(Type objectType)
    {
        return typeof(BaseBehaviour).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        string typeName = (string)jo[TypePropertyName]!;
        Type? type = Assembly.GetExecutingAssembly().GetType(typeName);
        if (type == null)
        {
            throw new Exception($"Unknown type: {typeName}");
        }

        object obj = _serviceProvider.GetRequiredService(type);
        serializer.Populate(jo.CreateReader(), obj);
        return obj;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        JObject jo = new JObject();
        jo.Add(TypePropertyName, value?.GetType().Name);
        serializer.Serialize(jo.CreateWriter(), value);
        jo.WriteTo(writer);
    }
}