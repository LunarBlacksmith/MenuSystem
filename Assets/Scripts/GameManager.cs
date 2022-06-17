using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GamePlayStates gamePlayStates;
    public bool gamePaused = false;
    public GameObject optionsMenu;
    public Transform player;
    public bool isNewGame = false;
    
    public void Start()
    {
        // make Game state the default state
        gamePlayStates = GamePlayStates.Game;
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    void Update()
    {
        // if it's a new game
        if (isNewGame)
        {
            // and we can find the player
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                // reset their actual position in the engine
                player.position = PlayerMovement.playerPos;
                player.rotation = PlayerMovement.playerRot;
                // change our boolean to not a new game (the opposite of what it was)
                isNewGame = !isNewGame;
            }
        }

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

#if UNITY_EDITOR
        // because Unity stupid, need to use P key instead of Escape
        if (Input.GetKeyDown(KeyCode.P))
        {
            gamePaused = !gamePaused; // opposite of what it was
            gamePlayStates = gamePaused ? GamePlayStates.MenuPause : GamePlayStates.Game;
        }
#endif
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
        // change the save slot path according to the index passed
        GameSaves.ChooseSlotSave(saveIndex_p);
        // then write a text save file to the static file path that was just changed
        GameSaves.WriteSaveFile(player);
    }
    public void LoadGame(int saveIndex_p)
    {
        try
        {
            // if we call this method with save index of -1
            if (saveIndex_p == -1)
            {
                try
                {
                    // we want to load the last save slot we saved to, not pass it a specific save slot
                    GameSaves.ChooseLastSave();
                    return;
                }
                catch (System.Exception)
                { return; }
            }

            // here we change the static path we load from according to the index we pass
            GameSaves.ChooseSlotLoad(saveIndex_p);
        }
        catch (System.Exception)
        { return; }
    }

    public void NewGame()
    {
        // reset the static player variables
        PlayerMovement.playerPos = new Vector3(0, -4, 0);
        PlayerMovement.playerRot = Quaternion.identity;
        // tell the game that it is a new game so we can reset in-game player position/rotation
        isNewGame = true;
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