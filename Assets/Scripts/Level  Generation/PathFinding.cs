using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    Vector3[] neighborPts = new Vector3[4] { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(-1, 0, 0), new Vector3(0, -1, 0) };

    List<Vector3> GeneratePath(List<TileData> rooms, Grid grid, Transform start, Transform target)
    {
        StartCoroutine(FindPath(rooms, grid, start, target));

        return null;
    }

    IEnumerator FindPath(List<TileData> rooms, Grid grid, Transform start, Transform target)
    {
        Vector3Int tmp = grid.WorldToCell(start.position);

        yield return null;

        while (tmp != grid.WorldToCell(target.position))
        {

        }
    }
}
