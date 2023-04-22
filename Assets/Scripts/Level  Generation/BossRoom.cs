using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    private TileData tile;
    private bool loadedEnemies = false;
    public GameObject roomExit;

    // Start is called before the first frame update
    void Start()
    {
        tile = GetComponent<TileData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tile.enemies.Count > 0)
            loadedEnemies = true;

        if (tile.enemies.Count == 0 && loadedEnemies == true && roomExit.activeSelf == false)
            roomExit.SetActive(true);
    } 
}
