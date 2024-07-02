using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomProcessors.Converters;

internal class ObjectWithTypeConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType
        .CustomAttributes
        .Any(data =>
            data.AttributeType == typeof(CustomSerializationAttribute));

    private const string TypePropertyName = "Type";
    private readonly IServiceProvider _serviceProvider;

    public ObjectWithTypeConverter(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        string typeName = (string)jo[TypePropertyName]!;
        Type? type = AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(assembly => assembly.GetType(typeName))
            .FirstOrDefault(type => type != null);
        if (type == null) throw new Exception($"Unknown type: {typeName}");

        if (_serviceProvider.GetServices(type).Any())
        {
            object obj = _serviceProvider.GetRequiredService(type);
            serializer.Populate(jo.CreateReader(), obj);
            return obj;
        }

        var constructorInfo = type.GetConstructors().First();
        var args = constructorInfo.GetParameters();
        if (args.Length == 0)
        {
            var obj = Activator.CreateInstance(type) ?? throw new InvalidOperationException();
            serializer.Populate(jo.CreateReader(), obj);
            return obj;
        }

        var argsFromProvider = args.Select(info => _serviceProvider.GetRequiredService(info.ParameterType));
        var instance = Activator.CreateInstance(type, argsFromProvider)
                       ?? throw new InvalidOperationException();

        serializer.Populate(jo.CreateReader(), instance);
        return instance;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        JObject jo = new();
        jo.Add(TypePropertyName, value?.GetType().Name);
        serializer.Serialize(jo.CreateWriter(), value);
        jo.WriteTo(writer);
    }
}