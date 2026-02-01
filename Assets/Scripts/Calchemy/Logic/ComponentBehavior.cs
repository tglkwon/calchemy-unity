using UnityEngine;
using LogicForge.Logic;

namespace LogicForge.Schema
{
    public static class ComponentBehaviorExtensions
    {
        public static void Execute(this LogicForge.Schema.Component component, ILogicContext context)
        {
            if (string.IsNullOrEmpty(component.Effect)) return;

            // Simple Switch-Case based on the string Key
            switch (component.Effect)
            {
                case "ATTACK":
                case "DAMAGE":
                    ExecuteDamage(component, context);
                    break;
                case "HEAL":
                    ExecuteHeal(component, context);
                    break;
                case "LOG_DEBUG":
                    Debug.Log($"[LogicForge] Execute: {component.Name ?? "Unknown"} - Condition: {component.Condition}");
                    break;
                default:
                    Debug.LogWarning($"[LogicForge] Unknown Effect: {component.Effect}");
                    break;
            }
        }

        private static void ExecuteDamage(Component component, ILogicContext context)
        {
            // Parse Formula if needed
            Debug.Log($"[Logic (Damage)] Targeting {context.Target?.name} with Formula: {component.Formula}");
        }

        private static void ExecuteHeal(Component component, ILogicContext context)
        {
            Debug.Log($"[Logic (Heal)] Targeting {context.Target?.name} with Formula: {component.Formula}");
        }
    }
}
