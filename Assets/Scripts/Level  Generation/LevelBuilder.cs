using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    Grid grid;
    Dictionary<Vector3Int, GameObject> gridPts = new();

    public int numRooms = 9;
    public int startSeperation;
    public int tileShift;
    public float pauseDelay;
    public List<TileData> placedTiles;
    public List<Hallways> hallways;
    //public GameObject player;
    private GameObject placedTilesParent;
    private GameObject hallwaysParent;
    int index = 0;

    [Header("Hallway")]
    public GameObject hallway;

    public List<GameObject> placeableTiles;
    private List<GameObject> placeables = new List<GameObject>();

    private PathFinding pathFinding = new PathFinding();
    private bool overlapChecked = true;
    private bool hallwaysGen = true;
    private bool wait = false;
    private bool pauseGen = false;

    public float maxLevelLoadTime = 10f;
    private bool levelLoaded = false;

    [Header("Canvas Components")]
    public GameObject generatingLevelPanel;
    public TextMeshProUGUI generatingStatusText;

    void Awake()
    {
        grid = GetComponent<Grid>();
        gridPts.Add(Vector3Int.FloorToInt(placedTiles[0].transform.position), placedTiles[0].gameObject);

        foreach (GameObject tile in placeableTiles)
            placeables.Add(tile);

        placedTilesParent = new GameObject("Tiles");
        hallwaysParent = new GameObject("Hallways");

        StartCoroutine(LevelLoadTimer());
    }

    private void LateUpdate()
    {
        if (placedTiles.Count < numRooms && placeableTiles.Count != 0 && pauseGen == false)
            StartCoroutine(DelayLevelGen());
        else if (overlapChecked == false && wait == false)
            CheckOverlap();
        else if (hallwaysGen == false && overlapChecked == true)
        {
            hallwaysGen = true;
            UpdateGenText("Loading Hallways");

            foreach (TileData tile in placedTiles)
                tile.LoadEnemies();

            //add tiles to the parent to keep things organized
            for (int i = 1; i < placedTiles.Count; i++)
                placedTiles[i].transform.parent = placedTilesParent.transform;

            bool check = GenerateHallways();

            if (check)
            {
                //enable player in the starting area
                EnablePlayer player = placedTiles[0].GetComponent<EnablePlayer>();
                player.col.enabled = true;
                player.move.enabled = true;
                player.weapon.enabled = true;
            }

            UpdateGenText("Hallways Generated");
            UpdateGenText("Level Loaded");
            StartCoroutine(DisableGenPanel());

            levelLoaded = true;
        }
    }
    
    IEnumerator LevelLoadTimer()
    {
        yield return new WaitForSeconds(maxLevelLoadTime);

        if (levelLoaded == false)
        {
            //add tiles to the parent to keep things organized
            for (int i = 1; i < placedTiles.Count; i++)
                placedTiles[i].transform.parent = placedTilesParent.transform;

            ReloadLevel();
        }
    }

    IEnumerator DelayLevelGen()
    {
        pauseGen = true;
        yield return new WaitForSeconds(pauseDelay);

        GenerateLevel();
        pauseGen = false;
    }

    private void GenerateLevel()
    {
        int loopBreak = 0;
        int connection;
        bool validTile;

        if (placedTiles.Count == 1)
            UpdateGenText("Placing Rooms");

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
                //Debug.Break();
            }

        } while (!validTile);

        if (placeableTiles.Count <= 0 || placedTiles.Count == numRooms)
        {
            pauseGen = true;
            overlapChecked = false;

            UpdateGenText("Rooms Placed");
            UpdateGenText("Checking Overlap");
        }
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

        //newTile.transform.parent = placedTilesParent.transform;

        foreach (Connection con in placedTiles[index].connections)
        {
            if ((con.direction == dir.right && direction == dir.left) ||
                (con.direction == dir.left && direction == dir.right) ||
                (con.direction == dir.front && direction == dir.back) ||
                (con.direction == dir.back && direction == dir.front))
            {
                con.connection = connector.alignPt;
                Destroy(con.wall);
                connector.connection = con.alignPt;
                Destroy(connector.wall);

                con.door.EnableDoors();
                connector.door.EnableDoors();

                break;
            }

        }
        return true;
    }

    private void CheckOverlap()
    {
        wait = true;

        StartCoroutine(PauseGen());
    }

    private bool GenerateHallways()
    {
        hallwaysGen = true;

        Dictionary<Vector3Int, bool> visited = new Dictionary<Vector3Int, bool>();

        foreach (TileData tile in placedTiles)
        {
            foreach (Connection con in tile.connections)
            {
                if (con.connection != null && visited.ContainsKey(grid.WorldToCell(con.connection.position)) != true)
                {
                    if (visited.ContainsKey(grid.WorldToCell(con.alignPt.position)))
                        continue;

                    visited.Add(grid.WorldToCell(con.alignPt.position), true);
                    List<Vector3Int> path = pathFinding.GeneratePath(placedTiles, grid, grid.WorldToCell(con.alignPt.position), grid.WorldToCell(con.connection.transform.position));

                    //if there is an error generating the level restart
                    if (path == null)
                    {
                        Debug.Log("ERROR: RELOADING LEVEL");
                        StartCoroutine(DelayReload());
                        return false;
                    }

                    if (path != null)
                    {
                        foreach (Vector3Int p in path)
                        {
                            GameObject tmp = Instantiate(hallway);
                            tmp.transform.position = grid.CellToWorld(p);
                            hallways.Add(tmp.GetComponent<Hallways>());
                            tmp.transform.parent = hallwaysParent.transform;
                        }
                    }
                }
            }
        }

        StartCoroutine(DeleteWalls());
        return true;
    }

    IEnumerator DelayReload()
    {
        UpdateGenText("ERROR: Reloading Level");

        yield return new WaitForSeconds(1f);

        ReloadLevel();
        generatingStatusText.text = "";
    }

    void ReloadLevel()
    {
        //clear dictionary
        gridPts.Clear();

        //handle the starting zone
        TileData tmp = placedTiles[0];
        tmp.transform.position = grid.WorldToCell(Vector3.zero);
        placedTiles.RemoveAt(0);
        placedTiles.Clear();
        placedTiles.Add(tmp);

        gridPts.Add(Vector3Int.FloorToInt(placedTiles[0].transform.position), placedTiles[0].gameObject);

        foreach (GameObject tile in placeables)
            placeableTiles.Add(tile);

        hallways.Clear();

        Destroy(placedTilesParent);
        Destroy(hallwaysParent);

        placedTilesParent = new GameObject("Tiles");
        hallwaysParent = new GameObject("Hallways");

        index = 0;

        overlapChecked = true;
        hallwaysGen = true;
        wait = false;
        pauseGen = false;
    }

    IEnumerator PauseGen()
    {
        yield return new WaitForSeconds(pauseDelay);

        overlapChecked = true;
        hallwaysGen = false;

        Vector3 overlapPt;
        //check each placed tiles
        foreach (TileData tile in placedTiles)
        {
            overlapPt = tile.CheckContains(placedTiles);
            if (tile.overlap != null || overlapPt != Vector3.up)
            {
                overlapChecked = false;
                hallwaysGen = true;
                Vector3Int gridPt = Vector3Int.FloorToInt(tile.transform.position);
                gridPt.y = 0;

                int cellSize = Mathf.RoundToInt(grid.cellSize.x);
                Vector3Int[] neighborGridpts = new Vector3Int[8] {new Vector3Int(-cellSize, 0, cellSize), new Vector3Int(0, 0, cellSize), new Vector3Int(cellSize, 0, cellSize),
                                                                    new Vector3Int(-cellSize, 0, 0), /*CENTER,*/ new Vector3Int(cellSize, 0, 0),
                                                                    new Vector3Int(-cellSize, 0, -cellSize), new Vector3Int(0, 0, -cellSize), new Vector3Int(cellSize, 0, -cellSize)
                                                                    };

                for (int i = 0; i < neighborGridpts.Length; i++)
                    Debug.DrawLine(gridPt, neighborGridpts[i] + gridPt, Color.green, 2f);
                //    neighborGridpts[i] = grid.WorldToCell(neighborGridpts[i]);

                Vector3 collisionPt;

                if (tile.overlap != null)
                {
                    collisionPt = tile.overlap.position;
                }
                else
                {
                    collisionPt = overlapPt;
                }

                float dist = 0, movePt1;
                Vector3 newPt = tile.transform.position;

                for (int i = 0; i < neighborGridpts.Length; i++)
                {
                    movePt1 = Vector3.Distance(neighborGridpts[i] + gridPt, collisionPt);
                    
                    if (movePt1 > dist)
                    {
                        dist = movePt1;
                        newPt = Vector3Int.FloorToInt(neighborGridpts[i] + gridPt);
                    }
                }

                tile.transform.position = newPt;
            }
        }

        wait = false;
    }

    IEnumerator DeleteWalls()
    {
        yield return new WaitForSeconds(pauseDelay);

        foreach (TileData tile in placedTiles)
        {
            foreach (Connection con in tile.connections)
            {

            }
        }

        foreach (Hallways hall in hallways)
        {
            hall.ConnectHallways();
        }
    }

    void UpdateGenText(string status)
    {
        generatingStatusText.text += status + "\n";
    }

    IEnumerator DisableGenPanel()
    {
        yield return new WaitForSeconds(0.5f);

        generatingLevelPanel.SetActive(false);
    }
}
