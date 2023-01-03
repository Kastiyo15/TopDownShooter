using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool _dead = false;


    [Header("References")]
    [SerializeField] private GameObject _panelMainMenu;
    [SerializeField] private GameObject _panelOptionsMenu;


    private void Start()
    {
        _panelMainMenu.SetActive(true);
        _panelOptionsMenu.SetActive(false);
    }


    public void NewGame()
    {
        Debug.Log("Starting New Game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void ContinueGame()
    {
        Debug.Log("Continuing Game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }


    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}