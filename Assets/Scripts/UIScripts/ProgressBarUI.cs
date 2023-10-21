using KitchenChaos;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private GameObject hasProgressGameObject;
        [SerializeField] private Image barImage;

        private IHasProgress hasProgress;

        public void Start()
        {
            if (!hasProgressGameObject.TryGetComponent(out hasProgress))
            {
                Debug.LogError("hasProgress is null! Change the reference to the hasProgressGameObject!");
            }
            else
            {
                hasProgress.OnProgressChanged += HasProgressOnProgressBarChanged;
                barImage.fillAmount = 0f;

                gameObject.SetActive(false);
            }
        }

        private void HasProgressOnProgressBarChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
        {
            barImage.fillAmount = e.ProgressNormalized;

            gameObject.SetActive(barImage.fillAmount is not (0 or 1));
        }
    }
}