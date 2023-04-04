using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFollow : MonoBehaviour
{
    public GameObject obj;
    void Start()
    {
        
    }
    void Update()
    {
        transform.position = obj.transform.position;
    }
}
