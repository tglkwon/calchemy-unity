using System.Collections.Generic;
using UnityEngine;
using Calchemy.Entities;
using Calchemy.Core;

namespace Calchemy.Systems
{
    /// <summary>
    /// 4x4 그리드 관리 및 빙고 판정 시스템
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        private const int GRID_SIZE = 4;
        private UnitEntity[,] grid = new UnitEntity[GRID_SIZE, GRID_SIZE];

        /// <summary>
        /// 특정 좌표에 카드 배치
        /// </summary>
        public void PlaceCard(int x, int y, UnitEntity unit)
        {
            if (IsValidCoordinate(x, y))
            {
                grid[x, y] = unit;
                unit.gridX = x;
                unit.gridY = y;
            }
        }

        /// <summary>
        /// 빙고 리스트를 반환 (가로, 세로, 대각형)
        /// </summary>
        public List<List<UnitEntity>> CheckBingos()
        {
            List<List<UnitEntity>> bingos = new List<List<UnitEntity>>();

            // 가로 체크
            for (int y = 0; y < GRID_SIZE; y++)
            {
                List<UnitEntity> row = new List<UnitEntity>();
                for (int x = 0; x < GRID_SIZE; x++)
                {
                    if (grid[x, y] != null) row.Add(grid[x, y]);
                }
                if (row.Count == GRID_SIZE) bingos.Add(row);
            }

            // 세로 체크
            for (int x = 0; x < GRID_SIZE; x++)
            {
                List<UnitEntity> col = new List<UnitEntity>();
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    if (grid[x, y] != null) col.Add(grid[x, y]);
                }
                if (col.Count == GRID_SIZE) bingos.Add(col);
            }

            // 대각선 체크 (추가 로직 필요)
            // ...

            return bingos;
        }

        private bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < GRID_SIZE && y >= 0 && y < GRID_SIZE;
        }

        /// <summary>
        /// 그리드 조작 인터페이스 (SWAP, TRANSFORM 등)
        /// </summary>
        public void ExecuteGridAction(string actionType, Vector2Int from, Vector2Int to)
        {
            switch (actionType)
            {
                case "SWAP":
                    SwapUnits(from, to);
                    break;
                // 기타 액션 추가 가능
            }
        }

        private void SwapUnits(Vector2Int a, Vector2Int b)
        {
            UnitEntity temp = grid[a.x, a.y];
            grid[a.x, a.y] = grid[b.x, b.y];
            grid[b.x, b.y] = temp;
            
            if(grid[a.x, a.y]) { grid[a.x, a.y].gridX = a.x; grid[a.x, a.y].gridY = a.y; }
            if(grid[b.x, b.y]) { grid[b.x, b.y].gridX = b.x; grid[b.x, b.y].gridY = b.y; }
        }
    }
}
