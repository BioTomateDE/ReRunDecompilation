using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public static UIManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!GameManager.Instance.playerDead && GameManager.Instance.playing)
        {
            if (pauseMenu.activeInHierarchy)
            {
                HidePause();
            }
            else
            {
                ShowPause();
            }
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
            GameManager.Instance.paused = pauseMenu.activeInHierarchy;
        }
    }

    public void HidePause()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowPause()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
