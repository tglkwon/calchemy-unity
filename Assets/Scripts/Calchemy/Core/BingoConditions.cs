using System.Collections.Generic;
using Calchemy.Entities;

namespace Calchemy.Core
{
    /// <summary>
    /// 동일 원소 빙고 조건 (All Same Element)
    /// </summary>
    public class ElementBingoCondition : IBingoCondition
    {
        public string ConditionName => "Element Bingo";

        public bool IsSatisfied(List<UnitEntity> units)
        {
            if (units == null || units.Count == 0) return false;
            
            var firstType = units[0].cardData.elementType;
            if (firstType == ElementType.None) return false;

            foreach (var unit in units)
            {
                if (unit == null || unit.cardData.elementType != firstType) 
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 4원소 조화 빙고 조건 (Harmony: Fire, Water, Earth, Air)
    /// </summary>
    public class HarmonyBingoCondition : IBingoCondition
    {
        public string ConditionName => "Harmony Bingo";

        public bool IsSatisfied(List<UnitEntity> units)
        {
            if (units == null || units.Count < 4) return false;

            HashSet<ElementType> types = new HashSet<ElementType>();
            foreach (var unit in units)
            {
                if (unit != null) types.Add(unit.cardData.elementType);
            }

            return types.Contains(ElementType.Fire) && 
                   types.Contains(ElementType.Water) && 
                   types.Contains(ElementType.Earth) && 
                   types.Contains(ElementType.Air);
        }
    }
}
