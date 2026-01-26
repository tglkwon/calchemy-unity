using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calchemy.Core;

namespace Calchemy.Systems
{
    /// <summary>
    /// 전투 페이즈 및 턴 흐름 제어 매니저
    /// </summary>
    public class BattleManager : MonoBehaviour
    {
        public GridManager gridManager;
        public EffectProcessor effectProcessor;

        public void StartBattle()
        {
            StartCoroutine(BattleRoutine());
        }

        private IEnumerator BattleRoutine()
        {
            Debug.Log("전투 시작 페이즈");
            yield return new WaitForSeconds(1f);

            // 1. 카드 배치 페이즈 (유저/AI 액션)
            Debug.Log("카드 배치 대기...");
            yield return new WaitForSeconds(0.5f);

            // 2. 순차 발동 페이즈
            Debug.Log("순차 카드 효과 발동 시작");
            yield return StartCoroutine(ProcessCardActivation());

            // 3. 빙고 체크 및 실행
            Debug.Log("빙고 판결 중...");
            List<List<Entities.UnitEntity>> bingos = gridManager.CheckBingos();
            foreach (var bingo in bingos)
            {
                foreach (var unit in bingo)
                {
                    effectProcessor.Process(unit.cardData.bingoLogic, unit);
                }
                yield return new WaitForSeconds(0.5f); // 연출 대기
            }

            // 4. 적 행동 페이즈
            Debug.Log("적 행동 시작");
            yield return new WaitForSeconds(1f);

            Debug.Log("턴 종료");
        }

        private IEnumerator ProcessCardActivation()
        {
            // 그리드 순회하며 카드 개별 로직 발동 시뮬레이션
            // 실제로는 GridManager의 데이터를 가져와 순회
            yield return null;
        }
    }
}
