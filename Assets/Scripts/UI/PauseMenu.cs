using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseScreen;
    public string mainMenu;

    bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
                PauseGame();
            else
                UnPauseGame();
        }
    }

    void PauseGame()
    {
        gamePaused = true;
        pauseScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    public void UnPauseGame()
    {
        gamePaused = false;
        pauseScreen.SetActive(false);

        Time.timeScale = 1.0f;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
