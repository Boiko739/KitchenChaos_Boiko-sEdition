using KitchenChaos;
using MySOs;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Counters
{
    public class PlatesCounter : BaseCounter
    {
        public event EventHandler OnPlateSpawned;
        public event EventHandler OnPlateTaken;

        [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

        private float _spawnPlateTimer;
        private readonly float _spawnDelay = 4f;
        private int _platesSpawnAmount;
        private readonly int _platesSpawnAmountMax = 4;

        private void Update()
        {
            if (!IsServer)
            {
                return;
            }

            _spawnPlateTimer += Time.deltaTime;
            if (GameManager.Instance.IsGamePlaying() && _spawnPlateTimer > _spawnDelay)
            {
                _spawnPlateTimer = 0;
                if (_platesSpawnAmount < _platesSpawnAmountMax)
                {
                    SpawnPlateServerRpc();
                }
            }
        }

        [ServerRpc]
        private void SpawnPlateServerRpc()
        {
            SpawnPlateClientRpc();
        }

        [ClientRpc]
        private void SpawnPlateClientRpc()
        {
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            _platesSpawnAmount++;
        }

        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject() && _platesSpawnAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                InteractLogicServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void InteractLogicServerRpc()
        {
            InteractLogicClientRpc();
        }

        [ClientRpc]
        private void InteractLogicClientRpc()
        {
            _platesSpawnAmount -= 1;
            OnPlateTaken?.Invoke(this, EventArgs.Empty);
        }
    }
}