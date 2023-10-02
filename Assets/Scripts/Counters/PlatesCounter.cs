using System;
using UnityEngine;

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
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > _spawnDelay)
        {
            _spawnPlateTimer = 0;
            if (_platesSpawnAmount < _platesSpawnAmountMax)
            {
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                _platesSpawnAmount++;
            }
        }
    }
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject() && _platesSpawnAmount > 0)
        {
            _platesSpawnAmount -= 1;
            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            OnPlateTaken?.Invoke(this, EventArgs.Empty);
        }
    }
}
