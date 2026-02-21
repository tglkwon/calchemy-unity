using System;
using System.Collections.Generic;
using UnityEngine;
using Calchemy.Network.Data;

namespace Calchemy.Core
{
    /// <summary>
    /// LogicForge에서 전달받은 Card Component 데이터 트리를 
    /// Unity 네이티브 C# 런타임 Action (Delegate) 및 State 체계로 변환하는 핵심 파서입니다.
    /// </summary>
    public class ComponentParser
    {
        // 런타임 실행 구조체 정의
        public class ParsedComponent
        {
            public string Trigger;
            public Func<bool> ConditionFunc; // 조건 판별식
            public Action<object> ExecuteAction; // 실제 발동 액션
        }

        /// <summary>
        /// 수신된 카드의 컴포넌트 데이터 목록을 실행 가능한 런타임 객체로 번역합니다.
        /// </summary>
        public static List<ParsedComponent> ParseCardComponents(List<LogicComponentData> rawComponents)
        {
            var resultList = new List<ParsedComponent>();
            if (rawComponents == null) return resultList;

            foreach (var compData in rawComponents)
            {
                var parsed = new ParsedComponent
                {
                    Trigger = compData.triggerType,
                    ConditionFunc = BuildConditionFunc(compData.conditionType),
                    ExecuteAction = BuildEffectAction(compData)
                };
                resultList.Add(parsed);
            }

            return resultList;
        }

        // --- 구문 해석 로직들 (Strategy 패턴으로 확장 가능) ---

        private static Func<bool> BuildConditionFunc(string conditionType)
        {
            return conditionType switch
            {
                "ALWAYS" => () => true,
                "HP_BELOW_50" => () => CheckHpCondition(0.5f),
                // TODO: 추가 조건 구현
                _ => () => true
            };
        }

        private static Action<object> BuildEffectAction(LogicComponentData comp)
        {
            return (ctx) =>
            {
                // TODO: Target 타입에 맞춰 누구에게 적용할지 지정하는 타겟팅 로직 호출
                // TODO: Formula 타입에 맞춰 수치 계산
                int calculatedValue = CalculateFormula(comp.formulaType);

                switch (comp.effectType)
                {
                    case "DEAL_DAMAGE":
                        Debug.Log($"[ComponentParser] Effect 발동: 데미지 {calculatedValue} 가해짐!");
                        // BattleManager.Instance.DealDamageToTarget(comp.targetType, calculatedValue);
                        break;
                    case "HEAL":
                        Debug.Log($"[ComponentParser] Effect 발동: 체력 {calculatedValue} 회복!");
                        // BattleManager.Instance.HealTarget(comp.targetType, calculatedValue);
                        break;
                    case "ADD_BLOCK":
                        Debug.Log($"[ComponentParser] Effect 발동: 방어도 {calculatedValue} 증가!");
                        break;
                    default:
                        Debug.LogWarning($"[ComponentParser] 알 수 없는 이펙트 타입: {comp.effectType}");
                        break;
                }
            };
        }

        private static int CalculateFormula(string formulaType)
        {
            if (string.IsNullOrEmpty(formulaType)) return 0;
            
            // 예시: "FLAT_10" -> 10, "ATTACK_RATE_200" -> 기초공격력 * 2
            if (formulaType.StartsWith("FLAT_"))
            {
                if (int.TryParse(formulaType.Replace("FLAT_", ""), out int val))
                    return val;
            }
            // 확장 공식 필요 시 여기에 작성
            return 10; // 기본값
        }

        private static bool CheckHpCondition(float ratio)
        {
            // TODO: 실제 유닛 스탯 참조
            return true;
        }
    }
}
