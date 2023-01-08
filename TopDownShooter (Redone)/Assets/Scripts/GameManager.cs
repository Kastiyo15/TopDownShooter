using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Static Bools")]
    public static bool GameIsPaused = false;
    public static bool IsDead = false;

    [Header("Menu Panels")]
    [SerializeField] private GameObject _panelMaster;
    [SerializeField] private GameObject _panelPauseMenu;
    [SerializeField] private GameObject _panelOptionsMenu;
    [SerializeField] private GameObject _panelGameUI;

    [Header("Scene Strings")]
    [SerializeField] private string _stringMainMenu;
    [SerializeField] private string _stringGameScene;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        IsDead = false;

        LoadJsonData(this);

        // Make Pause menu panels invisible
        Resume();
    }


    // Update is called once per frame
    void Update()
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
            SaveJsonData(this);
        }
    }


    // Set IsDead true when player dies (called in the player Died Event)
    public void PlayerIsDead()
    {
        IsDead = true;
    }













    public void Resume()
    {
        // Game Properties
        _panelMaster.SetActive(false);
        _panelPauseMenu.SetActive(false);
        _panelOptionsMenu.SetActive(false);
        _panelGameUI.SetActive(true);

        Time.timeScale = 1f;
        GameIsPaused = false;
        IsDead = false;

        // Cursor Properties
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }


    public void Pause()
    {
        // Game Properties
        _panelMaster.SetActive(true);
        _panelPauseMenu.SetActive(true);
        _panelGameUI.SetActive(false);

        Time.timeScale = 0f;
        GameIsPaused = true;

        // Cursor Properties
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
