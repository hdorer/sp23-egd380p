using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    Grid grid;
    Dictionary<Vector3Int, GameObject> gridPts = new();

    public int numRooms = 9;
    public List<TileData> placedTiles;
    int index = 0;

    [Header("Hallway")]
    public GameObject hallway;

    [Header("Small Rooms")]
    public List<GameObject> smallRooms;

    [Header("Medium Rooms")]
    public List<GameObject> mediumRooms;

    [Header("Large Rooms")]
    public List<GameObject> largeRooms;
    

    void Awake()
    {
        grid = GetComponent<Grid>();
        gridPts.Add(Vector3Int.FloorToInt(placedTiles[0].transform.position), placedTiles[0].gameObject);
        //GenerateLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && placedTiles.Count < numRooms)
            GenerateLevel();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void GenerateLevel()
    {
        int loopBreak = 0;
        int connection;
        bool validTile = false;

        do
        {
            if (placedTiles[index].connections.Count == 0)
                index--;

            if (index <= 0)
                index = placedTiles.Count - 1;

            connection = Random.Range(0, placedTiles[index].connections.Count - 1);
            Vector3Int tmp = grid.WorldToCell(placedTiles[index].connections[connection].alignPt.position);
            
            switch (placedTiles[index].connections[connection].direction)
            {
                case dir.right:
                    Debug.Log("Right");
                    validTile = CreateTile(new Vector3Int(tmp.x + 1, tmp.z));
                    break;
                case dir.left:
                    Debug.Log("Left");
                    validTile = CreateTile(new Vector3Int(tmp.x - 1, tmp.z));
                    break;
                case dir.front:
                    Debug.Log("Front");
                    validTile = CreateTile(new Vector3Int(tmp.x, tmp.z + 1));
                    break;
                case dir.back:
                    Debug.Log("Back");
                    validTile = CreateTile(new Vector3Int(tmp.x, tmp.z - 1));
                    break;
            }

            loopBreak++;
            if (loopBreak > 100)
            {
                Debug.Log("Infinite Loop!!");
                Debug.Break();
            }

        } while (!validTile);
    }

    bool CreateTile(Vector3Int pos)
    {
        if (gridPts.ContainsKey(pos))
        {
            index--;
            return false;
        }

        GameObject newTile = Instantiate(hallway);
        newTile.transform.position = grid.CellToWorld(pos);
        gridPts[pos] = newTile.gameObject;
        placedTiles.Add(newTile.GetComponent<TileData>());
        index = placedTiles.Count - 1;
        //Debug.Log("Index: " + index + " Pos: " + pos);

        return true;
    }
}
