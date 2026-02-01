using UnityEngine;
using Calchemy.Data;
using System.Collections.Generic;

namespace Calchemy.Entities
{
    /// <summary>
    /// 그리드에 최적화된 유닛 엔티티
    /// </summary>
    public class UnitEntity : MonoBehaviour
    {
        public CardData cardData;
        public int currentHp;
        public int maxHp;

        public List<StatusEffect> statusEffects = new List<StatusEffect>();
        
        // 위치 정보 (그리드 좌표)
        public int gridX;
        public int gridY;

        public void Initialize(CardData data, int x, int y)
        {
            cardData = data;
            gridX = x;
            gridY = y;
            // 초기 체력 설정 등 (CardData에 기본 체력이 있다면 활용)
        }

        public void TakeDamage(int damage)
        {
            currentHp -= damage;
            if (currentHp <= 0) Die();
        }

        private void Die()
        {
            // 사망 처리 로직
            Debug.Log($"{cardData.cardName}이(가) 파괴되었습니다.");
        }

        public void AddStatus(string id, int value, int duration)
        {
            // Simple logic: Find existing and stack, or add new
            var existing = statusEffects.Find(s => s.id == id);
            if (existing != null)
            {
                existing.value += value;
                existing.duration = UnityEngine.Mathf.Max(existing.duration, duration); // Refresh duration
                Debug.Log($"[UnitEntity] Status Stacked: {id} (Val: {existing.value}, Dur: {existing.duration})");
            }
            else
            {
                statusEffects.Add(new StatusEffect(id, value, duration));
                Debug.Log($"[UnitEntity] Status Added: {id} (Val: {value}, Dur: {duration})");
            }
        }
    }
}
