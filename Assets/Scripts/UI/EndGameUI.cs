using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private Image endGamePanel;
    [SerializeField] private Button toTitleButton;
    [SerializeField] private TextMeshProUGUI resultText;

    private Color32 loseColor = new Color32(217, 107, 89, 175);
    private Color32 winColor = new Color32(158, 224, 137, 175);
    void Start()
    {
        endGamePanel.gameObject.SetActive(false);
        UnitManager.Instance.OnGameResult += OnGameResult;
        toTitleButton.onClick.AddListener(ToTitle);
    }

    void OnGameResult(object sender, EventArgs args)
    {
        GameEndArgs gameEndArgs = (GameEndArgs)args;
        
        if (gameEndArgs != null && gameEndArgs.IsWin)
        {
            endGamePanel.color = winColor;
            resultText.text = "YOU WIN";
        }
        else
        {
            endGamePanel.color = loseColor;
            resultText.text = "YOU LOSE";
        }

        endGamePanel.gameObject.SetActive(true);
    }

    protected virtual void ToTitle()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
