using System.Collections.Generic;
using Project.Core.CustomCodeGeneratorModule.Editor;

namespace Project.Core.SourceGeneratorModule.Editor 
{
    [Generator]
    public class PrefabNamesGenerator : ICodeGenerator 
    {
        private const string FILE_NAME = "PrefabNames";
        
        public void Execute(GeneratorContext context)
        {
            var classInstance = new ClassInstance()
                .AddNamespace(GeneratorConstants.DefaultNameSpace)
                .SetPublic()
                .SetStatic()
                .SetName(FILE_NAME);

            foreach (string prefabName in Address.Prefabs.AllKeys) 
            {
                classInstance
                    .AddField(new FieldInstance()
                    .SetPublic()
                    .SetConst()
                    .SetStringType()
                    .SetName(prefabName.Replace(" ", string.Empty))
                    .SetAssignedValue($"\"{prefabName}\""));
            }
            
            context.OverrideFolderPath(GeneratorConstants.ContentFilePath);
            context.AddFile(FILE_NAME + GeneratorConstants.GeneratedFileEnding, classInstance.GetString());
        }
    }
    
    public class Address
    {
        public class Prefabs
        {
            public static List<string> AllKeys = new(){};
        }
    }
}