using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] BaseCounter baseCounter;
    [SerializeField] GameObject[] visualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged; ;
    }

    private void PlayerOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (var visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true); 
        }
    }

    private void Hide()
    {
        foreach (var visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}