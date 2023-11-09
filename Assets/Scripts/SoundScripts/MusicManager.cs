using UnityEngine;

namespace KitchenChaos
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }

        private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

        private AudioSource audioSource;

        public float Volume { get; private set; } = 1f;

        private void Awake()
        {
            Instance = this;

            audioSource = GetComponent<AudioSource>();
            Volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
            audioSource.volume = Volume;
        }

        public void ChangeVolume()
        {
            Volume += .1f;
            if (Volume > 1f)
            {
                Volume = 0f;
            }

            audioSource.volume = Volume;

            PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, Volume);
            PlayerPrefs.Save();
        }
    }
}