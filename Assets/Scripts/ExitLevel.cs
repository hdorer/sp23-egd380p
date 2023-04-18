using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitLevel : MonoBehaviour
{
    public int playerLayer;
    private LevelLoading loader;
    private Image fadeOut;

    private void Awake()
    {
        loader = FindObjectOfType<LevelLoading>();
        fadeOut = FindObjectOfType<FadeOut>().fadeImage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        // loop over 1 second backwards
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            fadeOut.color = new Color(1, 1, 1, i);
            yield return null;
        }

        loader.LoadNextLevel();
    }
}
