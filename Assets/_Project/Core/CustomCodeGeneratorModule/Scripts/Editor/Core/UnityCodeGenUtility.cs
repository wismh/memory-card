namespace Project.Core.CustomCodeGeneratorModule.Editor 
{
    public static class UnityCodeGenUtility
    {
        public static void Generate() 
        {
            ScriptFileGenerator.Generate();
        }

        public const string defaultFolderPath = "Assets/UnityCodeGen.Generated";
    }
}