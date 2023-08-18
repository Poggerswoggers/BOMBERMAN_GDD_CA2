using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public float sensX, sensY;

    public Transform follow, Target;
    float mouseX, mouseY;


    [Header("AimCamera")]
    [SerializeField] private CinemachineVirtualCamera aimCamera;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    private void Update()
    {
        mouseX  += Input.GetAxisRaw("Mouse X") * sensX;
        mouseY  -= Input.GetAxisRaw("Mouse Y") * sensY;

        mouseY = Mathf.Clamp(mouseY, -45, 80);

        follow.rotation = Quaternion.Euler(0, mouseX, 0);
        Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        
    }


    public void AdsMode(bool aimstate)
    {
        if (aimstate)
        {
            aimCamera.gameObject.SetActive(true);
        }
        else
        {
            aimCamera.gameObject.SetActive(false);
        }
    }


}
