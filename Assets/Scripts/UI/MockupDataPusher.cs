using UnityEngine;
using System.Collections;
using Calchemy.UI;

namespace Calchemy.Demo
{
    /// <summary>
    /// 향후 실제 MCP 통신 코드로 교체될 예정인 임시 데이터 푸셔입니다.
    /// BattleUIMockup의 외부 훅(Hooks)이 정상 작동하는지 테스트합니다.
    /// </summary>
    public class MockupDataPusher : MonoBehaviour
    {
        public BattleUIMockup targetUI;

        private int turn = 0;
        private int pMax = 100;
        private int pHp = 100;
        private int eMax = 250;
        private int eHp = 250;

        private void Start()
        {
            if (targetUI == null)
            {
                targetUI = FindFirstObjectByType<BattleUIMockup>();
            }
        }

        [ContextMenu("Simulate Turn Data Sync (MCP Receive)")]
        public void SimulateMcpDataReceive()
        {
            if (targetUI == null) return;

            turn++;
            targetUI.UpdateTurn(turn);

            // 임의의 데미지 시뮬레이션
            pHp -= Random.Range(0, 15);
            eHp -= Random.Range(5, 30);

            if (pHp < 0) pHp = 0;
            if (eHp < 0) eHp = 0;

            // Mockup 스크립트의 외부 연동 함수 호출
            targetUI.SyncState(pHp, pMax, eHp, eMax);
        }

        [ContextMenu("Simulate End Turn (Animation trigger)")]
        public void SimulateMcpActionTrigger()
        {
            if (targetUI == null) return;

            targetUI.DamageTrigger(true, 10);
            targetUI.DamageTrigger(false, 25);
        }
    }
}
