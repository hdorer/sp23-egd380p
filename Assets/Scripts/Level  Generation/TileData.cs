using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dir { left, right, front, back };

[System.Serializable]
public class Connection
{
    public Transform alignPt;
    public GameObject wall;
    public dir direction;
    [HideInInspector]
    public Transform connection;
}

public class TileData : MonoBehaviour
{
    public List<Connection> connections;
    [HideInInspector]
    public Collider overlap = null;
    public BoxCollider[] boxColliders;

    private void OnDrawGizmos()
    {
        foreach (BoxCollider col in boxColliders)
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }

    public bool CheckContains(Vector3 pt)
    {
        foreach (BoxCollider col in boxColliders)
        {
            if (col.bounds.Contains(pt) == true)
                return true;
        }

        return false;
    }

    public bool CheckContains(List<TileData> tiles)
    {
        foreach (TileData tile in tiles)
        {
            foreach (Connection c in connections)
            {
                if (tile.CheckContains(c.alignPt.position))
                    return true;
            }
        }

        return false;
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
