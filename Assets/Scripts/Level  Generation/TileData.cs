using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dir { left, right, front, back };

[System.Serializable]
public class Connection
{
    public Transform alignPt;
    public dir direction;
    [HideInInspector]
    public Transform connection;
}

public class TileData : MonoBehaviour
{
    public List<Connection> connections;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
