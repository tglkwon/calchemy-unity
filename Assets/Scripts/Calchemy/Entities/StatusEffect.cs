using System;

namespace Calchemy.Entities
{
    [Serializable]
    public class StatusEffect
    {
        public string id;       // e.g., "Burn", "Poison"
        public int value;       // Stack amount or intensity
        public int duration;    // Turn count
        
        public StatusEffect(string id, int value, int duration)
        {
            this.id = id;
            this.value = value;
            this.duration = duration;
        }
    }
}
