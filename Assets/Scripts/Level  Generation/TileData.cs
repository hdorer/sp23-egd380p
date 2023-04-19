using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    public SlidingDoor door;
}

public class TileData : MonoBehaviour
{
    public List<Connection> connections = new List<Connection>();
    [HideInInspector]
    public Transform overlap = null;
    public List<GameObject> spawnableEnemies;
    public List<Transform> enemySpawns;
    private List<GameObject> enemies = new List<GameObject>();
    public BoxCollider[] boxColliders;

    GameObject player;

    private void Start()
    {
        player = FindObjectOfType<MovementScript>().gameObject;

        foreach (Connection con in connections)
        {
            if (con.door == null)
                continue;

            con.door.room = this;
            con.door.player = player.transform;
        }
    }

    private void Update()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
            if (enemies[i] == null)
                enemies.Remove(enemies[i]);

        if (enemies.Count <= 0)
            foreach (Connection con in connections)
            {
                if (con.door == null)
                    continue;

                con.door.locked = false;
                con.door.col.enabled = false;
            }
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

    public void LoadEnemies()
    {
        int enemy;

        if (spawnableEnemies.Count > 0)
        {
            foreach (Transform pt in enemySpawns)
            {
                enemy = Random.Range(0, spawnableEnemies.Count);
                enemies.Add(Instantiate(spawnableEnemies[enemy], pt.position, Quaternion.identity));
                enemies[enemies.Count - 1].transform.parent = this.transform;
                enemies[enemies.Count - 1].gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.parent.name + " " + gameObject.name);
        overlap = other.transform.root;

        TileData tile = other.GetComponent<TileData>();
        if (tile != null)
            tile.overlap = this.transform.root;

        if (other.CompareTag("Player") && enemies.Count > 0)
        {
            foreach (Connection con in connections)
            {
                con.door.locked = true;
                con.door.col.enabled = true;
            }

            foreach (GameObject go in enemies)
                go.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        overlap = other.transform.root;

        TileData tile = other.GetComponent<TileData>();
        if (tile != null)
            tile.overlap = this.transform.root;
    }

    private void OnTriggerExit(Collider other)
    {
        overlap = null;
    }
}
