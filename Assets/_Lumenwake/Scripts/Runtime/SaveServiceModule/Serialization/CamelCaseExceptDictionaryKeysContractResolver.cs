namespace Project.Core.SaveServiceModule.Serialization
{
    /// <summary>CamelCase for JSON properties; dictionary keys keep their original scene names (e.g. Level1).</summary>
    internal sealed class CamelCaseExceptDictionaryKeysContractResolver
        : Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver
    {
        protected override string ResolveDictionaryKey(string dictionaryKey) =>
            dictionaryKey;
    }
}
