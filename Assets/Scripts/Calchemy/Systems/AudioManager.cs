using UnityEngine;

namespace Calchemy.Systems
{
    /// <summary>
    /// 맵 전환, 전투 등 게임 진행에 따른 BGM과 주요 효과음(SFX)을 총괄 제어합니다.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        public AudioSource bgmSource;
        public AudioSource sfxSource;

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
        }

        public void PlayBGM(AudioClip clip, bool fade = true)
        {
            Debug.Log($"[AudioManager] BGM 재생 요청: {(clip != null ? clip.name : "null")} (뼈대)");
            if (bgmSource != null && clip != null)
            {
                bgmSource.clip = clip;
                bgmSource.Play();
            }
        }

        public void PlaySFX(AudioClip clip)
        {
            if (sfxSource != null && clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }
    }
}
