using UnityEngine;
using Calchemy.Entities;
using LogicForge.Schema.V2;

namespace Calchemy.Systems
{
    public class V2LogicAdapter : MonoBehaviour
    {
        public void Execute(EffectSchema effect, UnitEntity target)
        {
            if (effect == null) return;
            
            // Formula parsing should happen here (mocked for now as simple parse)
            float value = ParseFormula(effect.formula);

            switch (effect.type)
            {
                case "DealDamage":
                    // V2 "DealDamage" -> Entity.TakeDamage
                    Debug.Log($"[V2Logic] DealDamage: {value} to {target.name}");
                    target.TakeDamage((int)value);
                    break;

                case "ApplyStatus":
                    // V2 "ApplyStatus" -> Entity.AddStatus
                    // valueString stores the status ID (e.g. "Burn")
                    // formula stores the stack amount (e.g. "3")
                    // duration.value stores turn count
                    string statusId = effect.valueString;
                    int duration = effect.duration != null ? effect.duration.value : 1;
                    
                    Debug.Log($"[V2Logic] ApplyStatus: {statusId} (Val: {value}, Dur: {duration}) to {target.name}");
                    target.AddStatus(statusId, (int)value, duration);
                    break;

                default:
                    Debug.LogWarning($"[V2Logic] Unsupported Effect Type: {effect.type}");
                    break;
            }
        }

        private float ParseFormula(string formula)
        {
            // Mock formula solver
            // In real impl, use a parser like NCalc or simple Regex replacement
            if (string.IsNullOrEmpty(formula)) return 0;
            
            // Validating number for simple test
            if (float.TryParse(formula, out float result))
            {
                return result;
            }
            
            // Mock: if formula contains "$ATK", returns 10
            if (formula.Contains("$ATK")) return 10f;

            return 0f;
        }
    }
}
