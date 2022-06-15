using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gamePaused = false;
    public static GamePlayStates gamePlayStates;
    public GameObject optionsMenu;
    public static PlayerMovement player;

    public void Start()
    { 
        player = FindObjectOfType<PlayerMovement>();
        gamePlayStates = GamePlayStates.Game;
    }

    void Update()
    {
        // if the MainMenu is the current scene
        if (SceneManager.GetSceneByBuildIndex(0).isLoaded)
        {
            // if we the play state isn't already menu state
            if (gamePlayStates != GamePlayStates.MenuPause)
            {
                // set to be the menu state, we need to see the cursor
                gamePlayStates = GamePlayStates.MenuPause;
            }
            
            // Escape key does nothing and we cannot execute other code in Update()
            if (Input.GetKeyDown(KeyCode.Escape))
            { return; }
            
            return; // don't run any other code in Update() if we're in the MainMenu scene
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused; // opposite of what it was
            gamePlayStates = gamePaused ? GamePlayStates.MenuPause : GamePlayStates.Game;
        }

        switch (gamePlayStates)
        {
            case GamePlayStates.Game:
                {
                    if (Time.timeScale == 0)    // if we have paused time
                    { Time.timeScale = 1; }     // unpause it

                    if (Cursor.visible) // if the cursor is visible
                    {
                        Cursor.visible = false; // make it invisible
                        Cursor.lockState = CursorLockMode.Locked;   // lock the cursor to the game window
                    }
                    if (optionsMenu.activeSelf)         // if our pause menu is active
                    { optionsMenu.SetActive(false); }   // deactivate it
                    break;
                }
            case GamePlayStates.MenuPause:
                {
                    if (Time.timeScale == 1)    // if we haven't paused time
                    { Time.timeScale = 0; }     // pause it

                    if (!Cursor.visible)    // if the cursor isn't visible
                    {
                        Cursor.visible = true;  // make it visible
                        Cursor.lockState = CursorLockMode.None; // don't lock the cursor to the game window
                    }
                    if (!optionsMenu.activeSelf)        // if the pause menu isn't active
                    { optionsMenu.SetActive(true); }    // activate it
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

    public void SaveGame(int saveIndex_p)
    {
        if (player == null)
        {
            Debug.LogWarning("Cannot find a GameObject with the type of PlayerMovement.");
            return;
        }
        GameSaves.ChooseSlot(saveIndex_p);
        GameSaves.WriteSaveFile(player.gameObject);
    }
    public void LoadGame(int saveIndex_p)
    {
        if (player == null)
        {
            Debug.LogWarning("Cannot find a GameObject with the type of PlayerMovement.");
            return;
        }
        try
        {
            GameSaves.ChooseSlot(saveIndex_p);
            GameSaves.ReadSaveFile(player.gameObject);
        }
        catch (System.Exception)
        {
            return;
        }
    }

    public void NewGame()
    {
        PlayerMovement.playerPos = new Vector3(0, -4, 0);   // reset player position
        PlayerMovement.playerRot = Vector3.zero;            // reset player rotation
    }

    public void PauseGame()
    {
        gamePaused = !gamePaused; // opposite of what it was
        gamePlayStates = gamePaused ? GamePlayStates.MenuPause : GamePlayStates.Game;
    }
}

public enum GamePlayStates
{
    Game,
    MenuPause
}