using Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenChaos
{
    public class ResetStaticDataManager : MonoBehaviour
    {
        private void Awake()
        {
            BaseCounter.ResetStaticData();
            CuttingCounter.ResetStaticData();
            TrashCounter.ResetStaticData();
        }
    }
}
