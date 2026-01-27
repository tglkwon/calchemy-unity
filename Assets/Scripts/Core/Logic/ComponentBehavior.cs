using UnityEngine;
using LogicForge.Logic;

namespace LogicForge.Schema
{
    // Extend the generated partial class
    public partial class Component
    {
        public void Execute(ILogicContext context)
        {
            if (string.IsNullOrEmpty(this.Effect)) return;

            // Simple Switch-Case based on the string Key
            // This replaces the separate "EffectFactory"
            switch (this.Effect)
            {
                case "ATTACK":
                case "DAMAGE":
                    ExecuteDamage(context);
                    break;
                case "HEAL":
                    ExecuteHeal(context);
                    break;
                case "LOG_DEBUG":
                    Debug.Log($"[LogicForge] Execute: {this.Name ?? "Unknown"} - Condition: {this.Condition}");
                    break;
                default:
                    Debug.LogWarning($"[LogicForge] Unknown Effect: {this.Effect}");
                    break;
            }
        }

        private void ExecuteDamage(ILogicContext context)
        {
            // Parse Formula if needed, e.g. int amount = FormulaSolver.Solve(this.Formula);
            Debug.Log($"[Logic (Damage)] Targeting {context.Target?.name} with Formula: {this.Formula}");
            // Actual damage logic calls would go here
        }

        private void ExecuteHeal(ILogicContext context)
        {
            Debug.Log($"[Logic (Heal)] Targeting {context.Target?.name} with Formula: {this.Formula}");
        }
    }
}
