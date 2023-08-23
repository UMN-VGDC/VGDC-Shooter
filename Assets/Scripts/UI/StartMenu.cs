using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{

    [SerializeField] private GameObject[] menuObjects;
    [SerializeField] private GameObject startMenu;
    public void StartGame()
    {
        GameManager.Instance.UpdateGameState(GameState.Shooting);
        startMenu.SetActive(false);
    }

    public void SettingsMenu() => SetMenu(1);
    public void CreditsMenu() => SetMenu(2);
    public void ExitMenu() => SetMenu(3);
    public void Back() => SetMenu(0);
    private void SetMenu(int index)
    {
        for (int i = 0; i < menuObjects.Length; i++)
        {
            menuObjects[i].SetActive(i == index);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
