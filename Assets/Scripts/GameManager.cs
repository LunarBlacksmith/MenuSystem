using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gamePaused = false;
    public static GamePlayStates gamePlayStates;
    public GameObject optionsMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log($"Options Menu active: {optionsMenu.activeInHierarchy}");
            gamePaused = !gamePaused; // opposite of what it was
            gamePlayStates = gamePaused ? GamePlayStates.MenuPause : GamePlayStates.Game;
            Debug.Log($"Gameplay state: {gamePlayStates}");
        }

        switch (gamePlayStates)
        {
            case GamePlayStates.Game:
               { 
                    if (Cursor.visible)
                    {
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                        Debug.Log("Debug 31");
                        optionsMenu.SetActive(false);
                        Debug.Log("Debug 33");
                    }
                    break;
               }
            case GamePlayStates.MenuPause:
                {
                    if (!Cursor.visible)
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        Debug.Log("Debug 43");
                        optionsMenu.SetActive(true);
                        Debug.Log("Debug 45");
                    }
                    break;
                }
            default:
                {
                    if (!Cursor.visible)
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                    break;
                }
        }
    }

}

public enum GamePlayStates
{
    Game,
    MenuPause
}
