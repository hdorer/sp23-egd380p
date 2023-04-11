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
        Debug.Log("local to world player: " + obj.transform.localToWorldMatrix.GetPosition());
        Debug.Log("World to local player: " + obj.transform.worldToLocalMatrix.GetPosition());
        Debug.Log("Base Position: " + obj.transform.position);
        transform.position = new Vector3(obj.transform.position.x, 6.1f, obj.transform.position.z+ -1.27f);
    }
}
