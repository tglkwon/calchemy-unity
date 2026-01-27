using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;

namespace LogicForge.Editor
{
    public class LogicForgeSchemaGenerator
    {
        private const string SCHEMA_SAVE_PATH = "Assets/Shared/Schema/SharedSchema.cs";

        [MenuItem("Tools/LogicForge/Sync Schema", false, 0)]
        public static async void SyncSchema()
        {
            var settings = LogicForgeSync.LoadSettings();
            if (settings == null || !settings.IsValid())
            {
                EditorUtility.DisplayDialog("LogicForge Settings Missing", "Please configure settings first.", "OK");
                return;
            }

            // Assuming the API provides the generated C# code directly or we generate it from TS.
            // For Step 2, if the user says "Converted from TS", we might fetch the C# result from the server
            // OR fetch TS and Convert.
            // Let's assume an endpoint exists for the Schema content.
            
            string url = $"{settings.ApiUrl}/api/v1/sync/schema/csharp"; // Hypothetical endpoint
            Debug.Log($"[LogicForge] Requesting Schema from: {url}");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                if (!string.IsNullOrEmpty(settings.ApiKey))
                {
                    request.SetRequestHeader("Authorization", $"Bearer {settings.ApiKey}");
                }

                var operation = request.SendWebRequest();
                while (!operation.isDone) await Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string content = request.downloadHandler.text;
                    GenerateFile(content);
                }
                else
                {
                    Debug.LogWarning($"[LogicForge] Failed to fetch schema from API. ({request.error}). Using local fallback or checking TS.");
                    // Fallback logic could go here if we want to read a local TS file and parse it.
                }
            }
        }

        private static void GenerateFile(string content)
        {
            try
            {
                string fullPath = Path.Combine(Application.dataPath, "Shared/Schema");
                if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

                string filePath = Path.Combine(fullPath, "SharedSchema.cs");
                
                // Write the content. 
                // Note: The content received MUST be the valid C# code.
                File.WriteAllText(filePath, content);
                
                AssetDatabase.Refresh();
                Debug.Log($"[LogicForge] Schema generated at: {filePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[LogicForge] Schema Generation Error: {e.Message}");
            }
        }
    }
}
