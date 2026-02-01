using System.Collections.Generic;
using UnityEngine;
using Calchemy.Entities;
using Calchemy.Data;
using Calchemy.Core;

namespace Calchemy.Systems
{
    /// <summary>
    /// 4x4 그리드 관리 및 빙고 판정 시스템
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        public GridConfiguration config;
        public List<BingoPatternAsset> activePatterns;
        
        private UnitEntity[,] grid;

        private void Awake()
        {
            if (config != null) InitializeGrid();
        }

        public void InitializeGrid()
        {
            grid = new UnitEntity[config.columns, config.rows];
        }

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
        /// 등록된 패턴들을 기반으로 빙고 리스트를 반환
        /// </summary>
        public List<List<UnitEntity>> CheckBingos()
        {
            List<List<UnitEntity>> foundBingos = new List<List<UnitEntity>>();
            List<IBingoCondition> conditions = new List<IBingoCondition> 
            { 
                new ElementBingoCondition(), 
                new HarmonyBingoCondition() 
            };

            foreach (var pattern in activePatterns)
            {
                if (pattern.isLineType)
                {
                    // 라인형 패턴은 그리드의 모든 행/열/대각선을 자동으로 스캔 (MVP 수준)
                    ScanLineBingos(foundBingos, conditions);
                }
                else
                {
                    // 커스텀 모양 패턴 스캔
                    ScanPatternBingos(pattern, foundBingos, conditions);
                }
            }

            return foundBingos;
        }

        private void ScanLineBingos(List<List<UnitEntity>> foundBingos, List<IBingoCondition> conditions)
        {
            // 가로 스캔
            for (int y = 0; y < config.rows; y++)
            {
                List<UnitEntity> line = new List<UnitEntity>();
                for (int x = 0; x < config.columns; x++)
                    if (grid[x, y] != null) line.Add(grid[x, y]);
                
                EvaluateLine(line, foundBingos, conditions);
            }

            // 세로 스캔
            for (int x = 0; x < config.columns; x++)
            {
                List<UnitEntity> line = new List<UnitEntity>();
                for (int y = 0; y < config.rows; y++)
                    if (grid[x, y] != null) line.Add(grid[x, y]);
                
                EvaluateLine(line, foundBingos, conditions);
            }

            // 대각선 (정사각형 그리드 가정)
            if (config.columns == config.rows)
            {
                List<UnitEntity> d1 = new List<UnitEntity>(), d2 = new List<UnitEntity>();
                for (int i = 0; i < config.columns; i++)
                {
                    if (grid[i, i] != null) d1.Add(grid[i, i]);
                    if (grid[config.columns - 1 - i, i] != null) d2.Add(grid[config.columns - 1 - i, i]);
                }
                EvaluateLine(d1, foundBingos, conditions);
                EvaluateLine(d2, foundBingos, conditions);
            }
        }

        private void ScanPatternBingos(BingoPatternAsset pattern, List<List<UnitEntity>> foundBingos, List<IBingoCondition> conditions)
        {
            for (int y = 0; y < config.rows; y++)
            {
                for (int x = 0; x < config.columns; x++)
                {
                    List<UnitEntity> matchCandidate = new List<UnitEntity>();
                    foreach (var offset in pattern.offsets)
                    {
                        int targetX = x + offset.x;
                        int targetY = y + offset.y;
                        if (IsValidCoordinate(targetX, targetY) && grid[targetX, targetY] != null)
                            matchCandidate.Add(grid[targetX, targetY]);
                    }

                    if (matchCandidate.Count == pattern.offsets.Length)
                        EvaluateLine(matchCandidate, foundBingos, conditions);
                }
            }
        }

        private void EvaluateLine(List<UnitEntity> line, List<List<UnitEntity>> foundBingos, List<IBingoCondition> conditions)
        {
            if (line.Count < 3) return; // 최소 빙고 단위 (가변 가능)

            foreach (var cond in conditions)
            {
                if (cond.IsSatisfied(line))
                {
                    foundBingos.Add(line);
                    break;
                }
            }
        }

        private bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < config.columns && y >= 0 && y < config.rows;
        }

        /// <summary>
        /// 그리드 조작 인터페이스 (SWAP, TRANSFORM 등)
        /// </summary>
        public void ExecuteGridAction(string actionType, string targetSelector, int count, Vector2Int origin, ElementType toType = ElementType.None, string condition = "")
        {
            List<Vector2Int> candidates = GetTargetPositions(targetSelector, origin);
            
            // TODO: 필터링 로직 (condition) 적용 리스트업
            
            // 무작위 선택 시 셔플
            if (targetSelector == "RANDOM")
            {
                ShuffleList(candidates);
            }

            int affectedCount = 0;
            foreach (var pos in candidates)
            {
                if (affectedCount >= count) break;
                if (!IsValidCoordinate(pos.x, pos.y)) continue;

                switch (actionType.ToUpper())
                {
                    case "SWAP":
                        SwapUnits(origin, pos);
                        affectedCount++;
                        break;
                    case "TRANSFORM":
                        TransformUnit(pos, toType, origin);
                        affectedCount++;
                        break;
                }
            }
        }

        private List<Vector2Int> GetTargetPositions(string selector, Vector2Int origin)
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            int x = origin.x;
            int y = origin.y;

            switch (selector.ToUpper())
            {
                case "UP":
                    int ty = y - 1;
                    if (!IsValidCoordinate(x, ty)) ty = y + 1; // Reflection
                    positions.Add(new Vector2Int(x, ty));
                    break;
                case "DOWN":
                    ty = y + 1;
                    if (!IsValidCoordinate(x, ty)) ty = y - 1;
                    positions.Add(new Vector2Int(x, ty));
                    break;
                case "LEFT":
                    int tx = x - 1;
                    if (!IsValidCoordinate(tx, y)) tx = x + 1;
                    positions.Add(new Vector2Int(tx, y));
                    break;
                case "RIGHT":
                    tx = x + 1;
                    if (!IsValidCoordinate(tx, y)) tx = x - 1;
                    positions.Add(new Vector2Int(tx, y));
                    break;
                case "NEAR_4":
                    Vector2Int[] offsets4 = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
                    foreach (var offset in offsets4)
                    {
                        Vector2Int np = origin + offset;
                        if (IsValidCoordinate(np.x, np.y)) positions.Add(np);
                    }
                    break;
                case "RANDOM":
                case "ALL":
                    for (int iy = 0; iy < config.rows; iy++)
                        for (int ix = 0; ix < config.columns; ix++)
                            if (ix != x || iy != y) positions.Add(new Vector2Int(ix, iy));
                    break;
            }
            return positions;
        }

        private void TransformUnit(Vector2Int pos, ElementType toType, Vector2Int originPos)
        {
            UnitEntity target = grid[pos.x, pos.y];
            if (target == null) return;

            ElementType finalType = toType;
            if (toType == ElementType.Harmony) // 레거시의 ORIGIN 대용
            {
                finalType = grid[originPos.x, originPos.y].cardData.elementType;
            }

            // 실시간 원소 변경 (Data를 새로 할당하거나 필드 수정)
            // 여기서는 시연을 위해 로그만 남기거나 간단한 교체 로직 필요
            Debug.Log($"[GridManager] {pos}의 원소를 {finalType}으로 변환");
        }

        private void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
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
