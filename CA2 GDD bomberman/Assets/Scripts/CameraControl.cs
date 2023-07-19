using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform playerOrientation;
    public Transform playerObj;

    float xRot;
    float YRot;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewDr = playerObj.position - new Vector3(transform.position.x, playerObj.position.y, transform.position.z);
        playerOrientation.forward = viewDr.normalized;

    }
}
