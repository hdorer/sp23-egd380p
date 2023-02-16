using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    Grid grid;
    Dictionary<Vector3Int, GameObject> gridPts = new();

    public int numRooms = 9;
    public int startSeperation;
    public int tileShift;
    public List<TileData> placedTiles;
    int index = 0;

    [Header("Hallway")]
    public GameObject hallway;

    public List<GameObject> placeableTiles;

    private PathFinding pathFinding = new PathFinding();
    private bool overlapChecked = false;
    private bool hallwaysGen = true;


    void Awake()
    {
        grid = GetComponent<Grid>();
        gridPts.Add(Vector3Int.FloorToInt(placedTiles[0].transform.position), placedTiles[0].gameObject);
    }

    //private void LateUpdate()
    //{
    //    if (hallwaysGen == false && overlapChecked == true)
    //    {
    //        GenerateHallways();
    //        Debug.Break();
    //    }
    //}

    private void Update()
    {
        if (placedTiles.Count < numRooms && placeableTiles.Count != 0)
            GenerateLevel();
        else if (overlapChecked == false)
            CheckOverlap();
        else if (hallwaysGen == false && overlapChecked == true)
        {
            GenerateHallways();
            Debug.Break();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //foreach (TileData tile in placedTiles)
        //{
        //    foreach (Connection con in tile.connections)
        //    {
        //        if (con.connection != null)
        //            Debug.DrawLine(con.alignPt.position, con.connection.position, Color.red);
        //    }
        //}
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

            connection = Random.Range(0, placedTiles[index].connections.Count);
            Vector3Int tmp = grid.WorldToCell(placedTiles[index].connections[connection].alignPt.position);
            tmp.x += Random.Range(-tileShift, tileShift + 1);
            tmp.y += Random.Range(-tileShift, tileShift + 1);

            switch (placedTiles[index].connections[connection].direction)
            {
                case dir.right:
                    tmp = new Vector3Int(tmp.x + startSeperation, tmp.y);
                    break;
                case dir.left:
                    tmp = new Vector3Int(tmp.x - startSeperation, tmp.y);
                    break;
                case dir.front:
                    tmp = new Vector3Int(tmp.x, tmp.y + startSeperation);
                    break;
                case dir.back:
                    tmp = new Vector3Int(tmp.x, tmp.y - startSeperation);
                    break;
            }

            validTile = CreateTile(tmp, placedTiles[index].connections[connection].direction, 
                                    placedTiles[index].connections[connection]);

            loopBreak++;
            if (loopBreak > 100)
            {
                Debug.Log("Infinite Loop!!");
                Debug.Break();
            }

        } while (!validTile);
    }

    bool CreateTile(Vector3Int pos, dir direction, Connection connector)
    {
        if (gridPts.ContainsKey(pos) || connector.connection != null)
        {
            index--;
            return false;
        }
        
        int tileChoice = Random.Range(0, placeableTiles.Count);
        GameObject newTile = Instantiate(placeableTiles[tileChoice], new Vector3(0, 10, 0), transform.rotation);
        placeableTiles.Remove(placeableTiles[tileChoice]);
        newTile.transform.position = grid.CellToWorld(pos);
        gridPts[pos] = newTile.gameObject;
        placedTiles.Add(newTile.GetComponent<TileData>());
        index = placedTiles.Count - 1;

        foreach (Connection con in placedTiles[index].connections)
        {
            if ((con.direction == dir.right && direction == dir.left) ||
                (con.direction == dir.left && direction == dir.right) ||
                (con.direction == dir.front && direction == dir.back) ||
                (con.direction == dir.back && direction == dir.front))
            {
                con.connection = connector.alignPt;
                connector.connection = con.alignPt;
                break;
            }

        }
        return true;
    }

    private void CheckOverlap()
    {
        overlapChecked = true;
        hallwaysGen = false;
        foreach (TileData tile in placedTiles)
        {
            if (tile.overlap != null)
            {
                overlapChecked = false;
                hallwaysGen = true;
                Vector3Int gridPt = grid.WorldToCell(tile.transform.position);
                Vector3Int[] neighborGridpts = new Vector3Int[8] {new Vector3Int(gridPt.x + 1, gridPt.y),
                                                                    new Vector3Int(gridPt.x + 1, gridPt.y + 1),
                                                                    new Vector3Int(gridPt.x, gridPt.y + 1),
                                                                    new Vector3Int(gridPt.x - 1, gridPt.y + 1),
                                                                    new Vector3Int(gridPt.x - 1, gridPt.y),
                                                                    new Vector3Int(gridPt.x - 1, gridPt.y - 1),
                                                                    new Vector3Int(gridPt.x, gridPt.y - 1),
                                                                    new Vector3Int(gridPt.x + 1, gridPt.y - 1),
                                                                    };

                float dist = 0, tmp;

                for (int i = 0; i < neighborGridpts.Length; i++)
                {
                    tmp = Vector3Int.Distance(neighborGridpts[i], grid.WorldToCell(tile.overlap.transform.position));
                    if (tmp > dist)
                    {
                        dist = tmp;
                        tile.transform.position = Vector3Int.FloorToInt(grid.CellToWorld(neighborGridpts[i]));
                    }
                }
            }
        }
    }

    private void GenerateHallways()
    {
        hallwaysGen = true;

        Dictionary<Vector3Int, bool> visited = new Dictionary<Vector3Int, bool>();

        foreach (TileData tile in placedTiles)
        {
            foreach (Connection con in tile.connections)
            {
                if (con.connection != null && visited.ContainsKey(grid.WorldToCell(con.connection.position)) != true)
                {
                    visited.Add(grid.WorldToCell(con.alignPt.position), true);
                    List<Vector3Int> path = pathFinding.GeneratePath(placedTiles, grid, grid.WorldToCell(con.alignPt.position), grid.WorldToCell(con.connection.transform.position));

                    foreach (Vector3Int p in path)
                    {
                        GameObject tmp = Instantiate(hallway);
                        tmp.transform.position = grid.CellToWorld(p);
                    }
                }
            }
        }
    }
}
