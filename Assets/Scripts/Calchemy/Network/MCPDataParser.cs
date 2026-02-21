using System;
using System.Collections.Generic;
using UnityEngine;

namespace Calchemy.Network.Data
{
    /// <summary>
    /// LogicForge(서버)에서 전달받는 게임 데이터(GameData)의 JSON 스키마 DTO 집합입니다.
    /// MCP 클라이언트를 거쳐 수신된 JSON 텍스트가 이 클래스들로 파싱됩니다.
    /// </summary>
    [Serializable]
    public class SyncGameDataPayload
    {
        public List<SyncCardData> cards;
        public List<SyncArtifactData> artifacts;
        public List<SyncEntityStat> entities;
    }

    [Serializable]
    public class SyncCardData
    {
        public string id;
        public string name;
        public string elementType; // "FIRE", "WATER", "EARTH", "WIND"
        public int grade;
        public string description;

        // 로직 컴포넌트 트리
        public List<LogicComponentData> components;
    }

    /// <summary>
    /// 카드의 '기능'을 담당하는 컴포넌트 묶음입니다.
    /// 유니티 클라이언트는 이 데이터를 읽어 실제 C# Action으로 런타임 변환합니다.
    /// </summary>
    [Serializable]
    public class LogicComponentData
    {
        public string triggerType;   // 발동 시점 (예: "ON_BINGO", "ON_PLAY", "ON_TURN_START")
        public string conditionType; // 전제 조건 (예: "ALWAYS", "HP_BELOW_50")
        public string effectType;    // 실제 효과 (예: "DEAL_DAMAGE", "HEAL", "ADD_BLOCK")
        public string targetType;    // 대상 지정 (예: "SELF", "ENEMY_ALL", "BINGO_LINE")
        public string formulaType;   // 수치 연산 (예: "FLAT_10", "ATTACK_X_2")
        
        // 추가 속성들 (파라미터 묶음)
        // 유니티 JsonUtility의 한계로 인해 Dictionary 대신 KeyValue 쌍 리스트나 Raw 문자열로 사용할 수 있습니다.
        public string rawParametersJson; 
    }

    [Serializable]
    public class SyncArtifactData
    {
        public string id;
        public string name;
        public string description;
        public List<LogicComponentData> components;
    }

    [Serializable]
    public class SyncEntityStat
    {
        public string id;            // "GOLEM", "MONSTER_1" 등
        public string entityType;    // "PLAYER", "ENEMY_NORMAL", "ENEMY_BOSS"
        public int initialHp;
        public int baseAttack;
        public int baseDefense;
    }

    /// <summary>
    /// MCP 데이터를 파싱하는 유틸리티
    /// </summary>
    public static class MCPDataParser
    {
        public static SyncGameDataPayload ParseGameData(string json)
        {
            try
            {
                return JsonUtility.FromJson<SyncGameDataPayload>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[MCPDataParser] JSON 파싱 실패: {e.Message}\nJSON: {json}");
                return null;
            }
        }
    }
}
