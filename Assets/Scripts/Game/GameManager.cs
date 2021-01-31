using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public string Path;                     // Path to where the game data will be saved

    private bool b_pass = false;            // Did the game succeed or fail?
    private int gameScore = 0;
    public GameObject mainMenu;
    public GameObject inGameMenu;
	public GameObject gameScreen;
    public GameObject pauseMenu;
    public GameoverUI gameOverMenu;
	public GameObject crackedGlass;

	public TimelineUI timelineUI;
	public LivesUI livesUI;

    internal static GameManager instance;   // singleton instance
    //Put your game states here
    public enum GAMESTATES
    {
        MAINMENU,
        INIT,
        INGAME,
        PAUSED,
        WIN_CINEMATIC,
        GAMEOVER
    }

    public GAMESTATES gameState = GAMESTATES.INIT;

    public enum LEVELSTATES
	{
		DISPLAY_ITEMSLIST,
        FINDING,
        AFTER_FIND
	}

	public LEVELSTATES levelState = LEVELSTATES.DISPLAY_ITEMSLIST;

    private bool b_gameover;                // Is the game over?

    bool callOnce = true;                   // Used when changing the game state bool for calling function/code once in the game
	bool callOnce2 = true;
    //--------public game fields


    //--------private game fields


    void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        //Initialize class
        instance = this;

    }

    void Start()
    {
        SETPATH();

        // Do necessary initialization here
        // put here the initializations that should not be called when game resets (WE DO NOT RELOAD SCENE WHEN RESETTING GAME)


        //for these to work, EventManager.cs must be on the hierarchy
        EventsManager.OnGameReset.AddListener(OnGameReset);
        EventsManager.OnGamePaused.AddListener(OnGamePaused);
        EventsManager.OnGameOver.AddListener(OnGameOver);
    }

    void OnGameReset()
    {

    }
    void OnGamePaused()
    {

    }
    void OnGameOver()
    {

    }

    #region FSM
    void Update()
    {
        GameFSM();
    }
    void OnEnable()
    {

    }
    void OnDisable()
    {
        EventsManager.OnGameReset.RemoveListener(OnGameReset);
        EventsManager.OnGamePaused.RemoveListener(OnGamePaused);
        EventsManager.OnGameOver.RemoveListener(OnGameOver);
    }
    void GameFSM()
    {
        switch(gameState)
        {
			case GAMESTATES.MAINMENU:
				if(callOnce)
				{
                    mainMenu.SetActive(true);
                    pauseMenu.SetActive(false);
                    gameOverMenu.gameObject.SetActive(false);
                    callOnce = false;
				}

                if (Input.anyKeyDown)
                    ChangeGameState(GAMESTATES.INIT);

                break;
            case GAMESTATES.INIT:
                if(callOnce)
                {
                    // -- Put codes that are needed to be called only once -- //
                    //Do the setup for the game here.

                    mainMenu.SetActive(false);
                    gameOverMenu.gameObject.SetActive(false);

					LostItemManager.instance.CreateItemsList();
					FrameManager.instance.ResetFrames();
					timelineUI.StartTracking();
					livesUI.ResetLives();
					crackedGlass.SetActive(false);

                    //
                    callOnce = false;
                    //change gamestate after running init once
                    ChangeGameState(GAMESTATES.INGAME);
					ChangeLevelState(LEVELSTATES.DISPLAY_ITEMSLIST);
                }
                break;
            case GAMESTATES.INGAME:
                if (callOnce)
                {
                    // -- Put codes that are needed to be called only once -- //
                    pauseMenu.SetActive(false);
                    inGameMenu.SetActive(true);
					gameScreen.SetActive(true);
                    gameOverMenu.gameObject.SetActive(false);
                    //
                    callOnce = false;
                }

                //Game Loop
                Game();
                break;
            case GAMESTATES.PAUSED:
                if (callOnce)
                {
                    // -- Put codes that are needed to be called only once -- //
                    gameOverMenu.gameObject.SetActive(false);
                    pauseMenu.SetActive(true);
                    EventsManager.OnGamePaused.Invoke();

                    //
                    callOnce = false;
                }
                break;
			case GAMESTATES.WIN_CINEMATIC:
				if(callOnce)
				{
					callOnce = false;
				}
				break;
            case GAMESTATES.GAMEOVER:
                if (callOnce)
                {
                    // -- Put codes that are needed to be called only once -- //
                    pauseMenu.SetActive(false);
                    b_gameover = true;

                    StartCoroutine(GameOver());
                    //
                    callOnce = false;
                }


                break;

        }
    }
    public void ChangeGameState(int state)  //for button click event (just in case)
    {
        gameState = (GAMESTATES)state;
        callOnce = true;
    }
    public void ChangeGameState(GAMESTATES state)
    {
        gameState = state;
        callOnce = true;        // Set to true so every time the state change, there's a place to call some code once in the loop
    }

	public void ChangeLevelState(LEVELSTATES state)
	{
		levelState = state;
		callOnce2 = true;
	}
    #endregion

    IEnumerator GameOver()
    {
		if (!b_pass)
			crackedGlass.SetActive(true);
		yield return new WaitForSeconds(1.25f);

		gameOverMenu.SetGameOverBanner(b_pass);
		gameOverMenu.GetComponent<UIObject>().SetActive(true);
    }

    //in-game loop
    void Game()
    {
		if (livesUI.Lives <= 0)
		{
			b_pass = false;
			ChangeGameState(GAMESTATES.GAMEOVER);
		}
        // put updates here for when in in-game state
		switch(levelState)
		{
			case LEVELSTATES.DISPLAY_ITEMSLIST:
				if(callOnce2)
				{

					callOnce2 = false;

					ChangeLevelState(LEVELSTATES.FINDING);
				}
				break;
			case LEVELSTATES.FINDING:
                if (callOnce2)
                {

                    callOnce2 = false;
                }
                break;
			case LEVELSTATES.AFTER_FIND:
                if (callOnce2)
                {

                    callOnce2 = false;
                }
                break;
		}
    }

    void SETPATH()
    {
#if UNITY_EDITOR
        Path = Application.dataPath;
#else
		Path = Application.persistentDataPath;
#endif
    }
    
    public void ResetGame()
    {
        ChangeGameState(GAMESTATES.INIT);
    }

    public bool IsPlayState()
	{
		return gameState == GAMESTATES.INGAME && levelState == LEVELSTATES.FINDING;
	}
}
