using System.Collections.Generic;
using Calchemy.Entities;

namespace Calchemy.Core
{
    /// <summary>
    /// 빙고 달성 조건 인터페이스
    /// </summary>
    public interface IBingoCondition
    {
        string ConditionName { get; }
        bool IsSatisfied(List<UnitEntity> units);
    }
}
