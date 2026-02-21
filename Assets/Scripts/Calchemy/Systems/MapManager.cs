using UnityEngine;
using System.Collections.Generic;

namespace Calchemy.Systems
{
    /// <summary>
    /// Slay the Spire 스타일의 노드 맵 렌더링 및 진행도/세이브 관리 매니저입니다.
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void GenerateNewMap()
        {
            Debug.Log("[MapManager] 새로운 맵 노드 생성 알고리즘 실행 (뼈대)");
            // TODO: MapGenerator 로직 기반의 랜덤 방 생성 알고리즘 이식
        }

        public void EnterNode(int nodeId)
        {
            Debug.Log($"[MapManager] 노드 {nodeId} 입장 처리");
            // TODO: 전투방(Battle), 엘리트방(Elite), 휴식처(Rest), 상점(Shop) 등 분기 이동 로직 연결
        }
    }
}
