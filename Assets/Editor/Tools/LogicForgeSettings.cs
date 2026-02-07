using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

namespace LogicForge.Editor
{
    [CreateAssetMenu(fileName = "LogicForgeSettings", menuName = "LogicForge/Settings")]
    public class LogicForgeSettings : ScriptableObject
    {
        [Header("API Configuration")]
        [Tooltip("LogicForge API Base URL (e.g., http://localhost:3000)")]
        public string ApiUrl = "http://localhost:3000";

        [Tooltip("Admin API Key for authentication")]
        [SerializeField] private string apiKey;

        public string ApiKey
        {
            get
            {
                if (!string.IsNullOrEmpty(apiKey)) return apiKey;
                return LoadApiKeyFromSecrets();
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ApiUrl) && !string.IsNullOrEmpty(ApiKey);
        }

        private string LoadApiKeyFromSecrets()
        {
            // Try to find secrets.json in the project root (parent of Assets)
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string secretsPath = Path.Combine(projectRoot, "secrets.json");

            if (File.Exists(secretsPath))
            {
                try
                {
                    string json = File.ReadAllText(secretsPath);
                    // Simple regex to extract "logicforge_api_key" without full JSON parsing overhead if desired,
                    // or just use JsonUtility wrapper. Let's use simple regex for a single field or JsonUtility wrapper.
                    // Assuming secrets.json structure: { "logicforge_api_key": "YOUR_KEY" }
                    
                    // Using a lightweight wrapper for JsonUtility
                    var secrets = JsonUtility.FromJson<Secrets>(json);
                    if (secrets != null && !string.IsNullOrEmpty(secrets.logicforge_api_key))
                    {
                        return secrets.logicforge_api_key;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[LogicForge] Failed to read secrets.json: {e.Message}");
                }
            }
            return string.Empty;
        }

        [System.Serializable]
        private class Secrets
        {
            public string logicforge_api_key;
        }
    }
}
