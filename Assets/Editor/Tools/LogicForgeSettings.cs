using UnityEngine;

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

        public string ApiKey => apiKey;

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ApiUrl) && !string.IsNullOrEmpty(apiKey);
        }
    }
}
