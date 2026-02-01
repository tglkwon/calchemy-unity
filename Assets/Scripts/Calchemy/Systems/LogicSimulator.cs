using UnityEngine;
using LogicForge.Schema;
using LogicForge.Logic;
using Calchemy.Systems;

namespace Calchemy.Systems
{
    public class LogicSimulator : MonoBehaviour, ILogicContext
    {
        public DataLoader dataLoader;
        
        // ILogicContext Implementation
        public GameObject Caster => this.gameObject;
        public GameObject Target => this.gameObject; // Self-target for test

        private void Start()
        {
            if (dataLoader == null) dataLoader = FindObjectOfType<DataLoader>();
        }

        [ContextMenu("Run Simulation")]
        public void RunTest()
        {
            if (dataLoader == null || dataLoader.LoadedCards == null || dataLoader.LoadedCards.Count == 0)
            {
                Debug.LogWarning("[LogicSimulator] No cards loaded. Please run DataLoader first.");
                return;
            }

            var card = dataLoader.LoadedCards[0];
            Debug.Log($"[LogicSimulator] Simulating Card: {card.Name}");

            if (card.Components != null)
            {
                foreach (var component in card.Components)
                {
                    component.Execute(this);
                }
            }
        }
    }
}
