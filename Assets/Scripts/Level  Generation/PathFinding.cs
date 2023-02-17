using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Node
{
    public Vector3Int position;
    public Vector3Int from;

    public Node(Vector3Int pos, Vector3Int f)
    {
        position = pos;
        from = f;
    }
}

public class PathFinding
{
    Vector3Int[] neighborPts = new Vector3Int[4] { new Vector3Int(1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, -1, 0) };
    Vector3Int up = new Vector3Int(0, 2, 0);

    public List<Vector3Int> GeneratePath(List<TileData> rooms, Grid grid, Vector3Int start, Vector3Int target)
    {
        Dictionary<Vector3Int, bool> visited = new Dictionary<Vector3Int, bool>();
        Dictionary<Vector3Int, Node> from = new Dictionary<Vector3Int, Node>();
        List<Node> path = new List<Node>();

        Node tmp = new Node();
        tmp.position = start;
        path.Add(tmp);
        visited.Add(tmp.position, true);

        CheckValid(rooms, grid.CellToWorld(tmp.position) + up);

        int loopBreak = 0;
        while (tmp.position != target)
        {
            if (visited.ContainsKey(tmp.position + neighborPts[0]) != true && CheckValid(rooms, grid.CellToWorld(tmp.position + neighborPts[0]) + up) == false)
            {
                //CheckValid(rooms, grid.CellToWorld(tmp.position + neighborPts[0]) + up);
                path.Add(new Node(tmp.position + neighborPts[0], tmp.position));
                from.Add(tmp.position + neighborPts[0], tmp);
                visited.Add(tmp.position + neighborPts[0] , true);
            }

            if (visited.ContainsKey(tmp.position + neighborPts[1]) != true && CheckValid(rooms, grid.CellToWorld(tmp.position + neighborPts[1]) + up) == false)
            {
                //CheckValid(rooms, grid.CellToWorld(tmp.position + neighborPts[1]) + up);
                path.Add(new Node(tmp.position + neighborPts[1], tmp.position));
                from.Add(tmp.position + neighborPts[1], tmp);
                visited.Add(tmp.position + neighborPts[1], true);
            }

            if (visited.ContainsKey(tmp.position + neighborPts[2]) != true && CheckValid(rooms, grid.CellToWorld(tmp.position + neighborPts[2]) + up) == false)
            {
                //CheckValid(rooms, grid.CellToWorld(tmp.position + neighborPts[2]) + up);
                path.Add(new Node(tmp.position + neighborPts[2], tmp.position));
                from.Add(tmp.position + neighborPts[2], tmp);
                visited.Add(tmp.position + neighborPts[2], true);
            }

            if (visited.ContainsKey(tmp.position + neighborPts[3]) != true && CheckValid(rooms, grid.CellToWorld(tmp.position + neighborPts[3]) + up) == false)
            {
                //CheckValid(rooms, grid.CellToWorld(tmp.position + neighborPts[3]) + up);
                path.Add(new Node(tmp.position + neighborPts[3], tmp.position));
                from.Add(tmp.position + neighborPts[3], tmp);
                visited.Add(tmp.position + neighborPts[3], true);
            }

            SortList(ref path, target);

            path.Remove(tmp);

            if (path.Count <= 0)
            {
                Debug.Log("NO PATH FOUND");
                break;
            }

            tmp = path[0];

            loopBreak++;
            if (loopBreak > 100)
            {
                Debug.Log("LOOP BREAK");
                Debug.Break();
                break;
            }
        }

        Vector3Int backTrack = target;
        Node oneBefore = path[0];

        List<Vector3Int> finishedPath = new List<Vector3Int>();

        while (backTrack != start)
        {
            backTrack = oneBefore.position;
            finishedPath.Add(backTrack);
            from.TryGetValue(oneBefore.position, out oneBefore);
        }

        return finishedPath;
    }

    private void SortList(ref List<Node> path, Vector3Int target)
    {
        if (path.Count - 1 <= 0)
        {
            Debug.Log("PATH EMPTY");
            return;
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            for (int j = i; j < path.Count; j++)
            {
                if (Vector3Int.Distance(path[i].position, target) > Vector3Int.Distance(path[j].position, target))
                {
                    Node tmp = path[i];
                    path[i] = path[j];
                    path[j] = tmp;
                }
            }
        }
    }

    private bool CheckValid(List<TileData> rooms, Vector3 point)
    {
        // true == point is inside of collider
        bool check = false;
        
        foreach (TileData room in rooms)
        {
            check = room.CheckContains(point);

            if (check == true)
                break;
        }

        //if (check == false)
        //    rooms[0].Sphere(point);

        return check;
    }
}
