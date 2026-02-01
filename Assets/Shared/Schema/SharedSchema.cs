namespace LogicForge.Schema
{
    using System.Collections.Generic;

    [System.Serializable]
    public partial class SharedLogic
    {
        /// <summary>
        /// 8-Component 로직 배열
        /// </summary>
        public List<Component> Components ;

        /// <summary>
        /// 고유 식별자
        /// </summary>
        public string Id ;

        /// <summary>
        /// 추가 개발 정보
        /// </summary>
        public Dictionary<string, object> Metadata ;

        /// <summary>
        /// 기획 명칭
        /// </summary>
        public string Name ;
    }

    [System.Serializable]
    public partial class Component
    {
        /// <summary>
        /// 대상 필터 조건 (예: IF_ENEMY_ALIVE, IF_HP_BELOW_50)
        /// </summary>
        public string Condition ;

        /// <summary>
        /// 소모 자원 (예: MP 30, HP 10)
        /// </summary>
        public string Cost ;

        /// <summary>
        /// 지속 시간 및 횟수 (예: 2_TURNS, INSTANT, PERMANENT)
        /// </summary>
        public string Duration ;

        /// <summary>
        /// 실제 액션/효과 (예: DAMAGE, HEAL, APPLY_BUF, DRAW_CARD)
        /// </summary>
        public string Effect ;

        /// <summary>
        /// 수치 계산식 (예: $ATK * 1.5, 30 + $INT)
        /// </summary>
        public string Formula ;

        /// <summary>
        /// 컴포넌트 이름 (표시용)
        /// </summary>
        public string Name;

        /// <summary>
        /// 사용 가능 조건 (예: MP > 30, HAND_SIZE < 5)
        /// </summary>
        public string Requirement ;

        /// <summary>
        /// 대상 (예: TARGET_ENEMY_ALL, TARGET_SELF, TARGET_ALLY_LOWEST_HP)
        /// </summary>
        public string Target ;

        /// <summary>
        /// 발동 시점 (예: ON_PLAY, ON_TURN_START, ON_DEATH)
        /// </summary>
        public string Trigger ;
    }
}
