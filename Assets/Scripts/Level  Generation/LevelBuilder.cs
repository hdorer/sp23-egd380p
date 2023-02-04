using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GenerateLevel();
    }

    private void Update()
    {
        if (placedTiles.Count < numRooms)
            GenerateLevel();
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
                    validTile = CreateTile(new Vector3Int(tmp.x + 5, tmp.z));
                    break;
                case dir.left:
                    validTile = CreateTile(new Vector3Int(tmp.x - 5, tmp.z));
                    break;
                case dir.front:
                    validTile = CreateTile(new Vector3Int(tmp.x, tmp.z + 5));
                    break;
                case dir.back:
                    validTile = CreateTile(new Vector3Int(tmp.x, tmp.z - 5));
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
        Debug.Log(pos);
        if (gridPts.ContainsKey(pos))
            return false;

        GameObject tmp = new GameObject();
        tmp.transform.position = grid.CellToWorld(pos);
        gridPts[pos] = tmp;

        return true;
    }
}
