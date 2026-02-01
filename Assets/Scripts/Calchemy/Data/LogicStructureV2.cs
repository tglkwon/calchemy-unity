using System;
using System.Collections.Generic;

namespace LogicForge.Schema.V2
{
    [Serializable]
    public class CardListSchema
    {
        public string version;
        public List<CardSchema> cards;
    }

    [Serializable]
    public class CardSchema
    {
        public string id;
        public string name;
        public string imageId;
        public string description;
        public LogicSchema logic;
    }

    [Serializable]
    public class LogicSchema
    {
        public TriggerSchema trigger;
        public TargetSchema target;
        public CostSchema cost;
        public List<EffectSchema> effects;
    }

    [Serializable]
    public class TriggerSchema
    {
        public string type;
        public int priority;
    }

    [Serializable]
    public class TargetSchema
    {
        public string type;
        public string filter;
    }

    [Serializable]
    public class CostSchema
    {
        public string type;
        public int value;
    }

    [Serializable]
    public class EffectSchema
    {
        public int sequenceId;
        public string type;
        public string formula;
        public string valueString; // Optional field saw in JSON
        public DurationSchema duration;
        public List<string> conditions; // Empty array in JSON, assuming strings or objects
    }

    [Serializable]
    public class DurationSchema
    {
        public string type;
        public int value;
    }
}
