using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSystem : MonoBehaviour
{
    public void SwitchSceneNum(int n)
    {
        SceneSwitcher.GoToScene(n);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
