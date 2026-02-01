using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Calchemy.Systems;
using Calchemy.Entities;
using LogicForge.Schema.V2; // Ensure this namespace is reachable (Assembly definitions might affect this, usually Editor accesses Scripts)

namespace Tests.Editor
{
    public class LogicV2Test
    {
        private GameObject testObj;
        private V2LogicAdapter adapter;
        private UnitEntity entity;

        [SetUp]
        public void Setup()
        {
            testObj = new GameObject("TestObject");
            adapter = testObj.AddComponent<V2LogicAdapter>();
            entity = testObj.AddComponent<UnitEntity>();
            
            // Initialize Entity Data
            entity.maxHp = 100;
            entity.currentHp = 100;
            entity.statusEffects = new List<StatusEffect>(); // Initialize list
            entity.cardData = ScriptableObject.CreateInstance<Calchemy.Data.CardData>(); // Mock CardData to prevent NPE on Die() logging
            entity.cardData.cardName = "Test Dummy";
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(testObj);
            Object.DestroyImmediate(entity.cardData);
        }

        [Test]
        public void Verify_DealDamage_DecreasesHP()
        {
            // Arrange
            var effect = new EffectSchema
            {
                type = "DealDamage",
                formula = "20"
            };

            // Act
            adapter.Execute(effect, entity);

            // Assert
            Assert.AreEqual(80, entity.currentHp, "HP should decrease by 20");
        }

        [Test]
        public void Verify_ApplyStatus_AddsStatusEffect()
        {
            // Arrange
            var effect = new EffectSchema
            {
                type = "ApplyStatus",
                valueString = "Burn",
                formula = "3", // Stack amount
                duration = new DurationSchema { value = 2 }
            };

            // Act
            adapter.Execute(effect, entity);

            // Assert
            Assert.AreEqual(1, entity.statusEffects.Count, "Should have 1 status effect");
            Assert.AreEqual("Burn", entity.statusEffects[0].id);
            Assert.AreEqual(3, entity.statusEffects[0].value);
            Assert.AreEqual(2, entity.statusEffects[0].duration);
        }

        [Test]
        public void Verify_ApplyStatus_StacksExisting()
        {
            // Arrange
            entity.AddStatus("Poison", 2, 3); // Existing

            var effect = new EffectSchema
            {
                type = "ApplyStatus",
                valueString = "Poison",
                formula = "3", // Add 3 stacks
                duration = new DurationSchema { value = 5 } // New longer duration
            };

            // Act
            adapter.Execute(effect, entity);

            // Assert
            Assert.AreEqual(1, entity.statusEffects.Count, "Should remain 1 stacked status");
            Assert.AreEqual("Poison", entity.statusEffects[0].id);
            Assert.AreEqual(5, entity.statusEffects[0].value, "Stacks should sum up (2+3=5)");
            Assert.AreEqual(5, entity.statusEffects[0].duration, "Duration should update to max (3 vs 5)");
        }
    }
}
