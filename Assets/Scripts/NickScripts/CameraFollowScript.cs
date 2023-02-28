using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    public float offsetX = 0, offsetY = 0, offsetZ = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(playerTransform.position.x+offsetX, offsetY,playerTransform.position.z+offsetZ);
    }
}
