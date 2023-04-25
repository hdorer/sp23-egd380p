using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    public float activateRange;
    public GameObject text;
    private GameObject player;

    private void Awake()
    {
        player = FindObjectOfType<MovementScript>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < activateRange)
            text.SetActive(true);
        else
            text.SetActive(false);
    }
}
