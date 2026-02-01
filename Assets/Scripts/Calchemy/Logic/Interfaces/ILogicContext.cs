using UnityEngine;

namespace LogicForge.Logic
{
    public interface ILogicContext
    {
        GameObject Caster { get; }
        GameObject Target { get; }
        // Add more context as needed (e.g. BattleManager, GridState)
    }
}
