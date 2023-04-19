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
    [HideInInspector]
    public BoxCollider col;

    public AudioSource doorOpen;
    private bool doorOpened = false;
    private bool doorClosed = true;


    public float activationRange = 3f;
    public float speed = 1.5f;
    public Door[] doors;
    public GameObject revealArea;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= activationRange && locked == false)
        {
            revealArea.SetActive(true);
            OpenDoor();

            if (doorClosed == true)
            {
                float time = doorOpen.time;

                if (doorOpen.isPlaying)
                    doorOpen.Stop();

                doorOpen.PlayScheduled(time);

                doorOpened = true;
                doorClosed = false;
            }
        }
        else
        {
            CloseDoor();

            if (doorOpened == true)
            {
                float time = doorOpen.time;

                if (doorOpen.isPlaying)
                    doorOpen.Stop();

                doorOpen.PlayScheduled(time);

                doorOpened = false;
                doorClosed = true;
            }
        }
    }

    public void EnableDoors()
    {
        foreach (Door door in doors) 
        {
            door.door.SetActive(true);
        }
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
