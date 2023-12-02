using UnityEngine;

namespace KitchenChaos
{
    public class LoaderCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;
        void Update()
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;

                Loader.LoaderCallback();
            }
        }
    }
}