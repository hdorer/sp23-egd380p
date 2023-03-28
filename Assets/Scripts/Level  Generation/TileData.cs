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
    //[HideInInspector]
    public Collider overlap = null;
    public List<GameObject> enemies;
    public BoxCollider[] boxColliders;
    public SlidingDoor[] doors;

    GameObject player;

    private void Start()
    {
        player = FindObjectOfType<MovementScript>().gameObject;
        foreach (SlidingDoor door in doors)
        {
            door.room = this;
            door.player = player.transform;
        }
    }

    private void Update()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
            if (enemies[i] == null)
                enemies.Remove(enemies[i]);

        if (enemies.Count <= 0)
            foreach (SlidingDoor door in doors)
                door.locked = false;
        //else
        //    foreach (SlidingDoor door in doors)
        //        door.locked = true;
    }

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

    public Vector3 CheckContains(List<TileData> tiles)
    {
        foreach (TileData tile in tiles)
        {
            foreach (Connection c in connections)
            {
                if (tile.CheckContains(c.alignPt.position + Vector3.up))
                    return c.alignPt.position;
            }
        }
        
        //return up if no point is found
        return Vector3.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.parent.name + " " + gameObject.name);
        overlap = other;

        if (other.CompareTag("Player") && enemies.Count > 0)
            foreach (SlidingDoor door in doors)
            {
                door.locked = true;
                GetComponent<EnableEnemies>().enemies.SetActive(true);
            }
    }
    private void OnTriggerStay(Collider other)
    {
        overlap = other;
    }

    private void OnTriggerExit(Collider other)
    {
        overlap = null;
    }
}
