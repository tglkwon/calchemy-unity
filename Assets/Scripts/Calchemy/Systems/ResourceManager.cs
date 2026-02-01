using UnityEngine;
using System.Collections.Generic;

namespace Calchemy.Systems
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }
        
        // 캐시 저장소 (ID -> Sprite)
        private Dictionary<string, Sprite> _spriteCache = new Dictionary<string, Sprite>();

        // 경로 상수
        // Standard path: Resources/Images/Cards/{ID}
        private const string PATH_PREFIX = "Images/Cards/";
        private const string FALLBACK_PATH = "Images/Cards/default_card"; // 기본 이미지 경로

        private void Awake()
        {
            if (Instance == null) 
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else 
            {
                Destroy(gameObject);
            }
            
            // 초기화 시 Fallback 이미지를 로드해두는 것이 안전함
            if (Resources.Load<Sprite>(FALLBACK_PATH) == null)
            {
                Debug.LogWarning($"[ResourceManager] Fallback image missing at 'Resources/{FALLBACK_PATH}'");
            }
        }

        /// <summary>
        /// 단일 카드의 스프라이트를 로드하고 캐싱합니다.
        /// </summary>
        public void LoadCardSprite(string cardId, string imageId = null)
        {
            if (string.IsNullOrEmpty(cardId)) return;

            // 1. 이미 캐시에 있으면 패스
            if (_spriteCache.ContainsKey(cardId)) return;

            // 2. 경로 결정 (LogicForge에 imageId 필드가 있다면 우선 사용, 없으면 cardId 사용)
            string resourceName = !string.IsNullOrEmpty(imageId) ? imageId : cardId;
            string fullPath = $"{PATH_PREFIX}{resourceName}";

            // 3. 리소스 로드
            Sprite sprite = Resources.Load<Sprite>(fullPath);

            // 4. 실패 시 Fallback 처리 및 로깅
            if (sprite == null)
            {
                Debug.LogWarning($"[ResourceManager] Missing sprite for ID: {cardId} (Path: {fullPath}). Using Fallback.");
                sprite = Resources.Load<Sprite>(FALLBACK_PATH);
            }

            // 5. 캐시 등록 (Fallback도 없으면 null이 들어갈 수 있음, null 체크)
            if (sprite != null)
            {
                _spriteCache[cardId] = sprite;
            }
        }

        /// <summary>
        /// UI나 인게임에서 이미지를 요청할 때 사용
        /// </summary>
        public Sprite GetSprite(string cardId)
        {
            if (string.IsNullOrEmpty(cardId)) return null;

            if (_spriteCache.TryGetValue(cardId, out Sprite sprite))
            {
                return sprite;
            }
            
            // 캐시에 없으면 Fallback 리턴 (혹은 여기서 로드 시도할 수도 있음)
            return Resources.Load<Sprite>(FALLBACK_PATH);
        }
    }
}
