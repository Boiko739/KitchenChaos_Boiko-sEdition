using System.Collections.Generic;
using UnityEngine;

namespace Counters
{
    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] private PlatesCounter platesCounter;
        [SerializeField] private Transform counterTopPoint;
        [SerializeField] private Transform plateVisualPrefab;

        private List<GameObject> spawnedPlatesVisual;

        private void Awake()
        {
            spawnedPlatesVisual = new List<GameObject>();
        }

        private void Start()
        {
            platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
            platesCounter.OnPlateTaken += PlatesCounter_OnPlateTaken;
        }

        private void PlatesCounter_OnPlateTaken(object sender, System.EventArgs e)
        {
            GameObject plate = spawnedPlatesVisual[^1];
            spawnedPlatesVisual.Remove(plate);
            Destroy(plate);
        }

        private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
        {
            Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
            float plateOffsetY = 0.1f;
            plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * spawnedPlatesVisual.Count, 0);
            spawnedPlatesVisual.Add(plateVisualTransform.gameObject);
        }
    }
}