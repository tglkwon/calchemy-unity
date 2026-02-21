using UnityEngine;
using Calchemy.Network;
using Calchemy.Network.Data;
using Calchemy.UI;

namespace Calchemy.Demo
{
    /// <summary>
    /// MCP 클라이언트를 통해 LogicForge(Server)의 초기 게임 데이터를 가져와
    /// UI 스크립트(BattleUIMockup)에 파싱된 스탯을 연결하는 통합 테스트 관리자입니다.
    /// </summary>
    public class BattleDataIntegrator : MonoBehaviour
    {
        [Header("References")]
        public BattleUIMockup targetUI;

        [ContextMenu("Request Start Data from MCP")]
        public void LoadBattleDataFromServer()
        {
            Debug.Log("[BattleDataIntegrator] LogicForge에 초기 전투 데이터 요청 시작...");

            // LogicForge에 노출된 어떤 도구나 리소스를 호출한다고 가정 (예: get_start_stats)
            // 인자로 게임 모드나 플레이어 정보 JSON 객체를 넘길 수 있습니다.
            string argsJson = "{}";

            if (MCPClientManager.Instance != null)
            {
                MCPClientManager.Instance.CallMCPTool("get_battle_init_data", argsJson, OnSuccessDataReceived, OnErrorDataReceived);
            }
            else
            {
                Debug.LogWarning("MCPClientManager 인스턴스가 존재하지 않아 더미 데이터 모델로 진행합니다.");
                SimulateDummyData();
            }
        }

        private void OnSuccessDataReceived(string jsonResponse)
        {
            // 실제 RPC 호출 응답은 JSON-RPC Result 형태로 오므로, result 객체의 raw 덱스트를 파싱한다고 전제합니다.
            Debug.Log($"[BattleDataIntegrator] 응답 수신 성공! JSON길이: {jsonResponse.Length}");
            
            // 데이터 파싱
            SyncGameDataPayload payload = MCPDataParser.ParseGameData(jsonResponse);

            if (payload != null && payload.entities != null)
            {
                ApplyStatsToUI(payload);
            }
        }

        private void OnErrorDataReceived(string errorMsg)
        {
            Debug.LogError($"[BattleDataIntegrator] LogicForge 연동 실패: {errorMsg}");
        }

        /// <summary>
        /// 서버 호출 없이 데이터 파싱 렌더링을 시뮬레이션 합니다.
        /// </summary>
        private void SimulateDummyData()
        {
            string dummyJson = @"
            {
                ""entities"": [
                    { ""id"": ""golem"", ""entityType"": ""PLAYER"", ""initialHp"": 300, ""baseAttack"": 2, ""baseDefense"": 2 },
                    { ""id"": ""monster_1"", ""entityType"": ""ENEMY_NORMAL"", ""initialHp"": 150, ""baseAttack"": 8, ""baseDefense"": 8 }
                ]
            }";

            SyncGameDataPayload payload = MCPDataParser.ParseGameData(dummyJson);
            ApplyStatsToUI(payload);
        }

        private void ApplyStatsToUI(SyncGameDataPayload payload)
        {
            if (targetUI == null) return;

            int playerHp = 100;
            int enemyHp = 100;

            foreach (var entity in payload.entities)
            {
                if (entity.entityType == "PLAYER")
                {
                    playerHp = entity.initialHp;
                }
                else if (entity.entityType.Contains("ENEMY"))
                {
                    enemyHp = entity.initialHp;
                }
            }

            Debug.Log($"[BattleDataIntegrator] 추출된 스탯 - 플레이어 HP:{playerHp}, 몬스터 HP:{enemyHp}");
            
            // UI에 즉시 동기화
            targetUI.SyncState(playerHp, playerHp, enemyHp, enemyHp);
            targetUI.UpdateTurn(1);
        }
    }
}
