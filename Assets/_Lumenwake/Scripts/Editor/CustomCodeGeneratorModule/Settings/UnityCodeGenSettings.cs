using UnityEditor;

namespace Project.Core.CustomCodeGeneratorModule.Editor
{
    public static class UnityCodeGenSettings
    {
        public static bool autoGenerateOnCompile 
        {
            get
            {
                return bool.TryParse(
                    EditorUserSettings.GetConfigValue(KEY_GENERATE_ON_COMPILE), 
                    out bool result
                ) && result;
            }
            set =>
                EditorUserSettings.SetConfigValue(KEY_GENERATE_ON_COMPILE, value.ToString());
        }
        
        private const string KEY_GENERATE_ON_COMPILE = "UnityCodeGen-AutoGenerateOnCompile";
    }
}