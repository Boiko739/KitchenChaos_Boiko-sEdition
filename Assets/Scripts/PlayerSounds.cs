using System;
using UnityEngine;

namespace KitchenChaos
{
    public class PlayerSounds : MonoBehaviour
    {
        public event EventHandler OnWalking;

        private float footstepTimer;
        private readonly float footstepTimerMax = .1f;

        private void Update()
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepTimerMax)
            {
                footstepTimer = 0f;
                if (Player.Instance.IsWalking())
                {
                    OnWalking?.Invoke(Player.Instance, EventArgs.Empty);
                }
            }
        }
    }
}