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

    [HideInInspector]
    public Collider overlap = null;
    [HideInInspector]
    public BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public bool CheckContains(Vector3 pt)
    {
        return boxCollider.bounds.Contains(pt);
    }

    private void OnTriggerEnter(Collider other)
    {
        overlap = other;
    }

    private void OnTriggerExit(Collider other)
    {
        overlap = null;
    }
}
