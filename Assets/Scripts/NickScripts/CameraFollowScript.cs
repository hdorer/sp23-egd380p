using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    private float offsetX = -9, offsetY = 20;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(playerTransform.position.x+offsetX, offsetY,playerTransform.position.z);
    }
}
