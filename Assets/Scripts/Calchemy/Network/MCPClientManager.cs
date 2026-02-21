using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Calchemy.Network
{
    /// <summary>
    /// 로컬 MCP 서버(LogicForge 등)와 통신하여 데이터를 가져오기 위한 클라이언트 매니저입니다.
    /// MCP의 SSE(Server-Sent Events) 또는 HTTP POST 방식을 통해 Tool Call 및 Resource Fetch를 수행합니다.
    /// </summary>
    public class MCPClientManager : MonoBehaviour
    {
        public static MCPClientManager Instance { get; private set; }

        [Header("MCP Connection Settings")]
        public string mcpServerUrl = "http://127.0.0.1:8080";
        public string mcpEndpoint = "/mcp";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // 초기 연결 상태 확인
            StartCoroutine(CheckHealthCoroutine());
        }

        /// <summary>
        /// 서버 헬스 체크
        /// </summary>
        private IEnumerator CheckHealthCoroutine()
        {
            string url = $"{mcpServerUrl}{mcpEndpoint}";
            using (UnityWebRequest request = UnityWebRequest.Head(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"[MCPClientManager] MCP 서버 연결 성공: {url}");
                }
                else
                {
                    Debug.LogWarning($"[MCPClientManager] MCP 서버 연결 실패: {request.error}");
                }
            }
        }

        /// <summary>
        /// 특정 리소스나 도구를 호출하는 범용(더미) 메서드입니다.
        /// 구체적인 Request Schema는 협의된 MCP Protocol 명세에 맞춰 확장합니다.
        /// </summary>
        public void CallMCPTool(string toolName, string argumentsJson, Action<string> onSuccess, Action<string> onError)
        {
            StartCoroutine(CallMCPToolCoroutine(toolName, argumentsJson, onSuccess, onError));
        }

        private IEnumerator CallMCPToolCoroutine(string toolName, string argumentsJson, Action<string> onSuccess, Action<string> onError)
        {
            // TODO: 실제 LogicForge의 엔드포인트에 맞는 JSON RPC 호출 규격으로 수정해야 함
            string requestUrl = $"{mcpServerUrl}/messages"; // 예시 경로

            string payload = $"{{\"jsonrpc\": \"2.0\", \"method\": \"tools/call\", \"params\": {{\"name\": \"{toolName}\", \"arguments\": {argumentsJson}}}, \"id\": 1}}";
            byte[] bodyRaw = Encoding.UTF8.GetBytes(payload);

            using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"[MCPClientManager] Tool '{toolName}' 호출 성공");
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError($"[MCPClientManager] Tool '{toolName}' 호출 실패: {request.error}");
                    onError?.Invoke(request.error);
                }
            }
        }
    }
}
