using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    Grid grid;

    public int numRooms = 9;
    public List<GameObject> placedTiles;

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

    private void GenerateLevel()
    {
        for (int i = 0; i < numRooms; i++)
        {
            int roomChoice = Random.Range(0, 3);
            GameObject tile = null;

            switch (roomChoice)
            {
                case 0:
                    tile = Instantiate(smallRooms[0]);
                    break;
                case 1:
                    tile = Instantiate(mediumRooms[0]);
                    break;
                case 2:
                    tile = Instantiate(largeRooms[0]);
                    break;
            }

            if (tile != null)
                tile.transform.position = grid.WorldToCell(placedTiles[placedTiles.Count - 1].transform.position);

        }
    }
}
