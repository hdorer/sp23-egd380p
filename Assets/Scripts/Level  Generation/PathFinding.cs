using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Node
{
    public Vector3Int position;

    public Node(Vector3Int pos)
    {
        position = pos;
    }
}

public class PathFinding
{
    Vector3Int[] neighborPts = new Vector3Int[4] { new Vector3Int(1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, -1, 0) };
    Vector3Int up = new Vector3Int(0, 2, 0);

    Dictionary<Vector3Int, bool> visited = new Dictionary<Vector3Int, bool>();
    Dictionary<Vector3Int, Node> from = new Dictionary<Vector3Int, Node>();
    PriorityQueue<Node, float> queue = new PriorityQueue<Node, float>();

    int maxPathDepth = 100;

    public List<Vector3Int> GeneratePath(List<TileData> rooms, Grid grid, Vector3Int start, Vector3Int target)
    {
        // clear previous values from data structures
        visited.Clear();
        from.Clear();
        queue.Clear();

        Node tmp = new Node();
        tmp.position = start;
        queue.Enqueue(tmp, Vector3.Distance(tmp.position, target));
        visited.Add(tmp.position, true);

        CheckValid(rooms, grid.CellToWorld(tmp.position) + up);

        int loopBreak = 0;
        Vector3Int tmpPos;

        while (tmp.position != target)
        {
            for (int i = 0; i < neighborPts.Length; i++)
            {
                tmpPos = tmp.position + neighborPts[i];
                if (visited.ContainsKey(tmpPos) != true && CheckValid(rooms, grid.CellToWorld(tmpPos) + up) == false)
                {
                    queue.Enqueue(new Node(tmpPos), Vector3.Distance(tmpPos, target));
                    from.Add(tmpPos, tmp);
                    visited.Add(tmpPos, true);
                }
            }

            //check if loop should continue
            if (CheckLoop(ref loopBreak, maxPathDepth))
                return null;

            //pop the top
            tmp = queue.Dequeue();
        }

        return BackTrack(start, target);
    }

    // Start at the target location and work backwards towards the start
    private List<Vector3Int> BackTrack(Vector3Int start, Vector3Int target)
    {
        // start at the target
        Vector3Int backTrack = target;
        Node oneBefore = new Node(target);
        List<Vector3Int> finishedPath = new List<Vector3Int>();

        finishedPath.Add(backTrack);

        while (backTrack != start)
        {
            // Move along the from Dictionary until the start location is reached
            backTrack = oneBefore.position;
            finishedPath.Add(backTrack);
            from.TryGetValue(oneBefore.position, out oneBefore);
        }

        return finishedPath;
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

    private bool CheckLoop(ref int loopBreak, int maxPathDepth)
    {
        //check if queue is empty
        if (queue.Count <= 0)
        {
            Debug.Log("NO PATH FOUND");
            return true;
        }

        //check if loop might be infinite
        loopBreak++;
        if (loopBreak > maxPathDepth)
        {
            Debug.Log("LOOP BREAK");
            //Debug.Break();
            return true;
        }

        return false;
    }
}
