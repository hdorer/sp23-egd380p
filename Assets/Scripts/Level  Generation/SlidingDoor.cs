using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Door
{
    public GameObject door;
    public Transform closed;
    public Transform open;
}

public class SlidingDoor : MonoBehaviour
{
    [HideInInspector]
    public TileData room;
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public bool locked = false;

    public float activationRange = 3f;
    public float speed = 1.5f;
    public Door[] doors;
    public GameObject revealArea;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= activationRange && locked == false)
        {
            revealArea.SetActive(true);
            OpenDoor();
        }
        else
            CloseDoor();
    }

    void OpenDoor()
    {
        foreach (Door door in doors)
        {
            door.door.transform.position = Vector3.MoveTowards(door.door.transform.position, door.open.position, speed * Time.deltaTime);
        }
    }

    void CloseDoor()
    {
        foreach (Door door in doors)
        {
            door.door.transform.position = Vector3.MoveTowards(door.door.transform.position, door.closed.position, speed * Time.deltaTime);
        }
    }
}
