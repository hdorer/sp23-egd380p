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
    public GameObject sphere;
    [HideInInspector]
    public Collider overlap = null;
    [HideInInspector]
    public BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnDrawGizmos()
    {
        if (boxCollider != null)
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
    }

    public bool CheckContains(Vector3 pt)
    {
        if (boxCollider.bounds.Contains(pt) == true)
        {
            //GameObject tmp = Instantiate(sphere);
            //tmp.transform.position = pt;
            //Debug.Log(pt);
            return true;
        }
        else
        {
            GameObject tmp = Instantiate(sphere);
            tmp.transform.position = pt;
            return false;
        }
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
