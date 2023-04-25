using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitLevel : MonoBehaviour
{
    private GameObject player;

    private LevelLoading loader;
    private Image fadeOut;

    private void Awake()
    {
        player = FindObjectOfType<MovementScript>().gameObject;
        loader = FindObjectOfType<LevelLoading>();
        fadeOut = FindObjectOfType<FadeOut>().fadeImage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        // loop over 1 second backwards
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            fadeOut.color = new Color(fadeOut.color.r, fadeOut.color.g, fadeOut.color.b, i);
            yield return null;
        }

        loader.LoadNextLevel();
    }
}
