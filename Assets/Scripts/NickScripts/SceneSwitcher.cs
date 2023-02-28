using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher
{
    static public void GoToScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
