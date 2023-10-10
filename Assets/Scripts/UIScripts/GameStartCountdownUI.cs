using KitchenChaos;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class GameCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
        gameObject.SetActive(false);
    }

    private void GameManagerOnStateChanged(object sender, System.EventArgs e)
    {
        gameObject.SetActive(GameManager.Instance.IsCountdownToStart());
    }

    private void Update()
    {
        countdownText.text = Math.Ceiling(GameManager.Instance.GetCountdownToStartTimer()).ToString();
    }
}
