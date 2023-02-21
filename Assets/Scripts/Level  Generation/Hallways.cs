using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallways : MonoBehaviour
{
    TileData tileData;

    //static Grid grid;

    private void Awake()
    {
        tileData = GetComponent<TileData>();
        //grid = FindObjectOfType<Grid>();
    }

    public void ConnectHallways()
    {
        foreach (Connection con in tileData.connections)
        {
            if (Physics.CheckSphere(con.alignPt.position, 0.5f))
            {
                Destroy(con.wall);
            }
        }
    }
}
