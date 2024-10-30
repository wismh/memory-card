using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

using Project.Core.CustomCodeGeneratorModule.Editor;

namespace Project.Core.SourceGeneratorModule.Editor
{
    [Generator]
    public class AddressableConstantsGenerator : ICodeGenerator 
    {
        private const string FILE_NAME = "Address";
        private const string BUILT_IN_DATA_GROUP_NAME = "BuiltInData";
        private const string DUPLICATE_ASSET_GROUP_NAME = "DuplicateAssetIsolation";
        private const string ALL_KEYS_FIELD_NAME = "AllKeys";
        private const string ALL_SCENE_KEYS_FIELD_NAME = "AllSceneKeys";

        public void Execute(GeneratorContext context)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            
            var mainClassInstance = new ClassInstance()
                .AddUsing("System.Collections.Generic")
                .AddNamespace(GeneratorConstants.DefaultNameSpace)
                .SetPublic()
                .SetStatic()
                .SetName(FILE_NAME);

            var allKeysFieldInstance = new FieldInstance().SetPublic()
                .SetStatic()
                .SetListType("string")
                .SetName(ALL_KEYS_FIELD_NAME);
            
            var allSceneKeysFieldInstance = new FieldInstance().SetPublic()
                .SetStatic()
                .SetListType("string")
                .SetName(ALL_SCENE_KEYS_FIELD_NAME);

            var mainClassAllKeys = new List<string>();
            var allScenesKeys = new List<string>();

            foreach (var addressableAssetGroup in settings.groups) 
            {
                string groupName = GetGroupName(addressableAssetGroup);
                
                if (GroupIsSuitableForGenerating(groupName) is false)
                    return;
                
                var groupClassAllKeys = new List<string>();
                
                var addressableClass = new ClassInstance()
                    .SetPublic()
                    .SetStatic()
                    .SetName(groupName);

                foreach (var addressableAssetEntry in addressableAssetGroup.entries.Where(addressableAssetEntry => !addressableAssetEntry.IsSubAsset))
                {
                    string assetKey = GetAssetName(addressableAssetEntry);
                    
                    mainClassAllKeys.Add(assetKey);
                    groupClassAllKeys.Add(assetKey);
                    
                    if (addressableAssetEntry.IsScene)
                        allScenesKeys.Add(assetKey);
                    
                    addressableClass.AddField(new FieldInstance()
                        .SetPublic()
                        .SetConst()
                        .SetStringType()
                        .SetName(assetKey)
                        .SetAssignedValue(@$"""{addressableAssetEntry.address}""")
                    );
                }

                mainClassInstance.AddClass(addressableClass);
                addressableClass.AddField(allKeysFieldInstance.Copy()
                    .SetAssignedValue(GetAssignedValueForStringList(groupClassAllKeys, 3)));
            }
            
            mainClassInstance.AddField(allKeysFieldInstance.Copy()
                .SetAssignedValue(GetAssignedValueForStringList(mainClassAllKeys, 2)));
            
            mainClassInstance.AddField(allSceneKeysFieldInstance.Copy()
                .SetAssignedValue(GetAssignedValueForStringList(allScenesKeys, 2)));
            
            context.OverrideFolderPath(GeneratorConstants.ContentFilePath);
            context.AddFile(FILE_NAME + GeneratorConstants.GeneratedFileEnding, mainClassInstance.GetString());
        }

        private string GetAssignedValueForStringList(IEnumerable<string> keys, int indentCount)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("new() {");
            
            foreach (string key in keys)
                stringBuilder.AppendLine(IndentGenerator.GetIndent(indentCount + 1) + @$"""{key}"",");

            stringBuilder.Append(IndentGenerator.GetIndent(indentCount) + "}");

            return stringBuilder.ToString();
        }

        private static string GetGroupName(AddressableAssetGroup addressableAssetGroup) =>
            addressableAssetGroup.Name.Replace(" ", string.Empty).Replace("-", string.Empty);

        private static string GetAssetName(AddressableAssetEntry addressableAssetEntry) 
        {
            var assetName = addressableAssetEntry.address.Replace(" ", string.Empty)
                .Replace("-", string.Empty)
                .Replace(@"(", "_")
                .Replace(@")", string.Empty);

            if (assetName.Contains("/") || assetName.Contains(@"\"))
                assetName = assetName[(assetName.LastIndexOf("/", StringComparison.Ordinal) + 1)..];

            if (assetName.Contains(@"\"))
                assetName = assetName[(assetName.LastIndexOf(@"\", StringComparison.Ordinal) + 1)..];

            if (assetName.Contains("."))
                assetName = assetName[..assetName.LastIndexOf(@".", StringComparison.Ordinal)];

            return assetName;
        }

        private static bool GroupIsSuitableForGenerating(string groupName) {
            if (string.Equals(groupName, BUILT_IN_DATA_GROUP_NAME, StringComparison.Ordinal))
                return false;
            
            if (string.Equals(groupName, DUPLICATE_ASSET_GROUP_NAME, StringComparison.Ordinal))
                return false;
            
            return !groupName.Any(s => char.IsSymbol(s) || char.IsWhiteSpace(s) || char.IsPunctuation(s));
        }
    }
}