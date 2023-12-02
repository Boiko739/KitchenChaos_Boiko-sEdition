using Counters;
using UnityEngine;

namespace KitchenChaos
{
    public class ResetStaticDataManager : MonoBehaviour
    {
        private void Awake()
        {
            Player.ResetStaticData();
            BaseCounter.ResetStaticData();
            CuttingCounter.ResetStaticData();
            TrashCounter.ResetStaticData();
        }
    }
}