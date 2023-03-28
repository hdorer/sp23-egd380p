using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public bool random = true;
    public int seed;

    // Start is called before the first frame update
    void Start()
    {
        if (random)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
            Debug.Log("Seed: " + seed);
        }

        Random.InitState(seed);
    }
}
