using OtherScripts;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class TestingCharacterSelectUI : MonoBehaviour
    {
        [SerializeField] private Button readyButton;

        private void Awake()
        {
            readyButton.onClick.AddListener(() =>
            {
                CharacterSelectReady.Instance.SetPlayerReady();
            });
        }
    }
}