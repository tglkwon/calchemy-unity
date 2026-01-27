using UnityEngine;

namespace Calchemy.Core
{
    /// <summary>
    /// 그리드 설정 데이터
    /// 행/열 크기 등을 정의하여 가변 그리드 시스템 지원
    /// </summary>
    [CreateAssetMenu(fileName = "NewGridConfig", menuName = "Calchemy/Grid Config")]
    public class GridConfiguration : ScriptableObject
    {
        public int columns = 4;
        public int rows = 4;

        public int TotalSize => columns * rows;
    }
}
