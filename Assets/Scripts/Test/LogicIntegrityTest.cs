using UnityEngine;
using System.Collections.Generic;
using LogicForge.Schema.V2; // Using the new V2 Strict Schema
using Calchemy.Systems; // For referencing existing systems
using Calchemy.Entities; // For UnitEntity

namespace Tests.Runtime
{
    public class LogicIntegrityTest : MonoBehaviour
    {
        [Header("Existing Systems")]
        public V2LogicAdapter adapter; 
        private UnitEntity dummyTarget; 

        void Start()
        {
            // Setup dummy environment
            if (adapter == null) adapter = gameObject.AddComponent<V2LogicAdapter>();
            dummyTarget = new GameObject("DummyTarget").AddComponent<UnitEntity>();
            dummyTarget.currentHp = 100;
            dummyTarget.maxHp = 100;
            dummyTarget.statusEffects = new List<Calchemy.Entities.StatusEffect>(); // Initialize if not done in Start

            RunStrictSchemaTest();
        }

        void RunStrictSchemaTest()
        {
            Debug.Log("--- 🛠️ TS Schema Data Integrity Test Start ---");

            // 1. Strict JSON 데이터 로드
            TextAsset jsonFile = Resources.Load<TextAsset>("Data/test_cards_ts_schema");
            if (jsonFile == null)
            {
                Debug.LogError("❌ [Step 1 Fail] 가상 데이터 파일(test_cards_ts_schema.json)을 찾을 수 없습니다.");
                return;
            }

            // 2. DTO 파싱 (Schema-to-C#으로 생성된 클래스 사용)
            CardListSchema data = null;
            try
            {
                data = JsonUtility.FromJson<CardListSchema>(jsonFile.text);
                
                if (data != null && data.cards != null)
                {
                    Debug.Log($"✅ [Parsing] JSON 파싱 성공! 포함된 카드 수: {data.cards.Count}");
                }
                else
                {
                    Debug.LogError("❌ [Parsing Fail] 파싱은 되었으나 데이터가 비어있습니다. 필드명을 확인하세요.");
                    return;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ [Parsing Fail] C# 클래스 구조와 JSON이 일치하지 않습니다.\n{e.Message}");
                return;
            }

            // 3. 기존 로직 해석기 검증
            foreach (var card in data.cards)
            {
                VerifyCardLogic(card);
            }

            Debug.Log("--- ✅ Test Complete ---");
        }

        void VerifyCardLogic(CardSchema card)
        {
            Debug.Log($"🔍 Testing Card: {card.name} ({card.id})");

            // [검증 A] 데이터 구조 확인
            if (card.logic == null || card.logic.effects == null)
            {
                Debug.LogError($"❌ [Structure] '{card.name}'의 Logic 또는 Effects 데이터가 누락되었습니다.");
                return;
            }

            Debug.Log($"▷ Trigger Type: {card.logic.trigger?.type ?? "None"}");
            
            // [검증 B] 실행 테스트 (Execution)
            Debug.Log($"   [Execution] Pre-Execution HP: {dummyTarget.currentHp}");
            
            foreach (var effect in card.logic.effects)
            {
                adapter.Execute(effect, dummyTarget);
            }

            Debug.Log($"   [Execution] Post-Execution HP: {dummyTarget.currentHp}");
            foreach(var status in dummyTarget.statusEffects)
            {
                Debug.Log($"   [Execution] Active Status: {status.id} : {status.value}");
            }
        }

        // CheckCompatibility method removed as we are now executing

    }
}
