using System.Collections.Generic;
using UnityEngine;
using LogicForge.Schema;
using Newtonsoft.Json; // Requires 'com.unity.nuget.newtonsoft-json'

namespace Core.Data
{
    public class DataLoader : MonoBehaviour
    {
        private const string DATA_PATH = "Data/Raw/cards"; // Resources relative path without extension

        public List<SharedLogic> LoadedCards { get; private set; } = new List<SharedLogic>();

        private void Start()
        {
            LoadCardData();
        }

        public void LoadCardData()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(DATA_PATH);
            if (jsonFile == null)
            {
                Debug.LogError($"[DataLoader] Failed to load card data at {DATA_PATH}");
                return;
            }

            try
            {
                // Deserialize directly to List<SharedLogic> assuming API returns an array.
                // If API returns { "data": [...] }, use a wrapper.
                // Defaulting to List based on typical behavior, can be adjusted.
                LoadedCards = JsonConvert.DeserializeObject<List<SharedLogic>>(jsonFile.text);
                
                if (LoadedCards == null)
                {
                     // Fallback: Try wrapper if null (simple heuristic)
                    var wrapper = JsonConvert.DeserializeObject<CardDataWrapper>(jsonFile.text);
                    if (wrapper != null && wrapper.data != null)
                    {
                        LoadedCards = wrapper.data;
                    }
                }

                ValidateCards();
                Debug.Log($"[DataLoader] Successfully loaded {LoadedCards?.Count ?? 0} cards.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[DataLoader] Json Parse Error: {e.Message}");
            }
        }

        private void ValidateCards()
        {
            if (LoadedCards == null) return;

            int invalidCount = 0;
            for (int i = LoadedCards.Count - 1; i >= 0; i--)
            {
                var card = LoadedCards[i];
                bool isValid = true;

                // 1. Mandatory Fields
                if (string.IsNullOrEmpty(card.Id) || string.IsNullOrEmpty(card.Name))
                {
                    Debug.LogError($"[Data Integrity] Card at index {i} missing ID or Name.");
                    isValid = false;
                }

                // 2. Resource Validation (Image)
                // Assuming Metadata contains 'imageId' or similar, or we map ID to sprite.
                // Example: Check if sprite exists in Resources/Sprites/Cards/{ID}
                // if (Resources.Load<Sprite>($"Sprites/Cards/{card.Id}") == null) { ... }

                if (!isValid)
                {
                    LoadedCards.RemoveAt(i);
                    invalidCount++;
                }
            }

            if (invalidCount > 0)
            {
                Debug.LogWarning($"[DataLoader] Removed {invalidCount} invalid cards.");
            }
        }

        [System.Serializable]
        private class CardDataWrapper
        {
            public List<SharedLogic> data;
        }
    }
}
