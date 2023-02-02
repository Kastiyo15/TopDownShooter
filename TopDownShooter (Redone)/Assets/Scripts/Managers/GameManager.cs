using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Static Bools")]
    public bool GameIsPaused = false;
    public bool IsDead = false;
    public bool EnteredFirstRoom = false; // Set this to true when player wnters first


    [Header("Menu Panels")]
    [SerializeField] private GameObject _panelMaster;
    [SerializeField] private GameObject _panelPauseMenu;
    [SerializeField] private GameObject _panelOptionsMenu;
    [SerializeField] private GameObject _panelGameUI;
    [SerializeField] private GameObject _panelDeathScreen;

    [Header("Scene Strings")]
    [SerializeField] private string _stringMainMenu;
    [SerializeField] private string _stringGameScene;

    [Header("References")]
    [SerializeField] private Crosshair _scriptCrosshair;
    [SerializeField] private Volume _globalVolume;
    private VolumeProfile _profile;

    public bool TimerActive = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _profile = _globalVolume.sharedProfile;
    }


    // Start is called before the first frame update
    private void Start()
    {
        SwitchVignette(true);

        IsDead = false;
        EnteredFirstRoom = false;

        // Load all saved data into the SaveData variables
        LoadJsonData(this);

        // Make Pause menu panels invisible
        Resume();

        // Add to runs counter
        CareerStatsManager.Instance.UpdateCareerStatsVariable(CareerStatsManager.VariableType.Runs);
        // Update the Level data classes and the XP bars
        LevelManager.Instance.StartLevelManager();
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsDead)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerIsDead();
        }


        // When timer active, add to the time value
        if (TimerActive && EnteredFirstRoom)
        {
            CareerStatsManager.Instance.UpdateStopWatch();
            ScoreStatsManager.Instance.AddTimerscore();
        }
    }


    // Set IsDead true when player dies (called in the player Died Event)
    public void PlayerIsDead()
    {
        SwitchVignette(false);

        Time.timeScale = 0f;
        GameIsPaused = false;
        IsDead = true;
        TimerActive = false;

        // Update Career Stats
        CareerStatsManager.Instance.UpdateCareerStatsVariable(CareerStatsManager.VariableType.PlayerDeaths);
        // Update the PlayerStatsManager Level data with current Level data
        LevelManager.Instance.UpdateSavedLevelData();
        // Update the run score with timer score
        ScoreStatsManager.Instance.t_runScore += ScoreStatsManager.Instance.t_timerScore;

        // Cursor Properties
        _scriptCrosshair.HideCrosshair();


        SaveJsonData(this);


        _panelPauseMenu.SetActive(false);
        _panelOptionsMenu.SetActive(false);
        _panelDeathScreen.SetActive(true);
        _panelGameUI.SetActive(false);
    }


    private void SwitchVignette(bool boolean)
    {
        if (!_profile.TryGet<Vignette>(out var vignette))
        {
            vignette = _profile.Add<Vignette>(false);
        }
        vignette.active = boolean;
    }


    public void Resume()
    {
        // Game Properties
        _panelMaster.SetActive(false);
        _panelPauseMenu.SetActive(false);
        _panelOptionsMenu.SetActive(false);
        _panelDeathScreen.SetActive(false);
        _panelGameUI.SetActive(true);

        Time.timeScale = 1f;
        GameIsPaused = false;
        IsDead = false;

        TimerActive = true;

        // Cursor Properties
        _scriptCrosshair.ShowCrosshair();
    }


    public void Pause()
    {
        // Game Properties
        _panelMaster.SetActive(true);
        _panelPauseMenu.SetActive(true);
        _panelGameUI.SetActive(false);

        Time.timeScale = 0f;
        GameIsPaused = true;

        TimerActive = false;

        // Cursor Properties
        _scriptCrosshair.HideCrosshair();

    }


    public void Restart()
    {
        Debug.Log("Restarting...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void SkipTutorial()
    {
        Debug.Log("Skipping Tutorial...");
        SceneManager.LoadScene(_stringMainMenu);
    }


    public void ExitToTitle()
    {
        Debug.Log("Loading Menu...");
        SceneManager.LoadScene(_stringMainMenu);
    }


    // Don't save data if quit before dying
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }



    //////////////////////////////////////////////////////////////////
    // SAVING AND LOADING //
    //////////////////////////////////////////////////////////////////

    public static void SaveJsonData(GameManager a_GameManager)
    {
        SaveData sd = new SaveData();
        a_GameManager.PopulateSaveData(sd);

        if (FileManager.WriteToFile("SaveData.dat", sd.ToJson()))
        {
            Debug.Log("Save Successful");
        }
    }


    public void PopulateSaveData(SaveData a_SaveData)
    {
        // Save Player Stats
        PlayerStatsManager.Instance.PopulateSaveData(a_SaveData);
    }



    public static void LoadJsonData(GameManager a_GameManager)
    {
        if (FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            a_GameManager.LoadFromSaveData(sd);
            Debug.Log("Load Complete");
        }
    }


    public void LoadFromSaveData(SaveData a_SaveData)
    {
        // Load Player Stats
        PlayerStatsManager.Instance.LoadFromSaveData(a_SaveData);
    }

    //////////////////////////////////////////////////////////////////
    // SAVING AND LOADING //
    //////////////////////////////////////////////////////////////////
}
