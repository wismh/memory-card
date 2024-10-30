using UnityEditor;

namespace Project.Core.CustomCodeGeneratorModule.Editor 
{
    internal static class MenuItems 
    {
        [InitializeOnLoadMethod]
        private static void Init() 
        {
            Menu.SetChecked(MENU_TOGGLE_AUTO_GENERATE, UnityCodeGenSettings.autoGenerateOnCompile);
        }

        [MenuItem(MENU_GENERATE)]
        private static void Generate() 
        {
            ScriptFileGenerator.Generate();
        }

        [MenuItem(MENU_TOGGLE_AUTO_GENERATE)]
        private static void ToggleAutoGenerate() 
        {
            UnityCodeGenSettings.autoGenerateOnCompile = !UnityCodeGenSettings.autoGenerateOnCompile;
            Menu.SetChecked(MENU_TOGGLE_AUTO_GENERATE, UnityCodeGenSettings.autoGenerateOnCompile);
        }

        private const string MENU_GENERATE = "Tools/UnityCodeGen/Generate %G";
        private const string MENU_TOGGLE_AUTO_GENERATE = "Tools/UnityCodeGen/Auto-generate on Compile";
    }
}