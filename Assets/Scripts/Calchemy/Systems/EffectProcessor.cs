using UnityEngine;
using Calchemy.Core;
using Calchemy.Entities;

namespace Calchemy.Systems
{
    /// <summary>
    /// YAML 명령(string)을 분석하여 엔티티에 영향력을 미치는 처리기
    /// </summary>
    public class EffectProcessor : MonoBehaviour
    {
        /// <summary>
        /// EffectLogic 데이터를 기반으로 실제 행동을 수행
        /// </summary>
        public void Process(EffectLogic logic, UnitEntity target)
        {
            if (logic == null || string.IsNullOrEmpty(logic.action)) return;

            Debug.Log($"[EffectProcessor] 실행 액션: {logic.action} -> 대상: {target.cardData.cardName}");

            switch (logic.action.ToUpper())
            {
                case "DAMAGE":
                    float dmg = GetParam(logic, "amount");
                    target.TakeDamage((int)dmg);
                    break;
                case "BUFF_HP":
                    float amount = GetParam(logic, "amount");
                    target.currentHp += (int)amount;
                    break;
                // 추가 YAML 액션 핸들러 구현
                default:
                    Debug.LogWarning($"정의되지 않은 액션 프로토콜: {logic.action}");
                    break;
            }
        }

        private float GetParam(EffectLogic logic, string key)
        {
            var param = logic.parameters.Find(p => p.key == key);
            return param != null ? param.value : 0f;
        }
    }
}
