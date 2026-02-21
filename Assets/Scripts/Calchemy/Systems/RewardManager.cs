using UnityEngine;

namespace Calchemy.Systems
{
    /// <summary>
    /// 전리품 팝업 띄우기, 상점 로직 연산, 골드(재화) 관리 매니저입니다.
    /// </summary>
    public class RewardManager : MonoBehaviour
    {
        public static RewardManager Instance { get; private set; }

        public int CurrentGold { get; private set; } = 1000;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ShowVictoryRewards()
        {
            Debug.Log("[RewardManager] 전투 승리 보상(골드, 카드 선택 팩, 유물 등) UI 생성 팝업 요청 (뼈대)");
        }

        public bool PurchaseItem(int cost)
        {
            if (CurrentGold >= cost)
            {
                CurrentGold -= cost;
                Debug.Log($"[RewardManager] 상점 아이템 구매 성공. (차감: {cost}, 잔여: {CurrentGold})");
                return true;
            }
            Debug.LogWarning("[RewardManager] 골드가 부족하여 아이템을 살 수 없습니다.");
            return false;
        }

        public void AddGold(int amount)
        {
            CurrentGold += amount;
            Debug.Log($"[RewardManager] 전투 골드 획득: +{amount} / 총 보유: {CurrentGold}");
        }
    }
}
