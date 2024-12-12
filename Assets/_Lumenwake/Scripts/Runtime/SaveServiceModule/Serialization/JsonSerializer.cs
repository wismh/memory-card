using Newtonsoft.Json;

namespace Project.Core.SaveServiceModule.Serialization
{
    public sealed class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializer(bool useCamelCase = false, bool prettyPrintInEditor = true)
        {
            var formatting = Formatting.None;
#if UNITY_EDITOR
            if (prettyPrintInEditor)
            {
                formatting = Formatting.Indented;
            }
#endif

            _settings = new JsonSerializerSettings
            {
                Formatting = formatting,
                NullValueHandling = NullValueHandling.Ignore,
            };

            if (useCamelCase)
            {
                _settings.ContractResolver = new CamelCaseExceptDictionaryKeysContractResolver();
            }
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}
