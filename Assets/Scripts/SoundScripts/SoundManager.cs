using Counters;
using MySOs;
using MyUIs;
using System;
using UnityEngine;

namespace KitchenChaos
{
    public class SoundManager : MonoBehaviour
    {
        private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

        public static SoundManager Instance { get; private set; }

        [SerializeField] private AudioClipRefsSO audioClipRefsSO;

        public float Volume { get; private set; } = 1f;

        private void Awake()
        {
            Instance = this;
            Volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
        }

        private void Start()
        {
            BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyobjectPlacedHere;
            TrashCounter.OnAnyTrashThrowen += TrashCounter_OnAnyTrashThrowen;
            CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;

            DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagerInstance_OnRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += DeliveryManagerInstance_OnRecipeFailed;

            CountdownTextUI.Instance.OnCountdownNumberChanged += GameStartCountdownUI_OnCountdownNumberChanged;

            Player.OnAnyPlayerPickedSomething += Player_OnAnyPlayerPickedSomething;
        }
        public void PlayBurnWarningSound(StoveCounter stoveCounter)
        {
            PlaySound(audioClipRefsSO.warning[1], stoveCounter.transform.position);
        }

        private void GameStartCountdownUI_OnCountdownNumberChanged(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.warning[0], Vector3.zero);
        }

        private void TrashCounter_OnAnyTrashThrowen(object sender, EventArgs e)
        {
            TrashCounter trashCounter = sender as TrashCounter;
            PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
        }

        private void BaseCounter_OnAnyobjectPlacedHere(object sender, EventArgs e)
        {
            BaseCounter baseCounter = (BaseCounter)sender;
            PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
        }

        private void Player_OnAnyPlayerPickedSomething(object sender, EventArgs e)
        {
            Player player = sender as Player;
            PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
        }

        private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
        {
            CuttingCounter cuttingCounter = (CuttingCounter)sender;
            PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
        }

        private void DeliveryManagerInstance_OnRecipeFailed(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
        }

        private void DeliveryManagerInstance_OnRecipeSuccess(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
        }

        private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
        {
            PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
        }

        private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * Volume);
        }

        public void ChangeVolume()
        {
            Volume += .1f;
            if (Volume > 1f)
            {
                Volume = 0f;
            }

            PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, Volume);
            PlayerPrefs.Save();
        }
    }
}