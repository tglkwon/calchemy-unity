using UnityEngine;

namespace Calchemy.Data
{
    /// <summary>
    /// 빙고 모양 패턴 데이터
    /// 특정 셀을 기준으로 한 상대 좌표(Offset)들의 집합
    /// </summary>
    [CreateAssetMenu(fileName = "NewBingoPattern", menuName = "Calchemy/Bingo Pattern")]
    public class BingoPatternAsset : ScriptableObject
    {
        public string patternName;
        
        [Tooltip("패턴을 구성하는 상대 좌표 리스트 (예: T자형은 (0,0), (1,0), (2,0), (1,1))")]
        public Vector2Int[] offsets;

        [Tooltip("이 패턴이 라인형(가로/세로/대각선)인지 여부")]
        public bool isLineType;
    }
}
