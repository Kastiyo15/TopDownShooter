using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Statuc Bools")]
    public static bool GameIsPaused = false;
    public static bool IsDead = false;

    [Header("Menu Panels")]
    [SerializeField] private GameObject _panelPauseMenu;
    [SerializeField] private GameObject _panelOptionsMenu;

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
    }


    public void Resume()
    {
        // Game Properties
        _panelPauseMenu.SetActive(false);
        _panelOptionsMenu.SetActive(false);

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
        _panelPauseMenu.SetActive(true);

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

        // Save Weapon Stats
        WeaponStatsManager.Instance.PopulateSaveData(a_SaveData);

        // Save Bullet Stats
        BulletStatsManager.Instance.PopulateSaveData(a_SaveData);
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

        // Load Weapon Stats
        WeaponStatsManager.Instance.LoadFromSaveData(a_SaveData);

        // Load Bullet Stats
        BulletStatsManager.Instance.LoadFromSaveData(a_SaveData);
    }
    //////////////////////////////////////////////////////////////////
    // SAVING AND LOADING //
    //////////////////////////////////////////////////////////////////
}
