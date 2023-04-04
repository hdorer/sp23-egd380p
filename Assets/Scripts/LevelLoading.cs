using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoading : MonoBehaviour
{
    public string[] levels;
    private int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadNextLevel()
    {
        if (currentLevel <= levels.Length - 1)
        {
            currentLevel++;
            SceneManager.LoadScene(levels[currentLevel - 1]);
        }
    }
}
