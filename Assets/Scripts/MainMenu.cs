using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu UI Elements")]
    [SerializeField] private RectTransform mainMenuPanel;
    [SerializeField] private Button singlePlayBtn;
    [SerializeField] private Button multiPlayBtn;
    [SerializeField] private Button exitBtn;

    [Header("Lobby Menu UI Elements")]
    [SerializeField] private RectTransform lobbyListPanel;

    private void Start()
    {
        AssignButtonCallback();

        mainMenuPanel.gameObject.SetActive(true);
        lobbyListPanel.gameObject.SetActive(false);
    }

    void AssignButtonCallback()
    {
        singlePlayBtn.onClick.AddListener(LoadGame);
        multiPlayBtn.onClick.AddListener(OpenLobbyMenu);
        exitBtn.onClick.AddListener(ExitGame);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("MainScene");    
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void OpenLobbyMenu()
    {
        mainMenuPanel.gameObject.SetActive(false);
        lobbyListPanel.gameObject.SetActive(true);
    }
}
