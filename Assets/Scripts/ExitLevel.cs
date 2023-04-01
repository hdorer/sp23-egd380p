using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    public int playerLayer;
    private LevelLoading loader;

    private void Awake()
    {
        loader = FindObjectOfType<LevelLoading>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
            loader.LoadNextLevel();
    }
}
