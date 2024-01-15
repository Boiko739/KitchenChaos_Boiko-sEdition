using Counters;
using UnityEngine;

namespace OtherScripts
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