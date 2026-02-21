using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 관련 컴포넌트 사용 가정
using System.Collections;

namespace Calchemy.UI
{
    /// <summary>
    /// MCP 통신 및 실차 테스트를 위한 전투 UI 모형(Mockup) 스크립트입니다.
    /// 체력 증감, 슬라이더 애니메이션, 이벤트 바인딩 등을 제공합니다.
    /// </summary>
    public class BattleUIMockup : MonoBehaviour
    {
        [Header("Player Elements")]
        public Image playerImage;
        public Slider playerHpSlider;
        public TextMeshProUGUI playerHpText;
        private int playerMaxHp = 100;
        private int currentPlayerHp;

        [Header("Enemy Elements")]
        public Image enemyImage;
        public Slider enemyHpSlider;
        public TextMeshProUGUI enemyHpText;
        private int enemyMaxHp = 250;
        private int currentEnemyHp;

        [Header("Controls (Optional)")]
        public Button damagePlayerButton;
        public Button damageEnemyButton;
        public Button resetBattleButton;

        private void Start()
        {
            InitializeBattle();

            // 인스펙터에서 버튼을 연결해두면 클릭 시 동작 (에디터 내 테스트용)
            if (damagePlayerButton != null)
                damagePlayerButton.onClick.AddListener(() => DamageTrigger(true, Random.Range(10, 25)));
            
            if (damageEnemyButton != null)
                damageEnemyButton.onClick.AddListener(() => DamageTrigger(false, Random.Range(20, 50)));

            if (resetBattleButton != null)
                resetBattleButton.onClick.AddListener(() => InitializeBattle());
        }

        /// <summary>
        /// 초기 체력 설정 및 UI 갱신 (MCP 연동 시 외부에서 호출 가능)
        /// </summary>
        public void InitializeBattle(int pMaxHp = 100, int eMaxHp = 250)
        {
            playerMaxHp = pMaxHp;
            currentPlayerHp = playerMaxHp;
            
            enemyMaxHp = eMaxHp;
            currentEnemyHp = enemyMaxHp;

            if (playerHpSlider != null) playerHpSlider.value = 1f;
            if (enemyHpSlider != null) enemyHpSlider.value = 1f;

            UpdateTextUI();
        }

        /// <summary>
        /// 캐릭터 데미지 처리 및 슬라이더 보간 애니메이션 시작
        /// </summary>
        public void DamageTrigger(bool isPlayerDamage, int damageAmount)
        {
            if (isPlayerDamage)
            {
                currentPlayerHp = Mathf.Max(0, currentPlayerHp - damageAmount);
                StartCoroutine(AnimateSlider(playerHpSlider, currentPlayerHp, playerMaxHp));
                // TODO: 플레이어 피격 파티클 또는 흔들림 연출
            }
            else
            {
                currentEnemyHp = Mathf.Max(0, currentEnemyHp - damageAmount);
                StartCoroutine(AnimateSlider(enemyHpSlider, currentEnemyHp, enemyMaxHp));
                // TODO: 적 피격 파티클 또는 흔들림 연출
            }
            
            UpdateTextUI();
        }

        private void UpdateTextUI()
        {
            if (playerHpText != null) 
                playerHpText.text = $"{currentPlayerHp} / {playerMaxHp}";
                
            if (enemyHpText != null) 
                enemyHpText.text = $"{currentEnemyHp} / {enemyMaxHp}";
        }

        /// <summary>
        /// 슬라이더 값이 자연스럽게 줄어들도록 하는 코루틴
        /// </summary>
        private IEnumerator AnimateSlider(Slider slider, int currentHp, int maxHp)
        {
            if (slider == null) yield break;

            float targetValue = (float)currentHp / maxHp;
            float startValue = slider.value;
            float duration = 0.3f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                slider.value = Mathf.Lerp(startValue, targetValue, elapsed / duration);
                yield return null;
            }

            slider.value = targetValue;
        }

        // --- MCP or External Logic Integration Hooks ---

        /// <summary>
        /// 외부(MCP)에서 전달된 캐릭터 상태 JSON 또는 객체를 통해 UI를 동기화합니다.
        /// </summary>
        public void SyncState(int pCurrentHp, int pMaxHp, int eCurrentHp, int eMaxHp)
        {
            playerMaxHp = pMaxHp;
            enemyMaxHp = eMaxHp;

            // 슬라이더 애니메이션 없이 즉각 반영이 필요한 경우
            currentPlayerHp = pCurrentHp;
            currentEnemyHp = eCurrentHp;

            if (playerHpSlider != null) playerHpSlider.value = (float)currentPlayerHp / playerMaxHp;
            if (enemyHpSlider != null) enemyHpSlider.value = (float)currentEnemyHp / enemyMaxHp;

            UpdateTextUI();
        }

        /// <summary>
        /// 턴 진행을 알리는 텍스트 갱신 (추가 UI 요소가 있다면 연결)
        /// </summary>
        public void UpdateTurn(int turnCount)
        {
            Debug.Log($"[BattleUIMockup] Turn updated: {turnCount}");
            // TODO: turnText.text = $"Turn: {turnCount}";
        }
    }
}
