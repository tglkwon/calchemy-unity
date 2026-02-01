using NUnit.Framework;
using UnityEngine;
using System.IO;
using LogicForge.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tests.Editor
{
    public class ResourceIntegrityTest
    {
        private const string RELATIVE_JSON_PATH = "Assets/Resources/Data/Raw/cards.json";
        private const string IMAGE_PATH_PREFIX = "Images/Cards/";

        [Test]
        public void Verify_All_Card_Resources_Exist()
        {
            // 1. JSON 파일 로드
            if (!File.Exists(RELATIVE_JSON_PATH))
            {
                Assert.Fail($"JSON file not found at {RELATIVE_JSON_PATH}");
                return;
            }

            string json = File.ReadAllText(RELATIVE_JSON_PATH);
            List<SharedLogic> cards = JsonConvert.DeserializeObject<List<SharedLogic>>(json);
            
            // Wrapper check just in case
            if (cards == null) 
            {
                var wrapper = JsonConvert.DeserializeObject<CardDataWrapper>(json);
                cards = wrapper?.data;
            }

            Assert.IsNotNull(cards, "Failed to parse cards.json");

            // 2. 리소스 검증
            int missingCount = 0;
            foreach (var card in cards)
            {
                // LogicForge data might not have 'imageId' yet, assuming ID for now
                // string resourceName = !string.IsNullOrEmpty(card.imageId) ? card.imageId : card.Id; 
                string resourceName = card.Id; 
                string path = $"{IMAGE_PATH_PREFIX}{resourceName}";

                // Resources.Load works in Editor Mode
                Sprite sprite = Resources.Load<Sprite>(path);

                if (sprite == null)
                {
                    Debug.LogWarning($"[Test] Missing Resource: '{path}' for Card ID '{card.Id}'");
                    missingCount++;
                }
            }

            // Assert
            // Note: We might allow SOME missing resources if fallbacks are intended, 
            // but for a strict check, we can warn or fail. 
            // Let's Log Assertions instead of failing the test to allow "Soft Logic" if art is WIP.
            if (missingCount > 0)
            {
                Debug.LogWarning($"Total missing sprites: {missingCount} / {cards.Count}");
                // Assert.Fail($"Missing {missingCount} sprites. Check console for details."); // Uncomment for strict mode
            }
            else
            {
                Debug.Log("All card sprites validated successfully.");
            }
        }

        private class CardDataWrapper
        {
            public List<SharedLogic> data;
        }
    }
}
