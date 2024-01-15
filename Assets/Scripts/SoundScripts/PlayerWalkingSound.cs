using Unity.Netcode;
using UnityEngine;

namespace OtherScripts
{
    public class PlayerWalkingSound : NetworkBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private AudioClip[] footsteps;

        private readonly float footstepTimerMax = .1f;
        private float footstepTimer;

        private void Update()
        {
            if (player.IsWalking)
            {
                footstepTimer += Time.deltaTime;
                if (footstepTimer >= footstepTimerMax)
                {
                    footstepTimer = 0f;

                    AudioSource.PlayClipAtPoint(footsteps[Random.Range(0, footsteps.Length)],
                        player.transform.position, SoundManager.Instance.Volume);
                }
            }
        }
    }
}