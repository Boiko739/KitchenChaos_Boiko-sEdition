using Counters;
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
            BaseCounter.OnAnyObjectPlacedHere += BaseCounterOnAnyobjectPlacedHere;
            TrashCounter.OnAnyTrashThrowen += TrashCounterOnAnyTrashThrowen;
            DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagerInstanceOnRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += DeliveryManagerInstanceOnRecipeFailed;
            CuttingCounter.OnAnyCut += CuttingCounterOnAnyCut;

            CountdownTextUI.Instance.OnCountdownNumberChanged += GameStartCountdownUIOnCountdownNumberChanged;

            Player.Instance.OnPickedSomething += PlayerOnPickedSomething;
            Player.Instance.GetComponentInChildren<PlayerSounds>().OnWalking += PlayerOnWalking;
        }

        public void PlayBurnWarningSound(StoveCounter stoveCounter)
        {
            PlaySound(audioClipRefsSO.warning[1], stoveCounter.transform.position);
        }

        private void GameStartCountdownUIOnCountdownNumberChanged(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.warning[0], Vector3.zero);
        }

        private void PlayerOnWalking(object sender, EventArgs e)
        {
            Player player = (Player)sender;
            PlaySound(audioClipRefsSO.footsteps, player.transform.position);
        }

        private void TrashCounterOnAnyTrashThrowen(object sender, EventArgs e)
        {
            TrashCounter trashCounter = sender as TrashCounter;
            PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
        }

        private void BaseCounterOnAnyobjectPlacedHere(object sender, EventArgs e)
        {
            BaseCounter baseCounter = (BaseCounter)sender;
            PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
        }

        private void PlayerOnPickedSomething(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
        }

        private void CuttingCounterOnAnyCut(object sender, EventArgs e)
        {
            CuttingCounter cuttingCounter = (CuttingCounter)sender;
            PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
        }

        private void DeliveryManagerInstanceOnRecipeFailed(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
        }

        private void DeliveryManagerInstanceOnRecipeSuccess(object sender, EventArgs e)
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