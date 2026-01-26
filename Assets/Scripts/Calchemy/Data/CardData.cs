using UnityEngine;
using Calchemy.Core;

namespace Calchemy.Data
{
    /// <summary>
    /// 카드 데이터 ScriptableObject
    /// Antigravity 데이터를 엔진용 고정 데이터로 변환하여 관리
    /// </summary>
    [CreateAssetMenu(fileName = "NewCardData", menuName = "Calchemy/Card Data")]
    public class CardData : ScriptableObject
    {
        [Header("기본 정보")]
        public string id;
        public string cardName;
        public ElementType elementType;
        public CardGrade grade;

        [Header("효과 로직")]
        public EffectLogic singleLogic; // 단일 발동 로직
        public EffectLogic bingoLogic;  // 빙고 시 발동 로직

        [Header("추가 연출 데이터")]
        public Sprite cardIllust;
        public string description;
    }
}
