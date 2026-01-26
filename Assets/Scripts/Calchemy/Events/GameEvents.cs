using System;
using UnityEngine;
using Calchemy.Entities;

namespace Calchemy.Events
{
    /// <summary>
    /// 시스템 간 결합도를 낮추기 위한 전역 이벤트 허브
    /// </summary>
    public static class GameEvents
    {
        // 체력 변화 시 (UnitEntity, 변화량)
        public static Action<UnitEntity, int> OnHpChanged;

        // 빙고 발생 시
        public static Action<int> OnBingoAchieved;

        // 전투 페이즈 변경 시
        public static Action<string> OnPhaseChanged;

        public static void TriggerHpChanged(UnitEntity unit, int delta) => OnHpChanged?.Invoke(unit, delta);
        public static void TriggerBingo(int count) => OnBingoAchieved?.Invoke(count);
        public static void TriggerPhaseChanged(string phaseName) => OnPhaseChanged?.Invoke(phaseName);
    }
}
