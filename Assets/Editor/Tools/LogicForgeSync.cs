using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;

namespace LogicForge.Editor
{
    public class LogicForgeSync
    {
        private const string SAVE_PATH = "Assets/Resources/Data/Raw/cards.json";

        [MenuItem("Tools/LogicForge/Sync Cards", false, 1)]
        public static async void SyncCards()
        {
            // 1. Load Settings
            var settings = LoadSettings();
            if (settings == null || !settings.IsValid())
            {
                if (EditorUtility.DisplayDialog("LogicForge Settings Missing", 
                    "Could not find valid LogicForgeSettings. Would you like to create one?", "Create", "Cancel"))
                {
                    CreateSettingsAsset();
                }
                return;
            }

            string url = $"{settings.ApiUrl}/api/v1/sync/cards";
            Debug.Log($"[LogicForge] Syncing cards from: {url}");

            // 2. Request API
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                if (!string.IsNullOrEmpty(settings.ApiKey))
                {
                    request.SetRequestHeader("Authorization", $"Bearer {settings.ApiKey}");
                }

                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"[LogicForge] Sync Failed: {request.error}\n{request.downloadHandler.text}");
                    EditorUtility.DisplayDialog("Sync Failed", $"Error: {request.error}", "OK");
                    return;
                }

                // 3. Save File
                string json = request.downloadHandler.text;
                SaveToFile(json);
            }
        }

        private static void SaveToFile(string json)
        {
            try
            {
                string fullPath = Path.Combine(Application.dataPath, "Resources/Data/Raw");
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                string filePath = Path.Combine(fullPath, "cards.json");
                File.WriteAllText(filePath, json);

                AssetDatabase.Refresh();
                Debug.Log($"[LogicForge] Successfully synced cards to: {SAVE_PATH}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[LogicForge] File Write Error: {e.Message}");
            }
        }

        public static LogicForgeSettings LoadSettings()
        {
            string[] guids = AssetDatabase.FindAssets("t:LogicForgeSettings");
            if (guids.Length == 0) return null;

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<LogicForgeSettings>(path);
        }

        private static void CreateSettingsAsset()
        {
            LogicForgeSettings asset = ScriptableObject.CreateInstance<LogicForgeSettings>();

            string path = "Assets/Editor/Tools";
            if (!AssetDatabase.IsValidFolder("Assets/Editor")) AssetDatabase.CreateFolder("Assets", "Editor");
            if (!AssetDatabase.IsValidFolder("Assets/Editor/Tools")) AssetDatabase.CreateFolder("Assets/Editor", "Tools");

            string assetPath = Path.Combine(path, "LogicForgeSettings.asset");
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            Debug.Log($"[LogicForge] Created settings at {assetPath}. Please configure API Key.");
        }
    }
}
