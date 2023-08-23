using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public float sensX, sensY;

    public Transform follow, Target;
    float mouseX, mouseY;

    public Vector2 mousePos;

    [Header("AimCamera")]
    [SerializeField] private CinemachineVirtualCamera aimCamera;

    public LevelManager lvlManager;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lvlManager = FindObjectOfType<LevelManager>();
    }



    private void Update()
    {
        if (lvlManager != null)
        {
            if (lvlManager.gameOver) return;
        }

        mouseX  += mousePos.x * sensX;
        mouseY  -= mousePos.y * sensY;

        mouseY = Mathf.Clamp(mouseY, -45, 80);

        follow.rotation = Quaternion.Euler(0, mouseX, 0);
        Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }



    public void AdsMode(bool aimstate)
    {
        if (aimstate)
        {
            Debug.Log("test");
            aimCamera.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("ran");
            aimCamera.gameObject.SetActive(false);
        }
    }


}
