using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UpdatedPlayerController : MonoBehaviour
{
    public Camera cam;
    public Cinemachine3rdPersonAim tpsAimCam;

    [Header("Grenade")]
    public GameObject bombPrefab;
    public float throwForce = 10f;
    public float throwWindUpRate = 10f;
    public float maxThrowForce = 40f;
    public float throwCooldown = 2.5f;
    float currentThrowCooldown;
    bool onCooldown = false;

    [Header("IceWall")]
    private bool isUsingWall = false;
    public float iceWallCD = 14f;
    public float currentIceWallCD = 0f;
    public KeyCode wallCastKeybind, directionKeybind;
    public float wallRange;
    public GameObject iceWallPreview, iceWallObject;
    public LayerMask layermask;
    private bool direction, casting;



    [SerializeField] private Transform debugTransform;

    public Transform firePoint;

    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, cam.nearClipPlane);
        Ray ray = cam.ViewportPointToRay(viewportCenter);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, tpsAimCam.AimCollisionFilter))
        {
            debugTransform.position = raycastHit.point;
        }



        if (Input.GetButton("Fire1") && !isUsingWall)
        {
            if (throwForce >= maxThrowForce) return;
            throwForce += throwWindUpRate * Time.deltaTime;
        }

        if (Input.GetButtonUp("Fire1") && !isUsingWall)
        {
            if (!onCooldown)
            {
                ThrowBomb();
                currentThrowCooldown = throwCooldown;
                onCooldown = true;
            }
            throwForce = 10f;
            
        }

        //bomb cooldown timer
        if (onCooldown && currentThrowCooldown > 0.001)
        {
            currentThrowCooldown -= Time.deltaTime;
        }
        else onCooldown = false;


        if (casting) CastingIceWall();

        if (Input.GetKeyDown(wallCastKeybind) && currentIceWallCD <- 0)
        {
            casting = !casting; //casting = false;
            if (!casting) iceWallPreview.SetActive(false);
            isUsingWall = true;

           
        }
        else currentIceWallCD -= Time.deltaTime;
        

    }

    void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, firePoint.position, transform.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();

        //quaternion.lookRot(aimDir);

        Vector3 aimDirection = (debugTransform.position - firePoint.position).normalized;
        rb.AddForce(aimDirection * throwForce, ForceMode.VelocityChange);

        Debug.Log(aimDirection);

    }

    void CastingIceWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, wallRange, layermask))
        {
            if (!iceWallPreview.activeSelf)
            {
                iceWallPreview.SetActive(true);
            }

            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            if (direction) rotation.y = 1; //direction is toggle
            else rotation.y = 0;

            iceWallPreview.transform.localRotation = rotation;
            iceWallPreview.transform.position = hit.point;

            if (Input.GetButtonUp("Fire1"))
            {
                Instantiate(iceWallObject, hit.point, iceWallPreview.transform.rotation);
                casting = false;
                isUsingWall = false;
                iceWallPreview.SetActive(false);
                currentIceWallCD = iceWallCD;
            }

        }
        else { iceWallPreview.SetActive(false); }


        if (Input.GetKeyDown(directionKeybind))
        {
            direction = !direction;
        }

    }
}
