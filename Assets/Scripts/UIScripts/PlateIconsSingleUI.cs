using MySOs;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class PlateIconsSingleUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
        {
            image.sprite = kitchenObjectSO.sprite;
        }
    }
}