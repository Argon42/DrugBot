using DrugBot.Bot.Vk;
using DrugBot.Common;
using Microsoft.Extensions.Configuration;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace DrugBot;

public abstract class VkFactory
{
    public class Api : IFactory<VkApi>
    {
        private readonly VkConfigs _config;

        public Api(VkConfigs config)
        {
            _config = config;
        }

        public VkApi Create()
        {
            var api = new VkApi();
            api.Authorize(new ApiAuthParams { AccessToken = _config.Token });
            return api;
        }
    }

    public class LongPollServer : IFactory<LongPollServerResponse>
    {
        private readonly VkConfigs _config;
        private readonly IFactory<IVkApi> _vkApiFactory;

        public LongPollServer(VkConfigs config, IFactory<IVkApi> vkApiFactory)
        {
            _config = config;
            _vkApiFactory = vkApiFactory;
        }

        public LongPollServerResponse Create()
        {
            return _vkApiFactory.Create().Groups.GetLongPollServer(_config.Id);
        }
    }

    public class Config : IFactory<VkConfigs>
    {
        private readonly IConfiguration _configuration;
        private const string AppId = "VK_APP_ID";
        private const string GroupToken = "VK_GROUP_TOKEN";

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public VkConfigs Create()
        {
            string? token = _configuration[GroupToken];
            string? id = _configuration[AppId];

            if (token == null || id == null)
                throw new Exception($"Environment variables {AppId} and/or {GroupToken} not exist");

            if (uint.TryParse(id, out uint parsedId) == false)
                throw new Exception($"Environment variable {AppId} is incorrect");

            return new VkConfigs() { Token = token ?? "", Id = parsedId };
        }
    }
}