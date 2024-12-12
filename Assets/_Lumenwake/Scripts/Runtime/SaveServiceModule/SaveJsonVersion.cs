using Newtonsoft.Json.Linq;

namespace Project.Core.SaveServiceModule
{
    public static class SaveJsonVersion
    {
        public const string PascalKey = "Version";
        public const string CamelKey = "version";

        public static int Read(JObject root)
        {
            return root.Value<int?>(CamelKey) ?? root.Value<int?>(PascalKey) ?? 0;
        }

        public static void Write(JObject root, int version)
        {
            if (root.ContainsKey(PascalKey))
            {
                root[PascalKey] = version;
                return;
            }

            if (root.ContainsKey(CamelKey))
            {
                root[CamelKey] = version;
                return;
            }

            root[PascalKey] = version;
        }
    }
}
